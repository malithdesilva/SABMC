using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SabraMedicalCenter.Areas.Admin.Models
{
    public class StudentHistoryDT
    {
        [Display(Name = "Student Name")]
        public string studentname { get; set; }

        [Display(Name = "Reg No")]
        public string studentregno { get; set; }

        [Display(Name = "Nic No")]
        public string nicno { get; set; }

        [Display(Name = "Date")]
        public DateTime? date { get; set; }

        [Display(Name = "Disease Name")]
        public string disease { get; set; }

        [Display(Name = "Descrition")]
        public string descrition { get; set; }

        [Display(Name = "Faculty")]
        public string faculty { get; set; }

        [Display(Name = "Rational")]
       
        public string rationanal { get; set; }

        [Display(Name = "Family Members")]
        
        public int? familymembers { get; set; }

        [Display(Name = "Father's Name")]
       
        public string fathernmae { get; set; }

        [Display(Name = "Mother's Name")]
       
        public string mothername { get; set; }


        [Display(Name = "Year")]
        public int? year { get; set; }

        [Display(Name = "Medical Issue")]
        public string medical { get; set; }

        [Display(Name = "Drug Name")]
        public List<string> drugname { get; set; }

        [Display(Name = "Drug Qty")]
        public List<int?> drugqty { get; set; }

        [Display(Name = "Drug code")]
        public List<int?> drugcode { get; set; }
    }
}