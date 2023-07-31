using Dapper;
using LoginFinal.BL;
using LoginFinal.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoginFinal.DAL
{
    public class OrderDAL
    {

        public List<Order> GetOrdersById(int id, SqlConnection de)
        {
            //return de.Orders.Where(a => (a.BuyerId == id || a.SellerId == id) && a.IsActive == 1 && a.IsAccepted != 0).Include(a => a.Buyer).Include(a => a.Seller).ToList();
            var getOrdersbyId = de.Query<Order>("EXECUTE GetAllRecords Orders,Id,-1, ' Where BuyerId=" + id + " OR SellerId=" + id + " AND IsAccepted <> 0 '").ToList();
            foreach (var x in getOrdersbyId)
            {
                var getbuyer = new UserBL().GetActiveUserById(x.BuyerId, de);
                var getSeller = new UserBL().GetActiveUserById(x.SellerId, de);
                x.Buyer = getbuyer;
                x.Seller = getSeller;
            }
            return getOrdersbyId;
        }


        public List<Order> GetAllOrders(SqlConnection de)
        {
            return de.Query<Order>("EXECUTE GetAllRecords Orders,Id,-1, ' Where IsAccepted <> 0 AND IsActive <> 0 '").ToList();

            //return de.Query<Order>("EXECUTE GetAllRecords Orders").ToList();
        }

        public Order GetOrderById(int id, SqlConnection de)
        {
            //return de.Orders.Where(a => a.Id == id && a.IsActive==1).Include(b=>b.Seller).Include(c=>c.Buyer).FirstOrDefault();
            var getOrdersbyId = de.Query<Order>("EXECUTE GetAllRecords Orders,Id," + id + "").FirstOrDefault();
            getOrdersbyId.Buyer = new UserBL().GetActiveUserById(getOrdersbyId.BuyerId, de);
            getOrdersbyId.Seller = new UserBL().GetActiveUserById(getOrdersbyId.SellerId, de);
            return getOrdersbyId;
        }

        public bool UpdateOrder(Order ed, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetUpdatePropandVal(ed);
                var update = de.Query("EXECUTE InsertOrUpdate " + ed.Id + ",Orders,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int AddOrder(Order order, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(order);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Orders," + getPropandVal[0] + "," + getPropandVal[1] + "").First();
                return Convert.ToInt32(add.Id);
            }
            catch
            {
                return -1;
            }
        }




        public List<Subscription> GetSubscriptionsById(int id, SqlConnection de)
        {
            //return de.Orders.Where(a => (a.BuyerId == id || a.SellerId == id) && a.IsActive == 1 && a.IsAccepted != 0).Include(a => a.Buyer).Include(a => a.Seller).ToList();
            var getSubscriptionsbyId = de.Query<Subscription>("EXECUTE GetAllRecords Subscription,Id,-1, ' Where BuyerId=" + id + " OR SellerId=" + id + " AND IsAccepted <> 0 '").ToList();
            foreach (var x in getSubscriptionsbyId)
            {
                var getbuyer = new UserBL().GetActiveUserById(x.BuyerId, de);
                x.Customer = getbuyer;
            }
            return getSubscriptionsbyId;
        }


        public List<Subscription> GetSubscriptionsAgainstSpecificButler(int cId, int bId, SqlConnection de)
        {
            //return de.Orders.Where(a => (a.BuyerId == id || a.SellerId == id) && a.IsActive == 1 && a.IsAccepted != 0).Include(a => a.Buyer).Include(a => a.Seller).ToList();
            var getSubscriptionsbyId = de.Query<Subscription>("EXECUTE GetAllRecords Subscriptions,Id,-1, ' Where BuyerId=" + cId + " AND SellerId=" + bId + " AND IsSubscribed <> 0 AND IsActive <> 0 '").ToList();
            foreach (var x in getSubscriptionsbyId)
            {
                var getbuyer = new UserBL().GetActiveUserById(x.BuyerId, de);
                x.Customer = getbuyer;
            }
            return getSubscriptionsbyId;
        }


        public List<Subscription> GetAllSubscriptions(SqlConnection de)
        {
            return de.Query<Subscription>("EXECUTE GetAllRecords Subscriptions").ToList();
        }

        public Subscription GetSubscriptionById(int id, SqlConnection de)
        {
            //return de.Orders.Where(a => a.Id == id && a.IsActive==1).Include(b=>b.Seller).Include(c=>c.Buyer).FirstOrDefault();
            var getSubscriptionsbyId = de.Query<Subscription>("EXECUTE GetAllRecords Subscriptions,Id," + id + "").FirstOrDefault();
            getSubscriptionsbyId.Customer = new UserBL().GetActiveUserById(getSubscriptionsbyId.BuyerId, de);
            return getSubscriptionsbyId;
        }

        public bool UpdateSubscription(Subscription sd, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetUpdatePropandVal(sd);
                var update = de.Query("EXECUTE InsertOrUpdate " + sd.Id + ",Subscriptions,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int AddnewSubscription(Subscription subscription, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(subscription);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Subscriptions," + getPropandVal[0] + "," + getPropandVal[1] + "").First();
                return Convert.ToInt32(add.Id);
            }
            catch
            {
                return -1;
            }
        }
    }
}