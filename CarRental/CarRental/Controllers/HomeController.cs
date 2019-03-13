using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkWithData.Models;
using WorkWithData.AccessToData;

namespace CarRental.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        static CarsListViewModel carsListViewModel;

        public ActionResult CarsList(int? carbrand, int? qualityclas, string sorttype, int? page) 
        {
            if(page == null)
            {
                carsListViewModel = WorkWithCars.GetCarsList(carbrand, qualityclas);
                carsListViewModel = WorkWithCars.GetSortedCarsList(sorttype, carsListViewModel);
                page = 1;
            }
            int pageSize = 3; // count of cars in page
            IList<Car> carsPerPages = carsListViewModel.Cars.Skip(((int)page - 1) * pageSize).Take(pageSize).ToList();
            PageInfo pageInfo = new PageInfo { PageNumber = (int)page, PageSize = pageSize, TotalItems = carsListViewModel.Cars.Count };
            CarsListViewModel carsList = new CarsListViewModel() { PageInfo = pageInfo, Cars = carsPerPages, CarBrand = carsListViewModel.CarBrand, QualityClas = carsListViewModel.QualityClas };
            return View(carsList);
        }
    }
}