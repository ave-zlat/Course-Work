using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using Ookii.Dialogs.Wpf;

namespace Alchemist
{
    public partial class Convert : Page
    {
        private string outputFolderPath;
        private string[] selectedImagesPaths;
        public Convert()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        private void SelectOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog folderBrowserDialog = new VistaFolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == true)
            {
                outputFolderPath = folderBrowserDialog.SelectedPath;
            }
        }

        private void SelectImagesButton_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog openFileDialog = new VistaOpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagesPaths = openFileDialog.FileNames;
                selectedImagesTextBox.Text = $"{selectedImagesPaths.Length} изображений выбрано";
            }
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
           if (string.IsNullOrEmpty(outputFolderPath) || string.IsNullOrEmpty(outputFileNameTextBox.Text) || selectedImagesPaths.Length == 0)
            {
                MessageBox.Show("Please select a folder, provide a file name, and choose at least one image.");
                return;
            }

            string outputFilePath = Path.Combine(outputFolderPath, outputFileNameTextBox.Text + ".pdf");

            try
            {
                PdfDocument document = new PdfDocument();
                foreach (string imagePath in selectedImagesPaths)
                {
                    XImage image = XImage.FromFile(imagePath);
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawImage(image, 0, 0, page.Width, page.Height);
                }
                document.Save(outputFilePath);
                document.Close();
                outputTextBox.Text = $"PDF file saved to: {outputFilePath}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
