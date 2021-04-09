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
using YourtubeV2.Downloads;
using YourtubeV2.Models;

namespace YourtubeV2
{
    /// <summary>
    /// Interaction logic for Mp4Window.xaml
    /// </summary>
    public partial class Mp4Window : Window
    {
        private readonly VideoDownloads videoDownloads;
        
        public Mp4Window()
        {
            InitializeComponent();
            videoDownloads = new VideoDownloads(VideoProgressBar);
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

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string[] array = UrlTextBox.Text.Split('?');
            if (string.IsNullOrEmpty(UrlTextBox.Text))
            {
                MessageBox.Show("Please provide a Url");
                return;
            }
            if (string.IsNullOrEmpty(DirectoryTextBox.Text))
            {
                MessageBox.Show("Please provide a directory");
                return;
            }
            if (array[0] != "https://www.youtube.com/watch" && array[0] != "https://www.youtube.com/playlist")
            {
                MessageBox.Show("We only support Youtube.com links");
                return;
            }
            if(array[0] == "https://www.youtube.com/playlist")
            {
                MessageBox.Show("You can download only one video");
                return;
            }
            if (array[0] == "https://www.youtube.com/watch")
            {
                var result = MessageBox.Show("Do you want to download this one video ?", "Download", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    progressBarVideo.Maximum = 1;
                    var videoId = UrlTextBox.Text.Split('=');
                    videoDownloads.DownloadVideo(UrlTextBox.Text, DirectoryTextBox.Text);
                }
                return;
            }
           
        }

        public void VideoProgressBar(int number)
        {
            progressBarVideo.Value = number;
            if (progressBarVideo.Value == progressBarVideo.Maximum)
            {
                MessageBox.Show("Video have been downloaded");
            }
        }

    }
}
