using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WorkWithData.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Range(18, 100, ErrorMessage = "Недопустимый возраст")]
        [Display(Name = "Возраст")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Длина фамилии должна быть от 1 до 10 символов")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Имя")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Длина имени должна быть от 1 до 10 символов")]
        public string ApplicationUserRealName { get; set; }

        [Range(typeof(bool),"false", "true", ErrorMessage = "Укажите булевое значение")]
        public bool IsBlocked { get; set; }

        public ApplicationUser()
        {
        }
    }
}
