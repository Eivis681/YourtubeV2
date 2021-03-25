using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Windows;
using YourtubeV2.Api;
using YourtubeV2.Downloads;
using YourtubeV2.Models;
using YourtubeV2.Service;

namespace YourtubeV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SongDownloads songDownloads;
        private readonly IDatabase _database;
        public MainWindow()
        {
            InitializeComponent();
            songDownloads = new SongDownloads(SongProgressBar);
            _database = new Database();
            _database.GetApiKey();
        }
        private void Mp4Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Mp4Window mp4Window = new Mp4Window();
            mp4Window.Show();
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

        private void DownloadBut_Click(object sender, RoutedEventArgs e)
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
            if (array[0] == "https://www.youtube.com/watch")
            {
                var result = MessageBox.Show("Do you want to download this one song ?", "Download", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    progressBarSong.Maximum = 1;
                    List<JsonList> songId = new List<JsonList>();
                    var songIds = UrlTextBox.Text.Split('=');
                    songId.Add(new JsonList { VideoId = songIds[1] });
                    songDownloads.DownloadSongs(songId, DirectoryTextBox.Text);
                }
                return;
            }
            if (array[0] == "https://www.youtube.com/playlist")
            {
                var result = MessageBox.Show("Do you want to download a song playlist ?", "Download", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    GetSongIdTitle getSongId = new GetSongIdTitle();
                    List<JsonList> resultList = getSongId.SongId(UrlTextBox.Text);
                    progressBarSong.Maximum = resultList.Count;
                    songDownloads.DownloadSongs(resultList, DirectoryTextBox.Text);
                }
            }
        }
        public void SongProgressBar(int number)
        {
            progressBarSong.Value = number;
            if (progressBarSong.Value == progressBarSong.Maximum)
            {
                MessageBox.Show("All songs have been downloaded");
            }
        }
    }
}
