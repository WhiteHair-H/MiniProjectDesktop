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

namespace WpfSMSApp.View.Account
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditAccount : Page
    {
        public EditAccount()
        {
            InitializeComponent();
        }

        


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LblUserIdentityNumber.Visibility = LblUserSurname.Visibility =
                    LblUserName.Visibility = LblUserEmail.Visibility 
                    = LblUserPassword.Visibility = LblUserAdmin.Visibility
                    = LblUserActivated.Visibility = Visibility.Hidden;


                //콤보박스 초기화
                List<string> comboValues = new List<string>
                {
                    "False", // index값 : 0
                    "True" // index값 : 1
                };
                CboUserAdmin.ItemsSource = comboValues;
                CboUserActivated.ItemsSource = comboValues;



                var user = Commons.LOGINED_USER;
                TxtUserID.Text = user.UserID.ToString();
                TxtUserIdentityNumber.Text = user.UserIdentityNumber.ToString();
                TxtUserSurname.Text = user.UserSurname.ToString();
                TxtUserName.Text = user.UserName.ToString();
                TxtUserEmail.Text = user.UserEmail.ToString();
                //TxtUserPassword.Password = user.UserPassword; 
                CboUserAdmin.SelectedIndex = user.UserAdmin == false ? 0 : 1;
                CboUserActivated.SelectedIndex = user.UserActivated == false ? 0 : 1;


                //TxtUserAdmin.Text = user.UserAdmin.ToString();
                //TxtUserActivated.Text = user.UserActivated.ToString();
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

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true; // 입력된 값이 모두 만족하는지 판별하는 플래그
            LblUserIdentityNumber.Visibility = LblUserSurname.Visibility =
                   LblUserName.Visibility = LblUserEmail.Visibility
                   = LblUserPassword.Visibility = LblUserAdmin.Visibility
                   = LblUserActivated.Visibility = Visibility.Hidden;

            var user = Commons.LOGINED_USER;

            if (string.IsNullOrEmpty(TxtUserSurname.Text))
            {
                LblUserSurname.Visibility = Visibility.Visible;
                LblUserSurname.Text = "이름(성)을 입력하세요";
                isValid = false;
            }

            if (string.IsNullOrEmpty(TxtUserName.Text))
            {
                LblUserName.Visibility = Visibility.Visible;
                LblUserName.Text = "이름을 입력하세요";
                isValid = false;
            }
            if (string.IsNullOrEmpty(TxtUserEmail.Text))
            {
                LblUserEmail.Visibility = Visibility.Visible;
                TxtUserEmail.Text = "이메일을 입력하세요";
                isValid = false;
            }
            if (string.IsNullOrEmpty(TxtUserPassword.Password))
            {
                LblUserPassword.Visibility = Visibility.Visible;
                LblUserPassword.Text = "패스워드를 입력하세요";
                isValid = false;
            }

            if (isValid)
            {
                //MessageBox.Show("DB 수정 처리!");
                user.UserSurname = TxtUserSurname.Text;
                user.UserName = TxtUserName.Text;
                user.UserEmail = TxtUserEmail.Text;
                user.UserPassword = TxtUserPassword.Password;
                user.UserAdmin = bool.Parse(CboUserAdmin.SelectedValue.ToString());
                user.UserActivated = bool.Parse(CboUserActivated.SelectedValue.ToString());





                try
                {
                    // 패스워드값 암호화
                    var mdhash = MD5.Create();
                    user.UserPassword = Commons.GetMd5Hash(mdhash, user.UserPassword);

                    var result = Logic.DataAccess.SetUser(user);
                    if (result == 0)
                    {
                        // 수정안됨
                        LblResult.Text = "계정 수정에 문제가 발생했습니다. 관리자에게 문의바랍니다.";
                        LblResult.Foreground = Brushes.OrangeRed;
                    }
                    else
                    {
                        // 정상적 수정
                        LblResult.Text = "정상적으로 수정했습니다.";
                        LblResult.Foreground = Brushes.DeepSkyBlue;
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
