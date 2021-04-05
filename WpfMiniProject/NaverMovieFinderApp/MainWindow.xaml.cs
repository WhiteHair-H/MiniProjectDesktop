using MahApps.Metro.Controls;
using NaverMovieFinderApp.Helper;
using NaverMovieFinderApp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;

namespace NaverMovieFinderApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Stsresult.Content = "";
            ImgPoster.Source = new BitmapImage(new Uri("No_Picture.jpg", UriKind.RelativeOrAbsolute));


            if (string.IsNullOrEmpty(TxtMovieName.Text))
            {
                Stsresult.Content = "검색할 영화명을 입력 후, 검색버튼을 눌러주세요";
                Commons.showMessageAsync("검색", "검색할 영화명을 입력 후, 검색버튼을 눌러주세요");
                return;
            }



            try
            {
                ProcessSearchNaverApi(TxtMovieName.Text);
                Commons.showMessageAsync("검색", "영화검색 완료");
            }
            catch (Exception ex)
            {
                Commons.showMessageAsync("예외", $"예외발생 : {ex}");
                // 로그 추가 commons
            }

            Commons.IsFavorite = false; // 즐겨찾기 아님


            //Commons.showMessageAsync("결과", $"{TxtMovieName.Text}");

        }

        private void ProcessSearchNaverApi(string movieName)
        {
            string clientID = "xQofXLMDFh2ic38jZyYI";
            string clientSecret = "pSneKCSHPF";
            string openApiUrl = $"https://openapi.naver.com/v1/search/movie?start=1&display30&query={movieName}";

            string resJson = Commons.GetRequestApi(openApiUrl, clientID, clientSecret);
            var parseJson = JObject.Parse(resJson);

            int total = Convert.ToInt32(parseJson["total"]);
            int display = Convert.ToInt32(parseJson["display"]);

            Stsresult.Content = $"{total} 중 {display} 호출성공";

            var items = parseJson["items"];
            var Json_array = (JArray)items;

            List<MovieItem> movieItems = new List<MovieItem>();
            foreach (var item in Json_array)
            {
                MovieItem movie = new MovieItem(
                    Commons.StripHtmlTag(item["title"].ToString()),
                    item["link"].ToString(),
                    item["image"].ToString(),
                    item["subtitle"].ToString(),
                    item["pubDate"].ToString(),
                    Commons.StripPipe(item["director"].ToString()),
                    Commons.StripPipe(item["actor"].ToString()),
                    item["userRating"].ToString());
                movieItems.Add(movie);
            }
            this.DataContext = movieItems;

        }

        private void TxtMovieName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) BtnSearch_Click(sender, e);
        }

        private void GrdData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (GrdData.SelectedItem == null)
            //{
            //    Commons.showMessageAsync("오류", "영화를 선택하세요");
            //    return;
            //}

            if (GrdData.SelectedItem is MovieItem)
            {
                var movie = GrdData.SelectedItem as MovieItem;
                //Commons.showMessageAsync("결과", $"{movie.Image}");

                if (string.IsNullOrEmpty(movie.Image))
                {
                    ImgPoster.Source = new BitmapImage(new Uri("No_Picture.jpg" , UriKind.RelativeOrAbsolute));
                    
                }
                else
                {
                    ImgPoster.Source = new BitmapImage(new Uri(movie.Image, UriKind.RelativeOrAbsolute));
                }

            }

            if (GrdData.SelectedItem is NaverFavoriteMovies)
            {
                var movie = GrdData.SelectedItem as NaverFavoriteMovies;
                //Commons.showMessageAsync("결과", $"{movie.Image}");

                if (string.IsNullOrEmpty(movie.Image))
                {
                    ImgPoster.Source = new BitmapImage(new Uri("No_Picture.jpg", UriKind.RelativeOrAbsolute));

                }
                else
                {
                    ImgPoster.Source = new BitmapImage(new Uri(movie.Image, UriKind.RelativeOrAbsolute));
                }

            }

        }

        private void BtnAddWatchList_Click(object sender, RoutedEventArgs e)
        {
            if(GrdData.SelectedItems.Count == 0)
            {
                Commons.showMessageAsync("오류", "즐겨찾기에 추가할 영화를 선택하세요(복수선택 가능)");
                return;
            }

            if (Commons.IsFavorite == true)
            {
                //이미 즐겨찾기한 내용을 막아주기 위한 것
                Commons.showMessageAsync("즐겨찾기", "즐겨찾기 조회내용을 다시 즐겨찾기 할 수 없습니다");
                return;
            }



            List<NaverFavoriteMovies> list = new List<NaverFavoriteMovies>();
            foreach (MovieItem item in GrdData.SelectedItems)
            {
                NaverFavoriteMovies temp = new NaverFavoriteMovies()
                {
                    Title = item.Title,
                    Link = item.Link,
                    Image = item.Image,
                    SubTitle = item.SubTitle,
                    PubDate = item.PubDate,
                    Director = item.Director,
                    Actor = item.Actor,
                    UserRating = item.UserRating,
                    RegDate = DateTime.Now
                    
                };

                list.Add(temp);

            }


            try
            {

                using (var ctx = new OpenApiLabEntities())
                {
                    //ctx.NaverFavoriteMovies.AddRange(list); 아래와 차이없음
                    ctx.Set<NaverFavoriteMovies>().AddRange(list);
                    ctx.SaveChanges();
                }


                Commons.showMessageAsync("저장", $"즐겨찾기 추가");


            }
            catch (Exception ex)
            {
                Commons.showMessageAsync("예외", $"예외발생 : {ex}");
                Commons.LOGGER.Error($"예외발생 : {ex}");
            }

        }

        private void BtnViewWatchList_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
            TxtMovieName.Text = "";

            //List<MovieItem> listData = new List<MovieItem>();
            List<NaverFavoriteMovies> list = new List<NaverFavoriteMovies>();

            try
            {
                using (var ctx = new OpenApiLabEntities())
                {
                list =  ctx.NaverFavoriteMovies.ToList();
                }
                this.DataContext = list;
                Stsresult.Content = $"즐겨찾기 {list.Count}개 조회";
                if (Commons.IsDelete == false)
                    Commons.showMessageAsync("즐겨찾기", $"즐겨찾기 {list.Count}개 조회완료");
                else
                    Commons.showMessageAsync("즐겨찾기", "즐겨찾기 삭제완료");

                Commons.IsFavorite = true; // 즐겨찾기 맞음
            }
            catch (Exception ex)
            {
                Commons.showMessageAsync("예외", $"예외발생 : {ex}");
                Commons.LOGGER.Error($"예외발생 : {ex}");
                Commons.IsFavorite = false; // 한번더 명시적으로 처리
            }
            Commons.IsDelete = false;


            // 변환 필요없음

            //foreach (var item in list)
            //{
            //    listData.Add(new MovieItem(
            //        item.Title,
            //        item.Link,
            //        item.Image,
            //        item.SubTitle,
            //        item.PubDate,
            //        item.Director,
            //        item.Actor,
            //        item.UserRating
                   
            //        ));
            //}
            //this.DataContext = listData;
            //Stsresult.Content = $"즐겨찾기 {listData.Count}개 조회";
            //Commons.showMessageAsync("즐겨찾기", $"즐겨찾기 {listData.Count}개 조회");
            //Commons.IsFavorite = true; // 즐겨찾기 맞음

        }

        private void BtnDeleteWatchList_Click(object sender, RoutedEventArgs e)
        {
            if (Commons.IsFavorite == false)
            {
                Commons.showMessageAsync("즐겨찾기", "즐겨찾기 내용이 아니면 삭제할 수 없습니다");
                return;
            }

            if (GrdData.SelectedItems.Count == 0)
            {
                Commons.showMessageAsync("즐겨찾기", "삭제할 즐겨찾기 영화를 선택하세요");
                return;
            }

            // List<NaverFavoriteMovies> removeList = new List<NaverFavoriteMovies>();
            foreach (NaverFavoriteMovies item in GrdData.SelectedItems)
            {
                using (var ctx = new OpenApiLabEntities())
                {
                    //ctx.NaverFavoriteMovies.Remove(item);
                    var itemDelete = ctx.NaverFavoriteMovies.Find(item.Idx); // 삭제할 영화 객체 검색 후 생성
                    ctx.Entry(itemDelete).State = System.Data.Entity.EntityState.Deleted; // 검색객체 상태를 삭제로 변경
                    ctx.SaveChanges(); // commit
                }
            }

            Commons.IsDelete = true;

            // 조회쿼리 다시
            BtnViewWatchList_Click(sender, e);



        }

        private void BtnWatchTrailer_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItems.Count == 0)
            {
                Commons.showMessageAsync("유튜브영화", "영화를 선택하세요");
                return;
            }

            if (GrdData.SelectedItems.Count > 1)
            {
                Commons.showMessageAsync("유튜브영화", "영화는 1개만 선택하세요");
                return;
            }

            string movieName = "";

            if (Commons.IsFavorite) // 즐겨찾기
            {
                var item = GrdData.SelectedItem as NaverFavoriteMovies;
                //MessageBox.Show(item.Link);
                movieName = item.Title;
            }
            else // 네이버 api
            {
                var item = GrdData.SelectedItem as MovieItem;
                //MessageBox.Show(item.Link);
                movieName = item.Title;
            }


            var trailerWindow = new TrailerWindow(movieName);
            trailerWindow.Owner = this;
            trailerWindow.ShowDialog();


        }

        private void BtnNaverMovieLink_Click(object sender, RoutedEventArgs e)
        {



            if (GrdData.SelectedItems.Count == 0)
            {
                Commons.showMessageAsync("네이버영화", "영화를 선택하세요");
                return;
            }

            if (GrdData.SelectedItems.Count > 1)
            {
                Commons.showMessageAsync("네이버영화", "영화는 1개만 선택하세요");
                return;
            }

            // 선택된 영화 네이버영화 URL 가져오기


            string linkUrl = "";
            if (Commons.IsFavorite) // 즐겨찾기
            {
                var item = GrdData.SelectedItem as NaverFavoriteMovies;
                //MessageBox.Show(item.Link);
                linkUrl = item.Link;
            }
            else // 네이버 api
            {
                var item = GrdData.SelectedItem as MovieItem;
                //MessageBox.Show(item.Link);
                linkUrl = item.Link;
            }

            Process.Start(linkUrl);
          


        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"즐겨찾기 여부는 {Commons.IsFavorite}");
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
