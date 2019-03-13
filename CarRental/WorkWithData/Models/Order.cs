using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WorkWithData.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Id")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 1000 символов")]
        public string ApplicationUserId { get; set; }

        [Required]
        [Display(Name = "Нужен водитель")]
        [Range(typeof(bool), "false", "true")]
        public bool NeedDriver { get; set; }

        [Required]
        [Range(0, 20000, ErrorMessage = "Недопустимое число")]
        [Display(Name = "Часы оренды")]
        public int RentHours { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата начала оренды")]
        public DateTime DateTimeIssued { get; set; }

        [Required]
        public int Price { get; set; }

        [Display(Name = "Заплачено")]
        [Range(typeof(bool), "false", "true")]
        public bool Paid { get; set; }

        [Display(Name = "Согласие на оренду")]
        public bool? AcceptOnOrder { get; set; }

        [Display(Name = "Возвращено")]
        public bool? IsReturned { get; set; }

        [Display(Name = "Штраф")]
        public int? Fine { get; set; }

        [Required]
        [Range(0, 20000, ErrorMessage = "Недопустимое число")]
        [Display(Name = "Машина")]
        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
