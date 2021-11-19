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
        public MainWindow()
        {
            InitializeComponent();
            //MessageBox.Show("Test", "Test", MessageBoxButton.OK, MessageBoxImage.Error);
            //Parser parser = new("");

            //try
            //{
            //    parser.A();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Exception: {ex.Message}");
            //}

            //foreach (var ouput in parser.Output)
            //{
            //    Console.WriteLine(ouput);
            //    Thread.Sleep(1000);
            //}
        }
    }
}
