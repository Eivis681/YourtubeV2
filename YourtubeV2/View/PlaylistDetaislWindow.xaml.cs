using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YourtubeV2.Api;
using YourtubeV2.Downloads;
using YourtubeV2.Models;
using YourtubeV2.Service;
using YoutubeExplode;

namespace YourtubeV2.View
{
    /// <summary>
    /// Interaction logic for PlaylistDetaislWindow.xaml
    /// </summary>
    public partial class PlaylistDetaislWindow : Window
    {
        List<SongList> Item = new List<SongList>();
        List<JsonList> downloadSongsList = new List<JsonList>();
        private readonly IDatabase _database;
        private readonly SongDownloads songDownloads;
        public PlaylistDetaislWindow()
        {
            InitializeComponent();
            songDownloads = new SongDownloads(SongProgressBar);
            _database = new Database();
            UserGetSet.input();
            PlaylistNameLabel.Text = $"Playlist {UserGetSet.SelectedPlaylistName} ";
            UpdateInterface();
            SongGrid.ItemsSource = Item;

        }
        private void BackButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            PlaylistWindow playlist = new PlaylistWindow();
            playlist.Show();
        }
        private void UpdateInterface()
        {
            Item = _database.GetSongs(UserGetSet.SelectedPlaylistId);
        }

        private void deletePlaylist_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this playlist ?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _database.DeletePlaylist(UserGetSet.SelectedPlaylistId);
                this.Hide();
                PlaylistWindow playlist = new PlaylistWindow();
                playlist.Show();
            }
        }

        private void downloadAllSongs_Click(object sender, RoutedEventArgs e)
        {
            downloadSongsList.Clear();
            DirectoryWindow.Visibility = Visibility.Visible;
            progressBarSong.Maximum = Item.Count;
            foreach (var item in Item)
            {
                downloadSongsList.Add(new JsonList { VideoId = item.SongId, Title = item.SongName });
            }
        }

        private void DownloadSelected_Click(object sender, RoutedEventArgs e)
        {
            var downloadList = Item.Where(x => x.Select == true).ToList();
            if (downloadList.Count == 0)
            {
                MessageBox.Show("Please select songs");
                return;
            }
            progressBarSong.Maximum = downloadList.Count;
            downloadSongsList.Clear();
            foreach (var item in downloadList)
            {
                downloadSongsList.Add(new JsonList { VideoId = item.SongId, Title = item.SongName });
            }
            DirectoryWindow.Visibility = Visibility.Visible;
        }

        private void DownloadNew_Click(object sender, RoutedEventArgs e)
        {
            var newSongs = Item.Where(x => x.Downloaded == "No").ToList();
            progressBarSong.Maximum = newSongs.Count;
            downloadSongsList.Clear();
            foreach (var item in newSongs)
            {
                downloadSongsList.Add(new JsonList { VideoId = item.SongId, Title = item.SongName });
            }
            DirectoryWindow.Visibility = Visibility.Visible;
        }

        private void DelteSelected_Click(object sender, RoutedEventArgs e)
        {
            var deleteList = Item.Where(x => x.Select == true).ToList();
            if (deleteList.Count == 0)
            {
                MessageBox.Show("Please select songs");
            }
            var result = MessageBox.Show("Are you sure you want to delete these songs ?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                List<JsonList> list = new List<JsonList>();
                foreach (var item in deleteList)
                {
                    list.Add(new JsonList { VideoId = item.SongId, Title = item.SongName });
                }
                _database.DeleteSongs(list);
                MessageBox.Show("Songs have been deleted");
                UpdateInterface();
                SongGrid.ItemsSource = Item;
            }
        }

        private void AddSongs_Click(object sender, RoutedEventArgs e)
        {
            AddSongsWindow.Visibility = Visibility.Visible;
        }

        private void UpdatePlaylist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RenamePlaylist_Click(object sender, RoutedEventArgs e)
        {
            PlaylistName.Text = UserGetSet.SelectedPlaylistName;
            RenamePlaylists.Visibility = Visibility.Visible;
        }

        private void SongGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Select")
            {
                e.Column.IsReadOnly = false;
                e.Column.Header = "";
            }
            if (e.Column.Header.ToString() == "Id")
            {
                e.Cancel = true;
            }
            if (e.Column.Header.ToString() == "SongId")
            {
                e.Cancel = true;
            }
            if (e.Column.Header.ToString() == "PlaylistId")
            {
                e.Cancel = true;
            }
        }
        public void SongProgressBar(int number)
        {
            progressBarSong.Value = number;
            if (progressBarSong.Value == progressBarSong.Maximum)
            {
                _database.UpdateDownloadStatus(downloadSongsList);
                UpdateInterface();
                MessageBox.Show("All songs have been downloaded");
                SongGrid.ItemsSource = Item;
                progressBarSong.Value = 0;
            }
        }

        private void CancleDownloadButton_Click(object sender, RoutedEventArgs e)
        {
            AddSongsWindow.Visibility = Visibility.Hidden;
            RenamePlaylists.Visibility = Visibility.Hidden;
            DirectoryWindow.Visibility = Visibility.Hidden;
        }

        private void DownloadSongsButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Directory.Text))
            {
                MessageBox.Show("Please select a directory");
                return;
            }
            songDownloads.DownloadSongs(downloadSongsList, Directory.Text);
            DirectoryWindow.Visibility = Visibility.Hidden;
            //TODO: update songs that have been downloaded 
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Cancel)
            {
                return;
            }
            Directory.Text = dialog.FileName;
        }

        private void SaveNewSongs_Click(object sender, RoutedEventArgs e)
        {
            var array = UrlTextBox.Text.Split('?');
            if(string.IsNullOrEmpty(UrlTextBox.Text))
            {
                MessageBox.Show("Please fill in the box");
            }
            if (array[0] != "https://www.youtube.com/watch" && array[0] != "https://www.youtube.com/playlist")
            {
                MessageBox.Show("We only support Youtube.com links");
                return;
            }
            if(array[0] == "https://www.youtube.com/watch")
            {
                var songIds = UrlTextBox.Text.Split('=');
                _database.AddSong(songIds[1], UrlTextBox.Text);
                MessageBox.Show("Song has been added");
                UpdateInterface();
                SongGrid.ItemsSource = Item;
            }
            if(array[0] == "https://www.youtube.com/playlist")
            {
                GetSongIdTitle getSong = new GetSongIdTitle();
                List<JsonList> songs = getSong.SongId(UrlTextBox.Text);
                var  playlistId = UrlTextBox.Text.Split('=');
                _database.AddSongs(songs, playlistId[1]);
                MessageBox.Show("Songs have been added");
                UpdateInterface();
                SongGrid.ItemsSource = Item;
            }
            AddSongsWindow.Visibility = Visibility.Hidden;
        }

        private void RenamePlaylistBut_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageBox.Show("Please fill in the box");
            }
            else
            {
                _database.RenamePlaylist(PlaylistName.Text);
                PlaylistNameLabel.Text = $"Playlist {PlaylistName.Text}";
                RenamePlaylists.Visibility = Visibility.Hidden;
            }
        }
    }
}
