using System;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
using System.Security.Cryptography;

namespace WpfSMSApp.View.User
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DeActive : Page
    {
        public DeActive()
        {
            InitializeComponent();
        }

        


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //콤보박스 초기화
                             

                List<Model.User> users = Logic.DataAccess.GetUsers();
                this.DataContext = users;
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 EditAccount Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnEditMyAccount_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
                

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
            
        {
            
        }

        private void GrdData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
           
            try
            {
                // 선택된 값이 입력창에 나오도록 처리
                var user = GrdData.SelectedItem as Model.User;

               
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 GrdData_SelectedCellsChanged : {ex}");
            }


        }

        private async void BtnDeactive_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true; // 입력된 값이 모두 만족하는지 판별하는 플래그

            if (GrdData.SelectedItem == null)
            {
                await Commons.ShowMessageAsync("오류", "비활성화할 사용자를 선택하세요");
                //MessageBox.Show("비활성화할 사용자를 선택하세요.");
                return;
            }


            if (isValid)
            {
                //MessageBox.Show("DB 입력 처리!");


                try
                {
                    var user = GrdData.SelectedItem as Model.User;
                    user.UserActivated = false; //사용자 비활성화
                   

                    var result = Logic.DataAccess.SetUser(user);
                    if (result == 0)
                    {
                        // 수정안됨
                        await Commons.ShowMessageAsync("오류", "사용자 수정에 실패했습니다");
                        return;

                    }
                    else
                    {
                        NavigationService.Navigate(new UserList());
                    }
                }
                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외발생 : {ex}");
                }

            }
        }
    }
}
