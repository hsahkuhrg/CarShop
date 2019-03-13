using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkWithData.Models;
using WorkWithData.AccessToData;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace CarRental.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminOfficeController : Controller
    {
        public ActionResult ShowOffice()
        {
            return View();
        }

        public ActionResult CarsList(int? carbrand, int? qualityclas, string sorttype)
        {
            CarsListViewModel carsListViewModel = WorkWithCars.GetCarsList(carbrand, qualityclas);
            carsListViewModel = WorkWithCars.GetSortedCarsList(sorttype, carsListViewModel);
            return View(carsListViewModel);
        }

        [HttpGet]
        public ActionResult CreateCar() // Добавить добавления картинки
        {
            ViewBag.QualityClassAndBrands = WorkWithCars.GetQualityClassAndBrands(null);
            return View();
        }

        [HttpPost]
        public ActionResult CreateCar(Car car, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                    car.PictureUrl = "~/Files/" + fileName;
                    WorkWithCars.CreateCar(car);
                    return RedirectToAction("CarsList");
                }
                ModelState.AddModelError("", "Файл отсутствует");
            }
            ViewBag.QualityClassAndBrands = WorkWithCars.GetQualityClassAndBrands(null);
            return View(car);
        }

        [HttpGet]
        public ActionResult EditCar(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }
            Car car = WorkWithCars.GetCar(Id);
            if(car != null)
            {
                ViewBag.QualityClassAndBrands = WorkWithCars.GetQualityClassAndBrands(car);
                return View(car);
            }
            return RedirectToAction("CarsList");
        }

        [HttpPost]
        public ActionResult EditCar(Car car, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                    car.PictureUrl = "~/Files/" + fileName;
                    WorkWithCars.EditCar(car);
                    return RedirectToAction("CarsList");
                }
                WorkWithCars.EditCar(car);
                return RedirectToAction("CarsList");
            }
            ViewBag.QualityClassAndBrands = WorkWithCars.GetQualityClassAndBrands(car);
            return View(car);
        }

        [HttpGet]
        public ActionResult DeleteCar(int Id)
        {
            Car car = WorkWithCars.GetCar(Id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        [HttpPost, ActionName("DeleteCar")]
        public ActionResult DeleteConfirmed(int Id)
        {
            bool result = WorkWithCars.DeleteCar(Id);
            if (result == false)
            {
                return HttpNotFound();
            }
            return RedirectToAction("CarsList");
        }

        [HttpGet]
        public ActionResult UsersList()
        {
            return View(WorkWithUsers.GetUsers());
        }

        [HttpGet]
        public ActionResult BlockUser(string Id)
        {
            WorkWithUsers.BlockUser(Id);
            return RedirectToAction("UsersList");
        }

        [HttpGet]
        public ActionResult AnBlockUser(string Id)
        {
            WorkWithUsers.AnBlockUser(Id);
            return RedirectToAction("UsersList");
        }

        public ActionResult PartialForAdminOffice()
        {
            return PartialView();
        }
    }
}