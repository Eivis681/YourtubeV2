using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using YourtubeV2.Models;

namespace YourtubeV2.Api
{
    class GetSongIdTitle
    {
        public List<JsonList> SongId(string urll)
        {
            string[] playlistId = urll.Split('=');
            string url = "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=50&playlistId=" + playlistId[1] + "&key=AIzaSyCBYx5nDeHanit6rpvzhZLSDy52diu7ecI";
            WebClient wc = new WebClient();
            string urlData = wc.DownloadString(url);
            List<JsonList> resultList = deserialeRequest(urlData, playlistId[1]);

            return resultList;
        }
        public List<JsonList> deserialeRequest(string requestData, string url)
        {
            List<JsonList> reqestDataList = new List<JsonList>();
            Root data = JsonConvert.DeserializeObject<Root>(requestData);
            if (data.nextPageToken != null)
            {
                GetNextPage(data.nextPageToken, url, reqestDataList);
            }
            foreach (var obj in data.items)
            {
                reqestDataList.Add(new JsonList { Title = obj.snippet.title, VideoId = obj.snippet.resourceId.videoId });
            }

            return reqestDataList;
        }

        public List<JsonList> GetNextPage(string pageToken, string url, List<JsonList> reqestDataList)
        {
            string uri = "https://youtube.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=50&pageToken=" + pageToken + "&playlistId=" + url + "&key=AIzaSyCBYx5nDeHanit6rpvzhZLSDy52diu7ecI";
            WebClient wc = new WebClient();
            string urlData = wc.DownloadString(uri);
            Root data = JsonConvert.DeserializeObject<Root>(urlData);
            foreach (var obj in data.items)
            {
                reqestDataList.Add(new JsonList { Title = obj.snippet.title, VideoId = obj.snippet.resourceId.videoId });
            }
            if (data.nextPageToken != null)
            {
                GetNextPage(data.nextPageToken, url, reqestDataList);
            }
            return reqestDataList;
        }

        public class Default
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Medium
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class High
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Standard
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Thumbnails
        {
            public Default @default { get; set; }
            public Medium medium { get; set; }
            public High high { get; set; }
            public Standard standard { get; set; }
        }

        public class ResourceId
        {
            public string kind { get; set; }
            public string videoId { get; set; }
        }

        public class Snippet
        {
            public DateTime publishedAt { get; set; }
            public string channelId { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public Thumbnails thumbnails { get; set; }
            public string channelTitle { get; set; }
            public string playlistId { get; set; }
            public int position { get; set; }
            public ResourceId resourceId { get; set; }
            public string videoOwnerChannelTitle { get; set; }
            public string videoOwnerChannelId { get; set; }
        }

        public class Item
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string id { get; set; }
            public Snippet snippet { get; set; }
        }

        public class PageInfo
        {
            public int totalResults { get; set; }
            public int resultsPerPage { get; set; }
        }

        public class Root
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string nextPageToken { get; set; }
            public List<Item> items { get; set; }
            public PageInfo pageInfo { get; set; }
        }
    }
}
