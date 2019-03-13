using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WorkWithData.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "WhoSendUserId")]
        public string WhoSendUserId { get; set; }
        [Required]
        [Display(Name = "WhoGetUserId")]
        public string WhoGetUserId { get; set; }
        [Required]
        [Display(Name = "Text")]
        [StringLength(1000, ErrorMessage = "Длина строки должна до 1000 символов")]
        public string Text { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
    }
}
