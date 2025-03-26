using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BOL_Business_Objects_Layer_
{
    public class Magzine
    {
        public string MagzineId { get; set; }
        public string MagzineCode { get; set; }
        public string MagzineName { get; set; }
        public string MagzineType { get; set; }
        public string PublishName { get; set; }
        public string Frequency { get; set; }
        public string ISBN { get; set; }
        public string OtherDetails { get; set; }       
        public string MagzineStatus { get; set; }
        public string Cdate { get; set; }
        public string GST { get; set; }
        public List<Price> Prices { get; set; }
    }
    public class Price
    {
        public string PriceId { get; set; }
        public string MagzineId { get; set; }
        public string Tenure { get; set; }
        public string Rate { get; set; }
        public string Cdate { get; set; }

        public string gst { get; set; }

        public string stock { get; set; }
    }
}
