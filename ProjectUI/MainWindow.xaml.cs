using ProjectCore;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjectUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Parser parser;
        private ParserDTO dto;
        private readonly List<Rectangle> _walls;

        public MainWindow()
        {
            InitializeComponent();
            _walls = new List<Rectangle>();
            dto = new ParserDTO();
        }

        private async void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            BtnRun.IsEnabled = false;
            string input = TbInput.Text.Replace("\r\n", "\n");
            try
            {
                dto = GetDto();
                parser = new Parser(input, dto);
                parser.S();
                LblStatus.Content = "Running...";
                LblStatus.Background = new SolidColorBrush(Colors.DodgerBlue);
                InitializeBoard();
                await Move(parser.Output);
                LblStatus.Content = "Finished!";
                LblStatus.Background = new SolidColorBrush(Colors.ForestGreen);
            }
            catch (Exception ex)
            {
                LblStatus.Content = "Error: " + ex.Message;
                LblStatus.Background = new SolidColorBrush(Colors.Red);
            }
            BtnRun.IsEnabled = true;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbInput.Text = "//Write your commands here...";
        }

        private ParserDTO GetDto()
        {
            dto.SetLx(ILx.Text);
            dto.SetUx(IUx.Text);
            dto.SetLy(ILy.Text);
            dto.SetUy(IUy.Text);
            dto.SetOx(IOx.Text);
            dto.SetOy(IOy.Text);
            dto.Walls.Clear();
            foreach (StackPanel row in LstWalls.Children)
            {
                string wxi = ((TextBox)row.Children[1]).Text;
                string wyi = ((TextBox)row.Children[3]).Text;
                if (wxi == "" || wyi == "") 
                    throw new Exception($"{((Label)row.Children[0]).Content.ToString().Replace(":","")}is empty!");
                dto.AddWall((int.Parse(wxi), int.Parse(wyi)));
            }

            return dto;
        }

        private void IN_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IN.Text == "" || IN.Text == "-") return;
            try
            {
                dto.SetN(IN.Text);
                LstWalls.Children.Clear();
                ScWalls.ScrollToTop();
                for (int i = 1; i <= dto.N; i++)
                {
                    TextBox wallX = new() { Name = $"wx{i}", MinWidth = 30, HorizontalContentAlignment = HorizontalAlignment.Center };
                    TextBox wallY = new() { Name = $"wy{i}", MinWidth = 30, HorizontalContentAlignment = HorizontalAlignment.Center };
                    Label id = new() { Content = $"Wall {i}: " };
                    Label separator = new() { Content = " , ", FontWeight = FontWeights.DemiBold };
                    StackPanel row = new() { Margin = new Thickness(5), Orientation = Orientation.Horizontal };
                    row.Children.Add(id);
                    row.Children.Add(wallX);
                    row.Children.Add(separator);
                    row.Children.Add(wallY);
                    LstWalls.Children.Add(row);
                }
            }
            catch (Exception ex)
            {
                LblStatus.Content = "Error: " + ex.Message;
                LblStatus.Background = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
