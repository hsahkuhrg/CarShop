using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WorkWithData.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Имя учетной записи")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Длина имени учетной записи должна быть от 8 до 20 символов")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Имя")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Длина имени должна быть от 1 до 10 символов")]
        public string ApplicationUserRealName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Длина фамилии должна быть от 1 до 10 символов")]
        public string Surname { get; set; }

        [Required]
        [Range(18, 100, ErrorMessage = "Недопустимый возраст")]
        [Display(Name = "Возраст")]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
