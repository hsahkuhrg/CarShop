using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkWithData.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WorkWithData.AccessToData;

namespace CarRental.Controllers
{
    [Authorize(Roles = "manager")]
    public class ManagerOfficeController : Controller
    {
        public ActionResult ShowOffice()
        {
            return View();
        }

        public ActionResult UsersList()
        {
            return View(WorkWithUsers.GetUsers());
        }

        public ActionResult CleanOldOrders()
        {
            WorkWithOrder.CleanOldOrders();
            return View("ShowOffice");
        }

        public ActionResult CarList(int? carbrand, int? qualityclas, string sorttype)
        {
            CarsListViewModel carsListViewModel = WorkWithCars.GetCarsList(carbrand, qualityclas);
            carsListViewModel = WorkWithCars.GetSortedCarsList(sorttype, carsListViewModel);
            return View(carsListViewModel);
        }

        [HttpGet]
        public ActionResult UpdateOrder(int Id)
        {
            Order order = WorkWithOrder.GetOrders(Id);
            List<AcceptString> acceptStrings = new List<AcceptString>() { new AcceptString() { Text = "Одобрено", Value = true }, new AcceptString() { Text = "Неодобрено", Value = false }, new AcceptString() { Text = "В обработке", Value = null } };
            ViewBag.AcceptString = new SelectList(acceptStrings, "Value", "Text");
            List<AcceptString> returnedStrings = new List<AcceptString>() { new AcceptString() { Text = "Возвращено", Value = true }, new AcceptString() { Text = "Невозвращено", Value = false }, new AcceptString() { Text = "Невыдано", Value = null } };
            ViewBag.ReturnedString = new SelectList(returnedStrings, "Value", "Text");
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateOrder(Order order)
        {
            if(order.Fine != null && order.Fine < 0)
            {
                ModelState.AddModelError("", "Штраф должен быть больше 0");
                List<AcceptString> acceptStrings = new List<AcceptString>() { new AcceptString() { Text = "Одобрено", Value = true }, new AcceptString() { Text = "Неодобрено", Value = false }, new AcceptString() { Text = "В обработке", Value = null } };
                ViewBag.AcceptString = new SelectList(acceptStrings, "Value", "Text");
                List<AcceptString> returnedStrings = new List<AcceptString>() { new AcceptString() { Text = "Возвращено", Value = true }, new AcceptString() { Text = "Невозвращено", Value = false }, new AcceptString() { Text = "Невыдано", Value = null } };
                ViewBag.ReturnedString = new SelectList(returnedStrings, "Value", "Text");
                return View(order);
            }
            WorkWithOrder.UpdateOrder(order);
            return View("ShowOffice");
        }

        [HttpGet]
        public ActionResult UserInfo(string Id)
        {
            List<Order> orders = WorkWithOrder.GetUserOrders(Id);
            ViewBag.ApplicationUser = WorkWithUsers.GetApplicationUser(Id);
            return View(orders);
        }

        [HttpGet]
        public ActionResult SendLeter(string Id)
        {
            ViewBag.WhoGetUserId = Id;
            return View();
        }

        [HttpPost]
        public ActionResult SendLeter(string WhoGetUserId, string Text) 
        {
            if(Text == "" || Text.Length > 1000)
            {
                ModelState.AddModelError("", "Длина строки должна быть до 1000 символов");
                ViewBag.WhoGetUserId = WhoGetUserId;
                return View();
            }
            Message message = new Message() { WhoSendUserId = User.Identity.GetUserId(), WhoGetUserId = WhoGetUserId, Date = DateTime.Now, Text = Text};
            WorkWithMessages.SendLeter(message);
            return View("ShowOffice");
        }

        [HttpGet]
        public ActionResult CarOrdersInfo(int Id)
        {
            List<Order> orders = WorkWithOrder.GetCarOrdersInfo(Id);
            return View(orders);
        }

        public ActionResult OrderList(string StringForFilter, int? OrderId = null)
        {
            List<Order> orders;
            if (OrderId != null)
            {
                orders = new List<Order>();
                Order order = WorkWithOrder.GetOrderById(OrderId);
                if (order != null)
                {
                    orders.Add(order);
                    return View(orders);
                }
                return HttpNotFound();
            }
            orders = WorkWithOrder.GetOrderList(StringForFilter);
            return View(orders);
        }

        public new ActionResult PartialView()
        {
            return PartialView();
        }
    }
}