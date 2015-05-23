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
            browseButton.Click += BrowseButton_Click;
            submitButton.Click += SubmitButton_Click;
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
            throw new System.NotImplementedException();
        }

    }
}
