using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.DAL
{
    public class CarDetailDAL
    {
        public List<CarDetail> GetActiveCarDetailList(DatabaseEntities de)
        {
            try
            {
                return de.CarDetails.Where(x => x.IsActive == 1).ToList();
            }
            catch (Exception ex) 
            {
                throw;
            }
            
        }

        public CarDetail GetActiveCarDetailById(int id, DatabaseEntities de)
        {
            return de.CarDetails.Where(x => x.Id == id).FirstOrDefault(x => x.IsActive == 1);
        }

        public bool AddCarDetail(CarDetail carDetail, DatabaseEntities de)
        {
            try
            {
                de.CarDetails.Add(carDetail);
                de.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateCarDetail(CarDetail carDetail, DatabaseEntities de)
        {
            try
            {
                de.Entry(carDetail).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //public bool DeleteCarDetail(int id, DatabaseEntities de)
        //{
        //    try
        //    {
        //        de.CarDetails.Remove(de.CarDetails.Where(x => x.Id == id).FirstOrDefault());
        //        de.SaveChanges();

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}