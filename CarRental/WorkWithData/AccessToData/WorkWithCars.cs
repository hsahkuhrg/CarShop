using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using WorkWithData.Models;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace WorkWithData.AccessToData
{
    public class WorkWithCars
    {
        public static CarsListViewModel GetCarsList(int? carbrand, int? qualityclas)
        {
            Context context = new Context();
            IQueryable<Car> cars = context.Cars.Include(p => p.CarBrand).Include(p => p.QualityClass);
            if(carbrand != null && carbrand != 0)
            {
                cars = cars.Where(p => p.CarBrandId == carbrand);
            }

            if(qualityclas != null && qualityclas != 0)
            {
                cars = cars.Where(p => p.QualityClassId == qualityclas);
            }

            List<CarBrand> carBrands = context.CarBrands.ToList();
            List<QualityClass> qualityClasses = context.QualityClasses.ToList();

            carBrands.Insert(0, new CarBrand() { Name = "Все", Id = 0 });
            qualityClasses.Insert(0, new QualityClass() { Name = "Все", Id = 0 });

            CarsListViewModel carsListViewModel = new CarsListViewModel()
            {
                Cars = cars.ToList(),
                CarBrand = new SelectList(carBrands, "Id", "Name"),
                QualityClas = new SelectList(qualityClasses, "Id", "Name")
            };
            return carsListViewModel;
        }

        public static CarsListViewModel GetSortedCarsList(string sorttype, CarsListViewModel carsListViewModel)
        {
            switch (sorttype)
            {
                case "SortByPriceFromMin":
                    var list1 = from item in carsListViewModel.Cars
                               orderby item.CostForOneHour
                               select item;
                    carsListViewModel.Cars = list1.ToList();
                    break;
                case "SortByPriceFromMax":
                    var list2 = from item in carsListViewModel.Cars
                               orderby item.CostForOneHour descending
                               select item;
                    carsListViewModel.Cars = list2.ToList();
                    break;
                case "SortByNameFromA":
                    var list3 = from item in carsListViewModel.Cars
                                orderby item.Name
                                select item;
                    carsListViewModel.Cars = list3.ToList();
                    break;
                case "SortByNameFromZ":
                    var list4 = from item in carsListViewModel.Cars
                                orderby item.Name descending
                                select item;
                    carsListViewModel.Cars = list4.ToList();
                    break;
            }
            return carsListViewModel;
        }

        public static SelectList[] GetQualityClassAndBrands(Car car)
        {
            Context context = new Context();
            List<CarBrand> carBrands = context.CarBrands.ToList();
            List<QualityClass> qualityClasses = context.QualityClasses.ToList();
            SelectList[] selectListItems = new SelectList[2];
            if (car == null)
            {
                selectListItems[0] = new SelectList(carBrands, "Id", "Name");
                selectListItems[1] = new SelectList(qualityClasses, "Id", "Name");
            }
            else
            {
                selectListItems[0] = new SelectList(carBrands, "Id", "Name", car.CarBrandId);
                selectListItems[1] = new SelectList(qualityClasses, "Id", "Name", car.QualityClassId);
            }
            return selectListItems;
        }

        public static void CreateCar(Car car)
        {
            Context context = new Context();
            context.Cars.Add(car);
            context.SaveChanges();
        }

        public static Car GetCar(int? Id)
        {
            Context context = new Context();
            Car car = context.Cars.Include(p => p.CarBrand).Include(p => p.QualityClass).FirstOrDefault(p => p.Id == Id);
            return car;
        }

        public static void EditCar(Car car)
        {
            Context context = new Context();
            context.Entry(car).State = EntityState.Modified;
            context.SaveChanges();
        }

        public static bool DeleteCar(int Id)
        {
            Context context = new Context();
            Car car = context.Cars.Find(Id);
            if (car == null)
            {
                return false;
            }
            context.Cars.Remove(car);
            context.SaveChanges();
            return true;
        }
    }
}
