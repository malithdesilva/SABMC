using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SabraMedicalCenter.Areas.Admin.Models
{
    public class AdminLogin
    {
        [Required]
        [Display(Name = "User Name")]
        public string username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}