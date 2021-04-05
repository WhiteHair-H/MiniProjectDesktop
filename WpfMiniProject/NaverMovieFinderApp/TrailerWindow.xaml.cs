using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MahApps.Metro.Controls;
using NaverMovieFinderApp.Helper;
using NaverMovieFinderApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NaverMovieFinderApp
{
    /// <summary>
    /// TrailerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TrailerWindow : MetroWindow
    {

        List<YoutubeItem> Youtubes; // 유튜브 APi 검색결과 담을 리스트

        public TrailerWindow()
        {
            InitializeComponent();
        }

        public TrailerWindow(string moiveName) : this()
        {
            LblMovieName.Content = $"{moiveName} 예고편";
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Youtubes = new List<YoutubeItem>(); // 초기화

            // 유튜브 API로 검색
            //MessageBox.Show("유튜브 검색");
            ProcessSearchYoutubeApi();


        }

        private async void ProcessSearchYoutubeApi()
        {
            await LoadDataCollection();
            LsvYoutubeSearch.ItemsSource = Youtubes;
        }

        private async Task LoadDataCollection()
        {
            var youtubeService = new YouTubeService(
                new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyBws9qCw7wjp-jsYeUWNX_MImMGjFl3e1Y",
                    ApplicationName = this.GetType().ToString()
                }) ;

            var request = youtubeService.Search.List("snippet");
            request.Q = (string)LblMovieName.Content; // {영화이름} 예고편  
            request.MaxResults = 10; // 사이즈가 크면 멈춰버림

            var response = await request.ExecuteAsync(); // 검색시작(Youtube OpenApi 호출)


            foreach (var item in response.Items)
            {
                if (item.Id.Kind == "youtube#video")
                {
                    YoutubeItem youtube = new YoutubeItem()
                    {
                        Title = item.Snippet.Title,
                        Author = item.Snippet.ChannelTitle,
                        URL = $"https://www.youtube.com/watch?v={item.Id.VideoId}"

                    };

                    // 썸네일 이미지
                    youtube.Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url, UriKind.RelativeOrAbsolute));


                    Youtubes.Add(youtube);



                }
            }

        }

        private void LsvYoutubeSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LsvYoutubeSearch.SelectedItems.Count == 0)
            {
                Commons.showMessageAsync("유튜브보기", "예고편을 볼 영화를 선택하세요");
            }

            if (LsvYoutubeSearch.SelectedItems.Count > 1)
            {
                Commons.showMessageAsync("유튜브보기", "예고편을 하나만 선택하세요");
            }

            if (LsvYoutubeSearch.SelectedItem is YoutubeItem)
            {
                var video = LsvYoutubeSearch.SelectedItem as YoutubeItem;
                BrwYoutubeWatch.Source = new Uri(video.URL , UriKind.RelativeOrAbsolute);
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BrwYoutubeWatch.Source = null; // 해제
            BrwYoutubeWatch.Dispose(); // 리소스 즉시해제
        }
    }
}
