using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SabraMedicalCenter.Areas.Admin.Models
{
    public class DrugSessionDTO
    {
        public string Drugcode { get; set; }
        public string DrugName { get; set; }
        public int DrugQty { get; set; }
    }
}