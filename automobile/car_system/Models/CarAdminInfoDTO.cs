using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Builder;
using Microsoft.Owin.BuilderProperties;

namespace CarSystem.Models
{
    public class Address
    {
        public Contact contact { get; set; }
    }
    public class AdminInfo
    {
        public string sellerCompanyCode { get; set; }
        public string officeCode { get; set; }
    }
    public class Contact
    {
        public string postalCode { get; set; }
    }
    public class LossInfo
    {
        //public object dateOfLoss { get; set; }
        //public object lossType { get; set; }
        public string primaryPointOfImpact { get; set; }
        //public object damageSeverity { get; set; }
    }
    public class OdometerInfo
    {
        public string odometerReading { get; set; }
        public string odometerBrand { get; set; }
    }
    public class RootClass
    {
        public string transactionId { get; set; }
        public AdminInfo adminInfo { get; set; }
        public VehicleLocationSite vehicleLocationSite { get; set; }
        public string claimNumber { get; set; }
        public LossInfo lossInfo { get; set; }
        public VehicleInformation vehicleInformation { get; set; }
        public Valuation valuation { get; set; }
        public VehicleCondition vehicleCondition { get; set; }
    }
    public class Valuation
    {
        public int acv { get; set; }
        public int repairCost { get; set; }
    }
    public class VehicleCondition
    {
        public string drivabilityRating { get; set; }
        public string titleCategory { get; set; }
    }
    public class VehicleInformation
    {
        public string vin { get; set; }
        public string year { get; set; }
        public string makeCode { get; set; }
        //public string makeDescription { get; set; }
        public string model { get; set; }
        public string vehicleType { get; set; }

        //public string bodyStyle { get; set; }
        public OdometerInfo odometerInfo { get; set; }
        public string hasKeys { get; set; }
        public string powerTrain { get; set; }
        public int engine { get; set; }
    }
    public class VehicleLocationSite
    {
        public Address address { get; set; }
    }
}
















//namespace CarSystem.Models
//{
//    public class CarAdminInfoDTO
//    {
//        public string sellerCompanyCode { get; set; }
//        public string officeCode { get; set; }
//    }


//    public class vehicleLocationSite
//    {
//        public string postalCode { get; set; }
//    }

//    public class LossInfo
//    {
//        public object dateOfLoss { get; set; }
//        public object lossType { get; set; }
//        public string primaryPointOfImpact { get; set; }
//        public object damageSeverity { get; set; }
//    }

//    public class OdometerInfo
//    {
//        public string odometerReading { get; set; }
//        public string odometerBrand { get; set; }
//    }

//    public class Valuation
//    {
//        public int acv { get; set; }
//        public int repairCost { get; set; }
//    }

//    public class VehicleCondition
//    {
//        public string drivabilityRating { get; set; }
//        public string titleCategory { get; set; }
//    }

//    public class VehicleInformation
//    {
//        public string vin { get; set; }
//        public string year { get; set; }
//        public string makeCode { get; set; }
//        public string makeDescription { get; set; }
//        public string model { get; set; }
//        public string vehicleType { get; set; }
//        public string bodyStyle { get; set; }
//        public OdometerInfo odometerInfo { get; set; }
//        public string hasKeys { get; set; }
//        public string powerTrain { get; set; }
//        public string engine { get; set; }
//    }

//    public class VehicleAddress
//    {
//        public Address address { get; set; }
//    }

//    public class RootClass
//    {
//        public string transactionId { get; set; }
//        public CarAdminInfoDTO adminInfo { get; set; }
//        public vehicleLocationSite vehicleLocationSite { get; set; }
//        public string claimNumber { get; set; }
//        public LossInfo lossInfo { get; set; }
//        public VehicleInformation vehicleInformation { get; set; }
//        public Valuation valuation { get; set; }
//        public VehicleCondition vehicleCondition { get; set; }
//        public VehicleAddress vehicleAddress { get; set; }


//    }


//}