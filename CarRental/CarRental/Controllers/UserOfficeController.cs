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
    [Authorize(Roles = "user")]
    public class UserOfficeController : Controller
    {
        public ActionResult ShowOffice()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Order(int Id)
        {
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
            Car car = WorkWithCars.GetCar(Id);
            if (user.IsBlocked)
            {
                return new HttpStatusCodeResult(423);
            }
            else if(car != null)
            {
                //ViewBag.Car = car;
                Order order = new Order() { ApplicationUserId = User.Identity.GetUserId(), Car = car, DateTimeIssued = DateTime.Now, CarId = car.Id, NeedDriver = false, RentHours = 0, Price = 0 };
                return View("Order", order);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Order(Order order)
        {
            if (ModelState.IsValid && order.DateTimeIssued > DateTime.Now && order.RentHours > 0)
            {
                order.Car = WorkWithCars.GetCar(order.CarId);
                if (order.NeedDriver)
                {
                    order.Price = order.RentHours * order.Car.CostForOneHour + order.RentHours * 10;
                }
                else
                {
                    order.Price = order.RentHours * order.Car.CostForOneHour;
                }
                WorkWithOrder.AddOrder(order);
                ViewBag.OrderInfo = "Заказ выполнен успешно";
                return View("BillPage", order);
            }
            return new HttpStatusCodeResult(400);
        }

        [HttpGet]
        public ActionResult PayForOrder(int Id)
        {
            if(WorkWithOrder.PayForOrder(Id))
            {
                ViewBag.OrderInfo = "Заказ оплачен успешно";
                return View();
            }
            ViewBag.OrderInfo = "Заказ не оплачен";
            return View();
        }

        public ActionResult CheckPost()
        {
            return View(WorkWithMessages.GetMessages(User.Identity.GetUserId()));
        }

        public ActionResult ShowOrders()
        {
            List<Order> orders = WorkWithOrder.GetUserOrders(User.Identity.GetUserId());
            return View(orders);
        }

        public ActionResult PartialForUserOffice()
        {
            return PartialView();
        }
    }
}