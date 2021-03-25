using System;
using System.Collections.Generic;
using System.Net;
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

namespace YourtubeV2
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        public DetailsWindow()
        {
            InitializeComponent();
            UserGetSet.input();
            ApiKeyTextBox.Text = UserGetSet.ApiKey;
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

        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PlaylistWindow playlistWindow = new PlaylistWindow();
            playlistWindow.Show();
        }

        private void EditBut_Click(object sender, RoutedEventArgs e)
        {
            if(EditBut.Content.ToString() == "Edit Key")
            {
                ApiKeyTextBox.IsReadOnly = false;
                EditBut.Content = "Save";
            }
            else if (EditBut.Content.ToString() == "Save")
            {
                try
                {
                    string url = "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=50&playlistId=PLYQ21GguN2ssJfHlwxZxzepHaiu3t798Y&key=" + ApiKeyTextBox.Text;
                    WebClient wc = new WebClient();
                    string urlData = wc.DownloadString(url);
                    Database database = new Database();
                    database.UpdateApiKey(ApiKeyTextBox.Text);
                    EditBut.Content = "Edit Key";
                    ApiKeyTextBox.IsReadOnly = true;
                    MessageBox.Show("Api key has been changed");
                }
                catch
                {
                    MessageBox.Show("Api key is invalid");
                }
            }
            
        }
    }
}
