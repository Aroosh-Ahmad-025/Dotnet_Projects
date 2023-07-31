using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.Models
{
    public class CarDetailDTO
    {
        public string LotYear { get; set; }
        public string LotMake { get; set; }
        public string LotModel { get; set; }
        public string LotRunCondition { get; set; }
        public string DamageTypeDescription { get; set; }
        public string CopartFacilityName { get; set; }
        public string SaleTitleState { get; set; }
        public string SaleTitleType { get; set; }
        public string DamageType { get; set; }
        public string LotColor { get; set; }
        public string HasKey { get; set; }
        public string OdometerReading { get; set; }
        public string SalePrice { get; set; }
        public string RepairCost { get; set; }


        // Additional Add

        //public String Name { get; set; }
        //public double GetSalePrice { get; set; }


    }
}