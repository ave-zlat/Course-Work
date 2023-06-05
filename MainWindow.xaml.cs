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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alchemist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            contentFrame.Navigate(new Home());
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton != null && radioButton.IsChecked == true)
            {
                string buttonContent = radioButton.Name.ToString();

                switch (buttonContent)
                {
                    case "Home":
                        contentFrame.Navigate(new Home());
                        break;
                    case "Merge":
                        contentFrame.Navigate(new Merge());
                        break;
                    case "Split":
                        contentFrame.Navigate(new Split());
                        break;
                    case "Convert":
                        contentFrame.Navigate(new Convert());
                        break;
                    case "Edit":
                        contentFrame.Navigate(new Edit());
                        break;
                    case "Delete":
                        contentFrame.Navigate(new Delete());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
