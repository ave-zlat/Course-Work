using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;
using System.Text;

namespace Alchemist
{
    public partial class Delete : Page
    {
        private string pdfFilePath;
        public Delete()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";

            if (openFileDialog.ShowDialog() == true)
            {
                pdfFilePath = openFileDialog.FileName;
                filePathTextBlock.Text = pdfFilePath;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pdfFilePath))
            {
                MessageBox.Show("Please select a PDF file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(pdfFilePath))
            {
                MessageBox.Show("Please select a PDF file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(pageNumberTextBox.Text, out int pageNumber) || pageNumber <= 0)
            {
                MessageBox.Show("Please enter a valid page number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DeleteSinglePage(pdfFilePath, pageNumber);

            MessageBox.Show("Page deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        
        private void DeleteSinglePage(string filePath, int pageNumber)
        {
            string outputFilePath = GetOutputFilePath(filePath);

            using (PdfDocument document = PdfReader.Open(filePath, PdfDocumentOpenMode.Modify))
            {
                if (pageNumber > document.PageCount)
                {
                    MessageBox.Show("Invalid page number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                document.Pages.RemoveAt(pageNumber - 1);
                document.Save(outputFilePath);
            }
        }


        private string GetOutputFilePath(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string outputFileName = $"{fileName}_pagedeleted.pdf";
            return Path.Combine(directory, outputFileName);
        }
    }
}
