using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using Logger = NLog.Logger;



namespace NaverMovieFinderApp.Helper
{
    public class Commons
    {
        /// <summary>
        /// 즐겨찾기 여부 플래그
        /// </summary>
        public static bool IsFavorite = false;

        public static bool IsDelete = false; // 즐겨찾기 삭제와 보기 플래그

        //  NLOG 정적객체
        public static  Logger LOGGER = LogManager.GetCurrentClassLogger();

        public static async Task<MessageDialogResult> showMessageAsync(
            string title, string message , MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(title, message, style, null);
        }
        public static string GetRequestApi(string openApiUrl, string clientID, string clientSecret)
        {
            string result = "";

            try
            {
                WebRequest request = WebRequest.Create(openApiUrl);
                request.Headers.Add("X-Naver-Client-Id", clientID);
                request.Headers.Add("X-Naver-Client-Secret", clientSecret);

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                result = reader.ReadToEnd();

                reader.Close();
                stream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외발생 : {ex}");
            }

            return result;
        }

        public static string StripHtmlTag(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", ""); //  html 특수문자 제거하는 정규표현식 
        }


        public static string StripPipe(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            else
                return text.Substring(0, text.LastIndexOf("|")).Replace("|", ", ");

            //string result = text.Replace("|", ", ");
            //return result.Substring(0, result.LastIndexOf(",")) ;
        }
    }




    

}
