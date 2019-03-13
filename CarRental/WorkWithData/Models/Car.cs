using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WorkWithData.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 10 символов")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Стоимость")]
        [Range(0, 2000, ErrorMessage = "Недопустимое число")]
        public int CostForOneHour { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required]
        [Range(0, 2000, ErrorMessage = "Недопустимое число")]
        public int CountOfCars { get; set; }

        [Required]
        [Display(Name = "Марка")]
        public int CarBrandId { get; set; }

        public CarBrand CarBrand { get; set; }

        [Required]
        [Display(Name = "Класс")]
        public int QualityClassId { get; set; }

        public QualityClass QualityClass { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Car()
        {
            Orders = new List<Order>();
        }
    }
}
