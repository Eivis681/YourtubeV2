using System;
using System.Collections.Generic;
using System.Text;
using YourtubeV2.Models;

namespace YourtubeV2.Service
{
    public interface IDatabase
    {
        void GetApiKey();
        List<PlaylistList> GetPlaylist();
        List<SongList> GetSongs(int playlistId);
        void CreatePlaylist(string playlistName);
        void UpdateApiKey(string apiKey);
        void DeletePlaylist(int playlistId);
        void DeleteSongs(List<JsonList> deleteSongs);
        void RenamePlaylist(string newPlaylistName);
        void AddSong(string songId, string songName);
        void AddSongs(List<JsonList> songList, string playlistId, bool addPlaylist);
        void UpdateDownloadStatus(List<JsonList> songList);
        List<string> GetPlaylistId();

    }
}
