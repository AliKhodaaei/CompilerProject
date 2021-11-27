using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectCore;

namespace ProjectUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Parser parser;
        private BoardManager manager;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            string input = TbInput.Text.Replace("\r\n", "\n");
            input = string.Concat(input, '\n');
            parser = new Parser(input);
            try
            {
                parser.A();
                manager = new BoardManager(Board, Bot, Zero, parser, LblLocation, TbOutput, Road);
                manager.InitializeBoard();
                await manager.Move(parser.Output);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbInput.Text = "//Write your code here...";
        }
    }
}
