using System.IO;
using System.Linq;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace YourtubeV2.Downloads
{
    public delegate void ProgressBarVideo(int data);
    class VideoDownloads
    {
        public readonly ProgressBarVideo progressBarVideo;
        public VideoDownloads(ProgressBarVideo progressBar)
        {
            progressBarVideo = progressBar;
        }
        public async void DownloadVideo(string url, string directory)
        {
            int counter = 0;
            var youtube = new YoutubeClient();
           
            var videoID = url.Split('=');
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoID[1]);
            var vid = await youtube.Videos.GetAsync(url);

            var streamInfo = streamManifest.GetMuxed().WithHighestVideoQuality();

            //var audioStreamInfo = streamManifest.GetAudio().WithHighestBitrate();
            //var videoStreamInfo = streamManifest.GetVideo().FirstOrDefault(s => s.VideoQualityLabel == "1080p");
            //if(videoStreamInfo == null)
            //{
            //    videoStreamInfo = streamManifest.GetVideo().FirstOrDefault(s => s.VideoQualityLabel == "720p");
            //}
            //if (videoStreamInfo == null)
            //{
            //    videoStreamInfo = streamManifest.GetVideo().FirstOrDefault(s => s.VideoQualityLabel == "480p");
            //}
            //if (videoStreamInfo == null)
            //{
            //    videoStreamInfo = streamManifest.GetVideo().FirstOrDefault(s => s.VideoQualityLabel == "360p");
            //}

            //var streamInfo = new IStreamInfo[] { audioStreamInfo, videoStreamInfo };
   
            if (streamInfo != null)
            {
                //var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
                //File.Create("asd.mp4");
                //await youtube.Videos.DownloadAsync(streamInfo, new ConversionRequestBuilder(directory).Build());//.SetFFmpegPath($"{directory}\\").
                await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{directory}\\{vid.Title}.mp4");
            }
            //{vid.Title}.mp4
            counter++;
            progressBarVideo.Invoke(counter);

        }
    }
}
//string data = Directory.GetCurrentDirectory() + "\\";

//File.Move(data + vid.Title + ".mp3", directory + "\\" + vid.Title + ".mp3");