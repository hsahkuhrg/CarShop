using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkWithData.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace WorkWithData.AccessToData
{
    public class WorkWithOrder
    {
        public static void AddOrder(Order order)
        {
            Context context = new Context();
            order.Car = null;
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public static bool PayForOrder(int Id)
        {
            Context context = new Context();
            Order order = context.Orders.Find(Id);
            if(order != null)
            {
                order.Paid = true;
                context.Entry(order).State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public static List<Order> GetUserOrders(string Id)
        {
            Context context = new Context();
            List<Order> orders = context.Orders.Include(m => m.Car).Where(m => m.ApplicationUserId == Id).ToList();
            return orders;
        }

        public static List<Order> GetCarOrdersInfo(int Id)
        {
            Context context = new Context();
            List<Order> orders = context.Orders.Where(m => m.CarId == Id).ToList();
            return orders;
        }

        public static void UpdateOrder(Order order)
        {
            Context context = new Context();
            order.Car = WorkWithCars.GetCar(order.CarId);
            if (order.NeedDriver)
            {
                order.Price = order.RentHours * order.Car.CostForOneHour + order.RentHours * 10;
            }
            else
            {
                order.Price = order.RentHours * order.Car.CostForOneHour;
            }
            order.Car = null;
            context.Entry(order).State = EntityState.Modified;
            context.SaveChanges();
        }

        public static Order GetOrders(int Id)
        {
            Context context = new Context();
            Order order = context.Orders.Include(m => m.Car).FirstOrDefault(m => m.Id == Id);
            return order;
        }

        public static List<Order> GetOrderList(string StringForFilter)
        {
            Context context = new Context();
            List<Order> orders;
            switch (StringForFilter)
            {
                case "NewOrders":
                    orders = context.Orders.Include(m => m.Car).Where(m => m.AcceptOnOrder == null).OrderBy(m => m.DateTimeIssued).ToList();
                    break;
                case "Permitted":
                    orders = context.Orders.Include(m => m.Car).Where(m => m.AcceptOnOrder == true).OrderBy(m => m.DateTimeIssued).ToList();
                    break;
                case "Prohibited":
                    orders = context.Orders.Include(m => m.Car).Where(m => m.AcceptOnOrder == false).OrderBy(m => m.DateTimeIssued).ToList();
                    break;
                case "PermittedAndPaied":
                    orders = context.Orders.Include(m => m.Car).Where(m => m.AcceptOnOrder == true & m.Paid == true).OrderBy(m => m.DateTimeIssued).ToList();
                    break;
                case "IsNotReturned":
                    orders = context.Orders.Include(m => m.Car).Where(m => m.IsReturned == false).OrderBy(m => m.DateTimeIssued).ToList();
                    break;
                case "WithFine":
                    orders = context.Orders.Include(m => m.Car).Where(m => m.Fine != null && m.Fine > 0).OrderBy(m => m.DateTimeIssued).ToList();
                    break;
                default:
                    orders = context.Orders.Include(m => m.Car).OrderBy(m => m.DateTimeIssued).ToList();
                    break;
            }
            return orders;
        }

        public static void CleanOldOrders()
        {
            Context context = new Context();
            List<Order> orders = context.Orders.Where(m => m.Fine == null & m.IsReturned == true).ToList();
            context.Orders.RemoveRange(orders);
            context.SaveChanges();
        }

        public static Order GetOrderById(int? OrderId)
        {
            Context context = new Context();
            Order order = context.Orders.Include(m => m.Car).FirstOrDefault(m => m.Id == OrderId);
            return order;
        }
    }
}
