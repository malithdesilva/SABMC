using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SabraMedicalCenter.Areas.Student.Models
{
    public class StudentHistoryDTO
    {
        [Display(Name = "Date")]
        public DateTime? date { get; set; }

        [Display(Name = "Disease Name")]
        public string disease { get; set; }

        [Display(Name = "Descrition")]
        public string descrition { get; set; }

        [Display(Name = "Medical Issue")]
        public string medical { get; set; }

        [Display(Name = "Drug Name")]
        public List<string> drugname { get; set; }

        [Display(Name = "Drug Qty")]
        public List<int?> drugqty { get; set; }
    }
}