using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace Contagious
{
    /// <summary>
    /// Logika interakcji dla klasy ChangeFolder.xaml
    /// </summary>
    public partial class ChangeFolder : Window
    {
        private ContagiousViewModel CvmCf;
        public ChangeFolder(ContagiousViewModel Cvm)
        {
            InitializeComponent();
            DataContext = Cvm;
            CvmCf = Cvm;
        }

        private void FindFolder(object sender, MouseButtonEventArgs e)
        {
            //FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            //DialogResult result = folderBrowserDialog.ShowDialog();

            //if (!string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            //{
            //    string[] files = Directory.GetFiles(folderBrowserDialog.SelectedPath);
            //    PathTextBox.Content = new Uri(files[0], UriKind.Relative);
            //}

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {

                PathTextBox.Content = dialog.FileName.ToString();
            }
        }

        public void RefreshList(object sender, RoutedEventArgs e)
        {
            CvmCf.dir = PathTextBox.Content.ToString();
            CvmCf.Refresh();
            this.Close();
        }
    }
}
