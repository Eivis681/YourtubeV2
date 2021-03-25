using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YourtubeV2
{
    /// <summary>
    /// Interaction logic for Mp4Window.xaml
    /// </summary>
    public partial class Mp4Window : Window
    {
        public Mp4Window()
        {
            InitializeComponent();
        }

        private void Mp3Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PlaylistWindow playlistWindow = new PlaylistWindow();
            playlistWindow.Show();
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            DetailsWindow detailsWindow = new DetailsWindow();
            detailsWindow.Show();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Cancel)
            {
                return;
            }
            DirectoryTextBox.Text = dialog.FileName;
        }
    }
}
