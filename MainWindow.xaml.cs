using System.Windows;
using System.Windows.Controls;

namespace Alchemist
{
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
                        break;;
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
