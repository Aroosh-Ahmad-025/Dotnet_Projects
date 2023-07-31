using CarSystem.DAL;
using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.BL
{
    public class CarDetailBL
    {
        public List<CarDetail> GetActiveCarDetailList(DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveCarDetailList(de);
        }


        public CarDetail GetActiveCarDetailById(int id, DatabaseEntities de)
        {
            return new CarDetailDAL().GetActiveCarDetailById(id, de);
        }

        public bool AddCarDetail(CarDetail carDetail, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(carDetail.LotYear) || String.IsNullOrEmpty(carDetail.DamageType) || String.IsNullOrEmpty(carDetail.DamageTypeDescription) 
                || string.IsNullOrEmpty(carDetail.LotModel) || string.IsNullOrEmpty(carDetail.LotRunCondition) || string.IsNullOrEmpty(carDetail.SalePrice)
                || string.IsNullOrEmpty(carDetail.OdometerReading) )
            {
                return false;
            }
            else
            {
                return new CarDetailDAL().AddCarDetail(carDetail, de);
            }
        }

        public bool UpdateCarDetail(CarDetail carDetail, DatabaseEntities de)
        {
            if (String.IsNullOrEmpty(carDetail.LotYear) || String.IsNullOrEmpty(carDetail.DamageType) || String.IsNullOrEmpty(carDetail.DamageTypeDescription)
                || string.IsNullOrEmpty(carDetail.LotModel) || string.IsNullOrEmpty(carDetail.LotRunCondition) || string.IsNullOrEmpty(carDetail.SalePrice)
                || string.IsNullOrEmpty(carDetail.OdometerReading))
            {
                return false;
            }
            else
            {
                return new CarDetailDAL().UpdateCarDetail(carDetail, de);
            }
        }

        //public bool DeleteCarDetail(int id, DatabaseEntities de)
        //{
        //    return new CarDetailDAL().DeleteCarDetail(id, de);
        //}
    }
}