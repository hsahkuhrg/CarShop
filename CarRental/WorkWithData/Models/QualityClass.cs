using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WorkWithData.Models
{
    public class QualityClass
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Длина строки должна быть 1 символ")]
        public string Name { get; set; }
        public ICollection<Car> Cars { get; set; }
        public QualityClass()
        {
            Cars = new List<Car>();
        }
    }
}
