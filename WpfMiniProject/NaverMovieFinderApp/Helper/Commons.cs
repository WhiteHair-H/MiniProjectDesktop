using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;


namespace NaverMovieFinderApp.Helper
{
    public class Commons
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
    }




    

}
