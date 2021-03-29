using System;
using System.Collections.Generic;
using System.Linq;
using WpfSMSApp.Model;

namespace WpfSMSApp.Logic
{
    internal class DataAccess
    {
        public static List<User> GetUsers()
        {
            List<User> users;

            using (var ctx = new SMSEntities())
            {
                users = ctx.User.ToList();
            }

            return users;
        }
    }
}