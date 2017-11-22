using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SabraMedicalCenter.Areas.Admin.Models
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(int y, string label)
        {
            this.Y = y;
            this.Label = label;
        }

      

        public DataPoint(int y, string label,string legendText)
        {
            this.Y = y;
            this.Label = label;
            this.LegendText = legendText;
        }

        public DataPoint(double x, int y)
        {
            this.X = x;
            this.Y = y;
        }


        public DataPoint(double x, int y, string label)
        {
            this.X = x;
            this.Y = y;
            this.Label = label;
        }

        public DataPoint(double x, int y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public DataPoint(double x, int y, double z, string label)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Label = label;
        }


        //Explicitly setting the name to be used while serializing to JSON. 
        [DataMember(Name = "label")]
        public string Label = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<int> Y = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "x")]
        public Nullable<double> X = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "z")]
        public Nullable<double> Z = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "legendText")]
        public string LegendText = null;
       
    }

    public class DataPoint1
    {
        public DataPoint1(DateTime x, int y)
        {
            this.date = x;
            this.Y = y;
        }

        [DataMember(Name = "y")]
        public Nullable<int> Y = null;

        [DataMember(Name = "x")]
        public Nullable<DateTime> date = null;
    }
}
