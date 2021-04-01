using MahApps.Metro.Controls;
using NaverMovieFinderApp.Helper;
using System;
using System.Collections.Generic;
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


            if (string.IsNullOrEmpty(TxtMovieName.Text))
            {
                Stsresult.Content = "검색할 영화명을 입력 후, 검색버튼을 눌러주세요";
                return;
            }

            Commons.showMessageAsync("결과", $"{TxtMovieName.Text}");
            
        }

        private void TxtMovieName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) BtnSearch_Click(sender, e);
        }
    }
}
