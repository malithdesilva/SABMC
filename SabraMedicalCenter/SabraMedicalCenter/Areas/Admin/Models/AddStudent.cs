using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SabraMedicalCenter.Areas.Admin.Models
{
    public class AddStudent
    {
        [Key]
        [Required]
        [Display(Name = "Student Name")]
        public string Student_Name { get; set; }

        [Required]
        [Display(Name = "Student Reg No")]
        public string Student_RegNo { get; set; }

        [Required]
        [Display(Name = "Student Age")]
        [Range(18, 28,ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public Nullable<int> Student_Age { get; set; }

        [Required]
        [Display(Name = "Student Nic.No")]
        [StringLength(10)]
        public string Student_NicNo { get; set; }

        [Required]
        [Display(Name = "Student Gender")]
        public Gender Student_Gender { get; set; }


        [Display(Name = "Date of birth")]
        [Required(ErrorMessage = "please Enter the birth date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Student_DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Student Faculty")]
        public Faculties Student_Faculty { get; set; }

        [Required]
        [Display(Name = "Student Year")]
        [Range(1, 4, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public Nullable<int> Student_Year { get; set; }

        [Required]
        [Display(Name = "Race")]
        public string Student_Rational { get; set; }

        [Required]
        [Display(Name = "Family Members")]
        [Range(0, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public Nullable<int> Student_FamilyMembers { get; set; }

        [Required]
        [Display(Name = "Fathers Name")]
        public string Student_FatherName { get; set; }

        [Required]
        [Display(Name = "Mother Name")]
        public string Student_MotherName { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum Faculties
    {
        Applied,
        Managment,
        Social,
        Agri,
        Geo
    }
}