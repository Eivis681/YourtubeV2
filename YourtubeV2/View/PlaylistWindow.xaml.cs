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
using YourtubeV2.Models;
using YourtubeV2.Service;
using YourtubeV2.View;

namespace YourtubeV2
{
    /// <summary>
    /// Interaction logic for PlaylistWindow.xaml
    /// </summary>
    public partial class PlaylistWindow : Window
    {
        private readonly IDatabase _database;

        private List<PlaylistList> playlistLists;
        public PlaylistWindow()
        {
            InitializeComponent();
            _database = new Database(); 
            UpdateInterface();
        }

        private void Mp3Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Mp4Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Mp4Window mp4Window = new Mp4Window();
            mp4Window.Show();
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            DetailsWindow detailsWindow = new DetailsWindow();
            detailsWindow.Show();
        }

        private void UpdateInterface()
        {
            PlaylistListView.Items.Clear();
            List<PlaylistList> list = _database.GetPlaylist();
            for (int i = 0; i < list.Count;i++)
            {
                PlaylistListView.Items.Add(list[i].PlaylistName);
            }
            playlistLists = list;
        }

        private void CreatePlaylist_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Visible;
            StackPanel.Visibility = Visibility.Collapsed;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _database.CreatePlaylist(PlaylistName.Text);
            InputBox.Visibility = Visibility.Collapsed;
            StackPanel.Visibility = Visibility.Visible;
            UpdateInterface();
        }

        private void CancleButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Collapsed;
            StackPanel.Visibility = Visibility.Visible;
        }
        private void listView_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            var selectedIndex = PlaylistListView.SelectedIndex;
            UserGetSet.input();
            UserGetSet.SelectedPlaylistName = item.ToString();
            UserGetSet.SelectedPlaylistId = playlistLists[selectedIndex].Id;
            this.Hide();
            PlaylistDetaislWindow playlist = new PlaylistDetaislWindow();
            playlist.Show();
        }
        //private void SaveButton_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void cancleButton_Click(object sender, RoutedEventArgs e)
        //{
        //    InputBox.Visibility = Visibility.Collapsed;
        //}
        //Visibility="Collapsed"
    }
}
