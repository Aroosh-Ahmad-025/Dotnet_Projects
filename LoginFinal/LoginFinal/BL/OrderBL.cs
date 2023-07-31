using LoginFinal.DAL;
using LoginFinal.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginFinal.BL
{
    public class OrderBL
    {

        public List<Order> GetOrdersByUserId(int id, SqlConnection de)
        {
            return new OrderDAL().GetOrdersById(id, de);
        }
        public List<Order> GetAllOrders(SqlConnection de)
        {
            return new OrderDAL().GetAllOrders(de);
        }
        public int AddOrder(Order newOrder, SqlConnection de)
        {
            return new OrderDAL().AddOrder(newOrder, de);
        }

        public Order GetOrderById(int id, SqlConnection de)
        {
            return new OrderDAL().GetOrderById(id, de);
        }
        public bool UpdateOrder(Order order, SqlConnection de)
        {
            return new OrderDAL().UpdateOrder(order, de);
        }



        public List<Subscription> GetSubscriptionsByUserId(int id, SqlConnection de)
        {
            return new OrderDAL().GetSubscriptionsById(id, de);
        }

        public List<Subscription> GetSubscriptionsAgainstSpecificButler(int cId, int bId, SqlConnection de)
        {
            return new OrderDAL().GetSubscriptionsAgainstSpecificButler(cId, bId, de);
        }

        public List<Subscription> GetAllSubscriptions(SqlConnection de)
        {
            return new OrderDAL().GetAllSubscriptions(de);
        }
        public int AddSubscription(Subscription newSubscription, SqlConnection de)
        {
            return new OrderDAL().AddnewSubscription(newSubscription, de);
        }

        public Subscription GetSubscriptionById(int id, SqlConnection de)
        {
            return new OrderDAL().GetSubscriptionById(id, de);
        }
        public bool UpdateSubscription(Subscription subscription, SqlConnection de)
        {
            return new OrderDAL().UpdateSubscription(subscription, de);
        }
    }
}