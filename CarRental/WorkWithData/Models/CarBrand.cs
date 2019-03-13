using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WorkWithData.Models
{
    public class CarBrand
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 10 символов")]
        public string Name { get; set; }
        public ICollection<Car> Cars { get; set; }
        public CarBrand()
        {
            Cars = new List<Car>();
        }
    }
}
