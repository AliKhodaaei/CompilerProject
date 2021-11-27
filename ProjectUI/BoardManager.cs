using ProjectCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows.Documents;

namespace ProjectUI
{
    public class BoardManager
    {
        private readonly int _boardSize = 450;
        private Parser _parser;
        private Grid _board;
        private Image _bot, _zero;
        private Label _lblLocation;
        private TextBlock _tbOutput;
        private Path _road;
        private Point CurrentLocation;
        private Point CLP;

        public BoardManager(Grid board, Image bot, Image zero, Parser parser, Label lblLocation, TextBlock tbOutput, Path path)
        {
            _parser = parser;
            _board = board;
            _bot = bot;
            _zero = zero;
            _lblLocation = lblLocation;
            _tbOutput = tbOutput;
            _road = path;
            CurrentLocation = new Point(_parser.Ox, _parser.Oy);
        }

        public object OpacityProperty { get; private set; }

        public void InitializeBoard()
        {
            var max = new List<int>() {Math.Abs(_parser.Lx), Math.Abs(_parser.Ux), Math.Abs(_parser.Ly), Math.Abs(_parser.Uy) }.Max() * 2 + 1;
            _board.RowDefinitions.Clear();
            _board.ColumnDefinitions.Clear();
            for (int i = 0; i < max; i++)
            {
                _board.RowDefinitions.Add(new RowDefinition());
                _board.ColumnDefinitions.Add(new ColumnDefinition());
            }
            _bot.Visibility = Visibility.Visible;
            _zero.Visibility = Visibility.Visible;
            Grid.SetRow(_zero, (max - 1) / 2);
            Grid.SetColumn(_zero, (max - 1) / 2);
            Grid.SetRow(_bot, (max - 1) / 2 - _parser.Oy);
            Grid.SetColumn(_bot, (max - 1) / 2 + _parser.Ox);
            _road.StrokeThickness = (_boardSize / max) / 2.5;
            _road.Data = Geometry.Parse($"M 225 225");
            PlaceWalls(max);
        }

        public async Task Move(List<string> output)
        {
            _tbOutput.Text = "";
            foreach (var o in output)
            {
                if (o.Contains("("))
                {
                    Point p = Point.Parse(o.Replace("(", "").Replace(")", ""));
                    int dx = Convert.ToInt32(p.X - CurrentLocation.X);
                    int dy = Convert.ToInt32(p.Y - CurrentLocation.Y);
                    Grid.SetColumn(_bot, Grid.GetColumn(_bot) + dx);
                    Grid.SetRow(_bot, Grid.GetRow(_bot) - dy);
                    CurrentLocation = p;
                    _lblLocation.Content = $"Current Location: ({p})";
                    _tbOutput.Inlines.Add(new Run(o + "\n") { Foreground = new SolidColorBrush(Colors.LightGreen) });
                    Fade(_bot, 0);
                    await Task.Delay(1000);
                }
                else if (o.Contains("ERROR"))
                {
                    _tbOutput.Inlines.Add(new Run(o + "\n") { Foreground = new SolidColorBrush(Colors.IndianRed) });
                }
            }
            _tbOutput.Inlines.Add(new Run(output[^2] + "\n") { Foreground = new SolidColorBrush(Colors.Orange) });
            _tbOutput.Inlines.Add(new Run(output[^1] + "\n") { Foreground = new SolidColorBrush(Colors.Orange) });
        }

        private void PlaceWalls(int max)
        {
            Point p = new((max - 1) / 2, (max - 1) / 2);
            foreach (var (x, y) in _parser.Walls)
            {
                Rectangle w = new() { Fill = new SolidColorBrush(Colors.LightGray)};
                Grid.SetRow(w, Convert.ToInt32(p.Y - y));
                Grid.SetColumn(w, Convert.ToInt32(p.X + x));
                _board.Children.Add(w);
            }
        }

        private void Fade(Image bot, int type)
        {
            var fade = new DoubleAnimation(type, 1 - type, TimeSpan.FromMilliseconds(500));
            Storyboard.SetTarget(fade, bot);
            Storyboard.SetTargetProperty(fade, new PropertyPath(Image.OpacityProperty));
            var sb = new Storyboard();
            sb.Children.Add(fade);
            sb.Completed += UpdateRoad;
            sb.Begin();
        }

        private void UpdateRoad(object sender, EventArgs e)
        {
            CLP = _bot.TranslatePoint(new Point(_bot.ActualWidth / 2, _bot.ActualHeight / 2), _board);
            _road.Data = Geometry.Parse($"{_road.Data} L {CLP.X},{CLP.Y}");
        }
    }
}
