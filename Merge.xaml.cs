using System.Windows.Controls;
using Ookii.Dialogs.Wpf;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Alchemist
{
    public partial class Merge : Page
    {
        private string outputFolderPath;
        private string[] selectedFilePaths;
        public Merge()
        {
            InitializeComponent();
        }
        private void SelectOutputDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
            {
                outputFolderPath = dialog.SelectedPath;
            }
        }

        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
            {
                selectedFilePaths = dialog.FileNames;
            }
        }

        private void MergeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(outputFolderPath))
            {
                MessageBox.Show("Пожалуйста, выберите директорию для сохранения объединенного файла.");
                return;
            }

            if (string.IsNullOrEmpty(outputFileNameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите имя объединенного файла.");
                return;
            }

            if (selectedFilePaths == null || selectedFilePaths.Length < 2)
            {
                MessageBox.Show("Пожалуйста, выберите несколько файлов для объединения.");
                return;
            }

            if (selectedFilePaths.Any(path => Path.GetExtension(path).ToLower() != ".pdf"))
            {
                MessageBox.Show("Выбранные файлы должны быть формата PDF.");
                return;
            }

            string outputFilePath = Path.Combine(outputFolderPath, outputFileNameTextBox.Text + ".pdf");

            try
            {
                using (PdfDocument outputDocument = new PdfDocument())
                {
                    foreach (string filePath in selectedFilePaths)
                    {
                        using (PdfDocument inputDocument = PdfReader.Open(filePath, PdfDocumentOpenMode.Import))
                        {
                            foreach (PdfPage page in inputDocument.Pages)
                            {
                                outputDocument.AddPage(page);
                            }
                        }
                    }

                    outputDocument.Save(outputFilePath);
                }

                MessageBox.Show("Файл успешно объединен и сохранен по пути: " + outputFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при объединении файлов: " + ex.Message);
            }
        }
    }
}

