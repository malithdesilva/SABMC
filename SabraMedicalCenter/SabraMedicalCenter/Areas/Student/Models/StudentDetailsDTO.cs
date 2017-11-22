using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SabraMedicalCenter.Areas.Student.Models
{
    public class StudentDetailsDTO
    {
        [Key]
        
        [Display(Name = "Student Name")]
        [Required(ErrorMessage = "Please enter your Name")]
        public string studentname { get; set; }

        [Display(Name = "Student Reg.No")]
        [Required(ErrorMessage = "Please enter your registration number")]
        public string studentregno { get; set; }

        [Display(Name = "Age")]
        //[Required(ErrorMessage = "Please enter your age")]
        public int? age { get; set; }

        
        [Display(Name = "NIC No")]
        [Required(ErrorMessage = "Please enter your nic number")]
        public string nicno { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please enter your gender")]
        public string gender { get; set; }

        [Display(Name = "Date of birth")]
        [Required(ErrorMessage = "Please enter your date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? dataofbirth { get; set; }

        [Display(Name = "Faculty")]
        [Required(ErrorMessage = "Please enter your faculty")]
        public string faculty { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Please enter your year")]
        public int? year { get; set; }

        [Display(Name = "Rational")]
        [Required(ErrorMessage = "Please enter your rational")]
        public string rationanal { get; set; }

        [Display(Name = "Family Members")]
        [Required(ErrorMessage = "Please enter your family members")]
        public int? familymembers { get; set; }

        [Display(Name = "Father's Name")]
        [Required(ErrorMessage = "Please enter your Father's name")]
        public string fathernmae { get; set; }

        [Display(Name = "Mother's Name")]
        [Required(ErrorMessage = "Please enter your Mother's Name")]
        public string mothername { get; set; }
    }
}