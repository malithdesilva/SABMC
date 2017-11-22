using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SabraMedicalCenter.Areas.Admin.Models
{
    public class MedicalFormDTO
    {
        public int studentpk { get; set; }
        [Required(ErrorMessage = "please search and fill student details first")]
        public string Searchword { get; set; }

        
      
        [Display(Name = "Name")]
        public string Student_Name { get; set; }

       
      
        [Display(Name = "RegNo")]
        public string Student_RegNo { get; set; }
       
      
        [Display(Name = "Age")]
        public Nullable<int> Student_Age { get; set; }


        
        
        [Display(Name = "NicNo")]
        public string Student_NicNo { get; set; }
        [Display(Name = "Gender")]
       
        public string Student_Gender { get; set; }
     
  
        [Display(Name = "Faculty")]
        public string Student_Faculty { get; set; }
       
     
        [Display(Name = "Year")]
        public Nullable<int> Student_Year { get; set; }

       
        [Required(ErrorMessage = "please Enter the Date")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime MedicalDate { get; set; }

       
        [Required(ErrorMessage = "Please Enter Disease")]
        [Display(Name = "Disease")]
        [DataType(DataType.Text)]
        public string Diesese { get; set; }

      
        [Required(ErrorMessage = "please fill discription")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }



        [Display(Name = "Drug Code")]

        public string Drugcode { get; set; }

        [Display(Name = "Drug Name")]
        
        public string DrugName { get; set; }

        
        public string Unit { get; set; }

        
        [Display(Name = "Drug Qty")]
        public int? DrugQty { get; set; }
    }

    public class pdf
    {
        public string Drugcode { get; set; }
        public string DrugName { get; set; }
        public string Unit { get; set; }
        public int? DrugQty { get; set; }
    }
}