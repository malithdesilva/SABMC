using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SabraMedicalCenter.Areas.Admin.Models
{
    public class DrugStockDTO
    {
        [Required]
        [Display(Name = "Drug Code")]
        public string drugcode { get; set; }

        [Required]
        [Display(Name = "Drug Name")]
        public string drugname { get; set; }

        [Required]
        [Display(Name = "Receive Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? receivedate { get; set; }

        [Required]
        [Display(Name = "Expire Date")]
        [DataType(DataType.Date)]
       
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? exipiredate { get; set; }


        [Required]
        [Display(Name = "Unit")]
        public string unit { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "Value must not be nagetive and should between 1 to 100000")]
        [Display(Name = "Quantity")]
        public int? quantity { get; set; }


        [Browsable(true)]
        [Display(Name = "Balance")]
        public int? balance { get; set; }
    }
}