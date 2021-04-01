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

namespace WpfSMSApp.View.Store
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddStore : Page
    {
        public AddStore()
        {
            InitializeComponent();
        }

        


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LblUserIdentityNumber.Visibility = LblUserSurname.Visibility =
                    Visibility.Hidden;

                TxtUserID.Text = TxtUserIdentityNumber.Text = "";


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

        public bool IsValidInput()
        {

            bool isValid = true; // 입력된 값이 모두 만족하는지 판별하는 플래그


            if (string.IsNullOrEmpty(TxtUserIdentityNumber.Text))
            {
                LblUserIdentityNumber.Visibility = Visibility.Visible;
                LblUserIdentityNumber.Text = "사번을 입력하세요";
                isValid = false;
            }
            else
            {
                var cnt = Logic.DataAccess.GetUsers().Where(u => u.UserIdentityNumber.Equals(TxtUserIdentityNumber.Text)).Count();
                if (cnt > 0)
                {
                    LblUserIdentityNumber.Visibility = Visibility.Visible;
                    LblUserIdentityNumber.Text = "중복된 사번이 존재합니다";
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(TxtUserSurname.Text))
            {
                LblUserSurname.Visibility = Visibility.Visible;
                LblUserSurname.Text = "이름(성)을 입력하세요";
                isValid = false;
            }

            

            return isValid;

        }



        

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
            
        {
            bool isValid = true; // 입력된 값이 모두 만족하는지 판별하는 플래그
            LblUserIdentityNumber.Visibility = LblUserSurname.Visibility =
                   Visibility.Hidden;


            var user = new Model.User();


            isValid = IsValidInput();




            if (isValid)
            {
                //MessageBox.Show("DB 입력 처리!");

                user.UserIdentityNumber = TxtUserIdentityNumber.Text;
                user.UserSurname = TxtUserSurname.Text;
              
                
                try
                {
                    // 패스워드값 암호화
                    var mdhash = MD5.Create();
                    user.UserPassword = Commons.GetMd5Hash(mdhash, user.UserPassword);

                    var result = Logic.DataAccess.SetUser(user);
                    if (result == 0)
                    {
                        // 수정안됨
                    }
                    else
                    {
                       // NavigationService.Navigate(new UserList());
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
