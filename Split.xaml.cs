using Microsoft.Win32;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace Alchemist
{
    public partial class Split : Page
    {
        private string selectedFolderPath;
        private string selectedFilePath;
        
        public Split()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        
        public bool IsSplitButtonEnabled
        {
            get { return !string.IsNullOrEmpty(selectedFolderPath) && !string.IsNullOrEmpty(selectedFilePath); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
            {
                selectedFolderPath = dialog.SelectedPath;
                statusTextBlock.Text = "Выбрана папка: " + selectedFolderPath;
            }
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == true)
            {
                selectedFilePath = dialog.FileName;
                statusTextBlock.Text = "Выбран файл: " + selectedFilePath;
            }
        }
       

        private void SplitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFolderPath))
            {
                statusTextBlock.Text = "Не выбрана папка для сохранения страниц.";
                statusTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrEmpty(selectedFilePath))
            {
                statusTextBlock.Text = "Не выбран файл для разделения.";
                statusTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (!Path.GetExtension(selectedFilePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                statusTextBlock.Text = "Выбранный файл не является файлом формата PDF.";
                statusTextBlock.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                // Открываем PDF-файл
                using (PdfDocument document = PdfReader.Open(selectedFilePath, PdfDocumentOpenMode.Import))
                {
                    statusTextBlock.Text = "";
                    statusTextBlock.Visibility = Visibility.Collapsed;
                    // Создаем папку для сохранения страниц
                    string outputFolderPath = Path.Combine(selectedFolderPath, Path.GetFileNameWithoutExtension(selectedFilePath));
                    Directory.CreateDirectory(outputFolderPath);

                    // Разделяем файл на отдельные страницы
                    for (int pageIndex = 0; pageIndex < document.PageCount; pageIndex++)
                    {
                        // Создаем новый документ с одной страницей
                        PdfDocument pageDocument = new PdfDocument();
                        pageDocument.AddPage(document.Pages[pageIndex]);

                        // Генерируем имя файла для сохранения
                        string outputFilePath = Path.Combine(outputFolderPath, $"Page{pageIndex + 1}.pdf");

                        // Сохраняем страницу в новый файл
                        pageDocument.Save(outputFilePath);

                        statusTextBlock.Text = "Файл успешно разделен на страницы и сохранен.";
                    }

                    
                }
            }
            catch (Exception ex)
            {
                statusTextBlock.Text = "Ошибка при разделении файла: " + ex.Message;
                statusTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
