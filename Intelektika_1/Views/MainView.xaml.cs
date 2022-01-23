using Intelektika_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Intelektika_1.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            //Xceed.Wpf.Toolkit.WatermarkTextBox
            //MainViewModel vm = DataContext as MainViewModel;
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "{height, weight, sport, position} file (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                MainViewModel viewModel = DataContext as MainViewModel;
                viewModel.ReadJsonFile(openFileDialog.FileName);
            }

        }
    }
}
