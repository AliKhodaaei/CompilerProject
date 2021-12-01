﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ProjectCore;

namespace ProjectUI
{
    public partial class MainWindow : Window
    {
        private readonly int BoardSize = 450;
        private Point CurrentLocation;
        private Point CLP;

        public void InitializeBoard()
        {
            var max = new List<int>() { Math.Abs(parser.Lx), Math.Abs(parser.Ux), Math.Abs(parser.Ly), Math.Abs(parser.Uy) }.Max() * 2 + 1;
            Board.RowDefinitions.Clear();
            Board.ColumnDefinitions.Clear();
            for (int i = 0; i < max; i++)
            {
                Board.RowDefinitions.Add(new RowDefinition());
                Board.ColumnDefinitions.Add(new ColumnDefinition());
            }
            Bot.Visibility = Visibility.Visible;
            Zero.Visibility = Visibility.Visible;
            Grid.SetRow(Zero, (max - 1) / 2);
            Grid.SetColumn(Zero, (max - 1) / 2);
            Grid.SetRow(Bot, (max - 1) / 2 - parser.Oy);
            Grid.SetColumn(Bot, (max - 1) / 2 + parser.Ox);
            Road.StrokeThickness = (BoardSize / max) / 3;
            CurrentLocation = new Point(parser.Ox, parser.Oy);
            PlaceWalls(max);
        }

        public async Task Move(List<string> output)
        {
            TbOutput.Text = "";
            await Dispatcher.BeginInvoke(new Action(() => UpdateRoad()), DispatcherPriority.ContextIdle, null);
            Road.Data = Geometry.Parse($"M {CLP.X} {CLP.Y}");
            foreach (var o in output)
            {
                if (o.Contains("("))
                {
                    Point p = Point.Parse(o.Replace("(", "").Replace(")", ""));
                    int dx = Convert.ToInt32(p.X - CurrentLocation.X);
                    int dy = Convert.ToInt32(p.Y - CurrentLocation.Y);
                    Grid.SetColumn(Bot, Grid.GetColumn(Bot) + dx);
                    Grid.SetRow(Bot, Grid.GetRow(Bot) - dy);
                    CurrentLocation = p;
                    LblLocation.Content = $"Current Location: ({p})";
                    TbOutput.Inlines.Add(new Run(o + "\n") { Foreground = new SolidColorBrush(Colors.LightGreen) });
                    Fade(Bot, 0);
                    await Dispatcher.BeginInvoke(new Action(() => UpdateRoad()), DispatcherPriority.ContextIdle, null);
                    await Task.Delay(1000);
                }
                else if (o.Contains("ERROR"))
                {
                    TbOutput.Inlines.Add(new Run(o + "\n") { Foreground = new SolidColorBrush(Colors.IndianRed) });
                }
            }
            TbOutput.Inlines.Add(new Run(output[^2] + "\n") { Foreground = new SolidColorBrush(Colors.Orange) });
            TbOutput.Inlines.Add(new Run(output[^1] + "\n") { Foreground = new SolidColorBrush(Colors.Orange) });
        }

        private void PlaceWalls(int max)
        {
            foreach (var w in _walls)
                Board.Children.Remove(w);
            _walls.Clear();

            Point p = new((max - 1) / 2, (max - 1) / 2);
            foreach (var (x, y) in parser.Walls)
            {
                Rectangle w = new() { Fill = new SolidColorBrush(Colors.LightGray) };
                Grid.SetRow(w, Convert.ToInt32(p.Y - y));
                Grid.SetColumn(w, Convert.ToInt32(p.X + x));
                Board.Children.Add(w);
                _walls.Add(w);
            }
        }

        private static void Fade(Image bot, int type)
        {
            var fade = new DoubleAnimation(type, 1 - type, TimeSpan.FromMilliseconds(300));
            Storyboard.SetTarget(fade, bot);
            Storyboard.SetTargetProperty(fade, new PropertyPath(UIElement.OpacityProperty));
            var sb = new Storyboard();
            sb.Children.Add(fade);
            sb.Begin();
        }

        private void UpdateRoad()
        {
            CLP = Bot.TranslatePoint(new Point(Bot.ActualWidth / 2, Bot.ActualHeight / 2), Board);
            Road.Data = Geometry.Parse($"{Road.Data} L {CLP.X},{CLP.Y}");
        }
    }
}