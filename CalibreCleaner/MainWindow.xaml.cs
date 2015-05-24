using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace CalibreCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            exitMenuItem.Click += ExitMenuItem_Click;
            aboutMenuItem.Click += AboutMenuItem_Click;
            browseButton.Click += BrowseButton_Click;
            submitButton.Click += SubmitButton_Click;
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(
@"CalibreCleaner, version 1.0
by Steve Perkins

http://steveperkins.com
https://github.com/steve-perkins/CalibreCleaner"
                , "About");
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                pathTextBox.Text = dialog.SelectedPath;
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<BookMetadata> booksMissingInDatabase, booksMissingOnFilesystem;
                CleanerService service = new CleanerService();
                service.findMissingBooks(pathTextBox.Text, out booksMissingInDatabase, out booksMissingOnFilesystem);
                missingFromDatabaseDataGrid.ItemsSource = booksMissingInDatabase;
                missingFromFilesystemDataGrid.ItemsSource = booksMissingOnFilesystem;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "An error has occurred");
            }
        }

    }
}
