using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SabraMedicalCenter.Areas.Student.Models
{
    public class StudentDTO
    {
        [Key]
        [Required(ErrorMessage = "please enter the user name")]
        [Display(Name = "User Name")]
        public string username { get; set; }

        [Required(ErrorMessage = "please enter the password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}