using System.Collections.Generic;
using System.Data.SQLite;
using YourtubeV2.Models;
using YoutubeExplode;

namespace YourtubeV2.Service
{

    public class Database : IDatabase
    {
        SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=Yourtube.db; Version = 3; New = True; Compress = True; ");
        public void GetApiKey()
        {
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT ApiKey FROM Details";
            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
            UserGetSet.input();
            while (sqlite_datareader.Read())
            {
                UserGetSet.ApiKey = sqlite_datareader.GetString(0);
            }
            sqlite_conn.Close();
        }

        public List<PlaylistList> GetPlaylist()
        {
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM Playlist";
            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
            List<PlaylistList> list = new List<PlaylistList>();
            while (sqlite_datareader.Read())
            {
                list.Add(new PlaylistList { Id = sqlite_datareader.GetInt32(0), PlaylistName = sqlite_datareader.GetString(1) });
            }
            sqlite_conn.Close();
            return list;
        }

        public List<SongList> GetSongs(int playlistId)
        {
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM Songs WHERE PlaylistId = ?";
            sqlite_cmd.Parameters.AddWithValue("PlaylistId", playlistId);
            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
            List<SongList> list = new List<SongList>();
            while (sqlite_datareader.Read())
            {
                list.Add(new SongList { Id = sqlite_datareader.GetInt32(0), SongId = sqlite_datareader.GetString(1), SongName = sqlite_datareader.GetString(2), Downloaded = sqlite_datareader.GetString(3), PlaylistId = sqlite_datareader.GetInt32(4), Select = false });
            }
            sqlite_conn.Close();
            return list;
        }

        public void CreatePlaylist(string playlistName)
        {
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO Playlist (PlaylistName) Values (?)";
            sqlite_cmd.Parameters.AddWithValue("PlaylistName", playlistName);
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }
        public void DeletePlaylist(int playlistId)
        {
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM Playlist WHERE Id = ?";
            sqlite_cmd.Parameters.AddWithValue("Id", playlistId);
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "DELETE FROM Songs WHERE PlaylistId = ?";
            sqlite_cmd.Parameters.AddWithValue("PlaylistId", playlistId);
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "DELETE FROM PlaylistDetails WHERE PlaylistId = ?";
            sqlite_cmd.Parameters.AddWithValue("PlaylistId", playlistId);
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void UpdateApiKey(string apiKey)
        {
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE Details SET ApiKey = ? WHERE Id = 1";
            sqlite_cmd.Parameters.AddWithValue("ApiKey", apiKey);
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void DeleteSongs(List<JsonList> deleteSongs)
        {
            sqlite_conn.Open();
            UserGetSet.input();
            foreach (var item in deleteSongs)
            {
                SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "DELETE FROM Songs WHERE SongId = ? AND PlaylistId = ?";
                sqlite_cmd.Parameters.AddWithValue("SongId", item.VideoId);
                sqlite_cmd.Parameters.AddWithValue("PlaylistId", UserGetSet.SelectedPlaylistId);
                sqlite_cmd.ExecuteNonQuery();
            }
            sqlite_conn.Close();
        }

        public void RenamePlaylist(string newPlaylistName)
        {
            sqlite_conn.Open();
            UserGetSet.input();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "UPDATE Playlist Set PlaylistName = ? Where Id = ?";
            sqlite_cmd.Parameters.AddWithValue("PlaylistName", newPlaylistName);
            sqlite_cmd.Parameters.AddWithValue("Id", UserGetSet.SelectedPlaylistId);
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public async void AddSong(string songId, string url)
        {
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            UserGetSet.input();
            var youtube = new YoutubeClient();
            var vid = await youtube.Videos.GetAsync(url);
            sqlite_cmd.CommandText = $"INSERT INTO Songs (SongId, SongName, Downloaded, PlaylistId) Values (?,?,?,?)";
            sqlite_cmd.Parameters.AddWithValue("SongId", songId);
            sqlite_cmd.Parameters.AddWithValue("SongName", vid.Title);
            sqlite_cmd.Parameters.AddWithValue("Downloaded", "No");
            sqlite_cmd.Parameters.AddWithValue("PlaylistId", UserGetSet.SelectedPlaylistId);
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }
        public void AddSongs(List<JsonList> songList, string playlistId)
        {
            sqlite_conn.Open();
            UserGetSet.input();
            foreach (var song in songList)
            {
                SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = $"INSERT INTO Songs (SongId, SongName, Downloaded, PlaylistId) Values (?,?,?,?)";
                sqlite_cmd.Parameters.AddWithValue("SongId", song.VideoId);
                sqlite_cmd.Parameters.AddWithValue("SongName", song.Title);
                sqlite_cmd.Parameters.AddWithValue("Downloaded", "No");
                sqlite_cmd.Parameters.AddWithValue("PlaylistId", UserGetSet.SelectedPlaylistId);
                sqlite_cmd.ExecuteNonQuery();
            }
            SQLiteCommand sqlite_cmd2 = sqlite_conn.CreateCommand();
            sqlite_cmd2.CommandText = $"INSERT INTO PlaylistDetails (PlaylistCode, PlaylistId) Values (?,?)";
            sqlite_cmd2.Parameters.AddWithValue("PlaylistCode", playlistId);
            sqlite_cmd2.Parameters.AddWithValue("PlaylistId", UserGetSet.SelectedPlaylistId);
            sqlite_cmd2.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        public void UpdateDownloadStatus(List<JsonList> songList)
        {
            sqlite_conn.Open();
            UserGetSet.input();
            foreach (var song in songList)
            {
                SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "UPDATE Songs Set Downloaded = ?  Where PlaylistId = ? AND SongId = ?";
                sqlite_cmd.Parameters.AddWithValue("SongId", song.VideoId);
                sqlite_cmd.Parameters.AddWithValue("PlaylistId", UserGetSet.SelectedPlaylistId);
                sqlite_cmd.Parameters.AddWithValue("Downloaded", "Yes");
                sqlite_cmd.ExecuteNonQuery();
            }
            sqlite_conn.Close();
        }
    }
}
//AIzaSyCBYx5nDeHanit6rpvzhZLSDy52diu7ecI
