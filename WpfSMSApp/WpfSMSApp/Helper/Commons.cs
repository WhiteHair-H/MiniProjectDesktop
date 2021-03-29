using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSMSApp.Model;

namespace WpfSMSApp
{

    public class Commons
    {
    // NLOG 정적 인스턴스 생성
    public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        // 로그인한 유저 정보
        public static User LOGINED_USER;

    }
}
