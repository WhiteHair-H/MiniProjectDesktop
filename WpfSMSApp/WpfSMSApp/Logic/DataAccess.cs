using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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


        /// <summary>
        /// 입력, 수정 동시에
        /// </summary>
        /// <param name="user"></param>
        /// <returns>0 또는 1이상</returns>
        public static int SetUser(User user)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.User.AddOrUpdate(user);

                return ctx.SaveChanges();
            }
        }

        public static List<Store> GetStores()
        {
            List<Store> stores;

            using (var ctx = new SMSEntities())
            {
                stores = ctx.Store.ToList();
            }

            return stores;
        }

        public static List<Stock> GetStocks()
        {
            List<Stock> stocks;

            using (var ctx = new SMSEntities())
            {
                stocks = ctx.Stock.ToList();
            }

            return stocks;
        }
        public static int SetStore(Store store)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.Store.AddOrUpdate(store);
                return ctx.SaveChanges();
            }
        }

    }
}