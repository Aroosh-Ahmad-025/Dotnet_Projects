using CarSystem.BL;
using CarSystem.Helping_Classes;
using CarSystem.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarSystem.Controllers
{
    public class AdminController : Controller
    {   

        DatabaseEntities de = new DatabaseEntities();
        GeneralPurpose gp = new GeneralPurpose();

        public bool CheckLogin()
        {
            if (gp.ValidateLoggedinUser() == null)
            {
                return false;
            }
            else
            {
                if (gp.ValidateLoggedinUser().Role == 2)
                {
                    return false;
                }

                return true;
            }

        }

        // UserCount = Index
        // Index =List

            #region User Starting

                    public ActionResult Index(string msg = "", string color = "black")
                    {

                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }


                        ViewBag.Message = msg;
                        ViewBag.Color = color;

                        ViewBag.UserCount = new UserBL().GetActiveUsersList(de).Where(x => x.Role == 2).Count();

                        return View();
                    }

                    // GET: Admin

                    public ActionResult List(string msg = "", string color = "black")
                    {
                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }

                        ViewBag.Message = msg;
                        ViewBag.Color = color;

                        List<User> uList = new UserBL().GetActiveUsersList(de);
                        ViewBag.List = uList;
                        return View();
                    }

                    public ActionResult AddUser(string msg = "", string color = "black")
                    {
                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }

                        ViewBag.Message = msg;
                        ViewBag.Color = color;
                        //User u = new UserBL().AddUser(, de);
                        return View();
                    }

                    [HttpPost]
                    public ActionResult PostAddUser(User user)
                    {
                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }

                        User checkMail = new UserBL().GetActiveUsersList(de).Where(x => x.Email.ToLower() == user.Email.ToLower() && x.Id != user.Id).FirstOrDefault();
                        if (checkMail != null)
                        {
                            return RedirectToAction("AddUser", "Admin", new { msg = "Email already exists.", color = "red" });
                        }

                        //StringCipher.Encrypt(user.Password);
                        //user.CreatedAt = gp.DateTimeNow();
                        //user.IsActive = 1;
                        //user.Role = 2;


                        User u = new User()
                        {
                            Name = user.Name.Trim(),
                            Contact = user.Contact,
                            Email = user.Email.Trim(),
                            Password = StringCipher.Encrypt(user.Password),
                            //Gender = user.Gender,
                            Gender=1,   // change (Haider Sir) 2/18/2022
                            Role = 2,
                            IsActive = 1,
                            CreatedAt = gp.DateTimeNow()
                        };


                        bool check = new UserBL().AddUser(u, de);
                        if (check == true)
                        {
                            return RedirectToAction("AddUser", "Admin", new { msg = "User has been added.", color = "green" });
                        }
                        return RedirectToAction("AddUser", "Admin", new { msg = "Something wents wrong.", color = "green" });
                    }

                    public ActionResult EditUser(string id, string msg = "", string color = "black")
                    {
                        //if (ValidateLoggedinUser() == null)
                        //{
                        //    return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        //}
                        //else
                        //{
                        //    if (ValidateLoggedinUser().Role == 2)
                        //    {
                        //        return RedirectToAction("Login", "Auth");
                        //    }

                        //}

                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }

                        ViewBag.Message = msg;
                        ViewBag.Color = color;

                        User getuser = new UserBL().GetActiveUserById(StringCipher.DecryptId(id), de);

                        ViewBag.User = getuser;

                        return View();
                    }
                    [HttpPost]
                    public ActionResult PostEditUser(User user)
                    {
                        //if (ValidateLoggedinUser() == null)
                        //{
                        //    return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        //}
                        //else
                        //{
                        //    if (ValidateLoggedinUser().Role == 2)
                        //    {
                        //        return RedirectToAction("Login", "Auth");
                        //    }

                        //}

                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }

                        User checkMail = new UserBL().GetActiveUsersList(de).Where(x => x.Email.ToLower() == user.Email.ToLower() && x.Id != user.Id).FirstOrDefault();
                        if (checkMail != null)
                        {
                            return RedirectToAction("EditUser", "Admin", new { id = user.Id, msg = "Email already exists.", color = "red" });
                        }

                        User getuser = new UserBL().GetActiveUserById(user.Id, de);
                        getuser.Name = user.Name;
                        //getuser.Gender = user.Gender;                 change 2/18/2022
                        getuser.Gender = 1;                        //   change 2/18/2022
                        getuser.Email = user.Email;
                        getuser.Contact = user.Contact;
                        getuser.Password = StringCipher.Encrypt(user.Password);

                        bool edit = new UserBL().UpdateUser(getuser, de);
                        if (edit == true)
                        {
                            //return RedirectToAction("List", "Admin", new { id = user.Id, msg = "User has been updated.", color = "green" });
                            return RedirectToAction("List", "Admin", new { msg = "User has been updated.", color = "green" });

                        }

                        return RedirectToAction("list", "Admin", new { msg = "Something's went wrong.", color = "red" });

                        //return RedirectToAction("EditUser", "Admin", new { id = user.Id, msg = "Something wents wrong.", color = "red" });
                    }


                    [HttpPost]
                    public ActionResult DeleteUser(int id)
                    {
                        //if (gp.ValidateLoggedinUser() == null)
                        //{
                        //    return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        //}
                        //else
                        //{
                        //    if (gp.ValidateLoggedinUser().Role == 2)
                        //    {
                        //        return RedirectToAction("Login", "Auth");
                        //    }

                        //}

                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }


                        User getUser = new UserBL().GetActiveUserById(id, de);
                        getUser.IsActive = 0;
                        bool edit = new UserBL().UpdateUser(getUser, de);
                        if (edit == true)
                        {
                            return RedirectToAction("List", "Admin", new { msg = "User has been deleted.", color = "green" });
                        }
                        return RedirectToAction("List", "Admin", new { msg = "Something wents wrong.", color = "red" });
                    }


            #endregion User Ending


            #region Car Detail Starting
                    public ActionResult CarDetailList(string msg = "", string color = "black")
                    {
                            if (!CheckLogin())
                            {
                                return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                            }

                            ViewBag.Message = msg;
                            ViewBag.Color = color;

                            List<CarDetail> cdList = new CarDetailBL().GetActiveCarDetailList(de);
                            ViewBag.List = cdList;
                            return View();
                    }

                    public ActionResult AddCarDetail(string msg = "", string color = "black")
                    {
                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }

                        ViewBag.Message = msg;
                        ViewBag.Color = color;

                        return View();
                    }

                    [HttpPost]
                    public ActionResult PostAddCarDetail(CarDetail carDetail)
                    {
                        try
                        {
                            if (!CheckLogin())
                            {
                                return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                            }

                            //User checkMail = new UserBL().GetActiveUsersList(de).Where(x => x.Email.ToLower() == user.Email.ToLower() && x.Id != user.Id).FirstOrDefault();
                            //if (checkMail != null)
                            //{
                            //    return RedirectToAction("AddUser", "Admin", new { msg = "Email already exists.", color = "red" });
                            //}

                            //StringCipher.Encrypt(user.Password);
                            //user.CreatedAt = gp.DateTimeNow();
                            //user.IsActive = 1;
                            //user.Role = 2;


                            CarDetail cd = new CarDetail()
                            {
                                LotYear = carDetail.LotYear.Trim(),
                                LotMake = carDetail.LotMake.Trim(),
                                LotModel = carDetail.LotModel.Trim(),
                                LotRunCondition = carDetail.LotRunCondition.Trim(),
                                DamageTypeDescription = carDetail.DamageTypeDescription.Trim(),
                                CopartFacilityName = carDetail.CopartFacilityName.Trim(),
                                SaleTitleState = carDetail.SaleTitleState.Trim(),
                                SaleTitleType = carDetail.SaleTitleType.Trim(),
                                DamageType = carDetail.DamageType.Trim(),
                                LotColor = carDetail.LotColor.Trim(),
                                HasKey = carDetail.HasKey.Trim(),
                                OdometerReading = carDetail.OdometerReading.Trim(),
                                SalePrice = carDetail.SalePrice.Trim(),
                                RepairCost = carDetail.RepairCost.Trim(),

                                IsActive = 1,
                                CreatedAt = gp.DateTimeNow()
                            };


                            bool check = new CarDetailBL().AddCarDetail(cd, de);
                            if (check == true)
                            {
                                return RedirectToAction("AddCarDetail", "Admin", new { msg = "Record has been added.", color = "green" });
                            }
                            return RedirectToAction("AddCarDetail", "Admin", new { msg = "Something wents wrong.", color = "green" });
                        }

                        catch (Exception e)
                        {
                            return RedirectToAction("AddCarDetail", "Admin", new { msg = e.Message , color ="green" });
                        }


                    }

                    [HttpPost]
                    public ActionResult PostEditCarDetail(CarDetail carDetail)
                    {
                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }

                        CarDetail getcd = new CarDetailBL().GetActiveCarDetailById(carDetail.Id, de);
                        getcd.LotYear = carDetail.LotYear;
                        getcd.LotMake = carDetail.LotMake;
                        getcd.LotModel = carDetail.LotModel;
                        getcd.LotRunCondition = carDetail.LotRunCondition;
                        getcd.DamageTypeDescription = carDetail.DamageTypeDescription;
                        getcd.CopartFacilityName = carDetail.CopartFacilityName;
                        getcd.SaleTitleState = carDetail.SaleTitleState;
                        getcd.SaleTitleType = carDetail.SaleTitleType;
                        getcd.DamageType = carDetail.DamageType;
                        getcd.LotColor = carDetail.LotColor;
                        getcd.HasKey = carDetail.HasKey;
                        getcd.OdometerReading = carDetail.OdometerReading;
                        getcd.SalePrice = carDetail.SalePrice;
                        getcd.RepairCost = carDetail.RepairCost;

                        bool edit = new CarDetailBL().UpdateCarDetail(getcd, de);
                        if (edit == true)
                        {
                            return RedirectToAction("CarDetailList", "Admin", new { msg = "User has been updated.", color = "green" });
                        }

                        return RedirectToAction("CarDetailList", "Admin", new { msg = "Something wents wrong.", color = "red" });

                    }

                    [HttpPost]
                    public ActionResult DeleteCarDetail(int id)
                    {
                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }


                        CarDetail getCarDetail = new CarDetailBL().GetActiveCarDetailById(id, de);
                        getCarDetail.IsActive = 0;
                        bool edit = new CarDetailBL().UpdateCarDetail(getCarDetail, de);
                        if (edit == true)
                        {
                            return RedirectToAction("CarDetailList", "Admin", new { msg = "Record has been deleted.", color = "green" });
                        }
                        return RedirectToAction("CarDetailList", "Admin", new { msg = "Something wents wrong.", color = "red" });
                    }

            #endregion Car Detail Ending


            #region Manage CSV

                    public ActionResult ImportCsv(string msg = "", string color = "black")
                    {
                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }

                        ViewBag.Message = msg;
                        ViewBag.Color = color;

                        return View();
                    }

                    [HttpPost]
                    public ActionResult PostImportCsv(HttpPostedFileBase PostedFile)
                    {
                        if (!CheckLogin())
                        {
                            return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
                        }

                        try
                        {

                            string fileName = Path.GetFileName(PostedFile.FileName);
                            string fileExt = Path.GetExtension(PostedFile.FileName);

                            if(fileExt.ToLower() != ".csv")
                            {
                                return RedirectToAction("ImportCsv", "Admin", new { msg = "Only .csv format allowed", color = "red" });
                            }

                            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                            {
                                HasHeaderRecord = false,
                                HeaderValidated = null,
                                BadDataFound = null
                            };

                            List<CarDetailDTO> cd_DtoList = new List<CarDetailDTO>();

                            using (var streamReader = new StreamReader(PostedFile.InputStream))
                            {
                                using (var csvReader = new CsvReader(streamReader, config))
                                {
                                    cd_DtoList = csvReader.GetRecords<CarDetailDTO>().ToList();
                                }
                            }

                            int totalRow = cd_DtoList.Count();
                            int flag = 0;
                            int count = 0;
                            foreach (CarDetailDTO cddto in cd_DtoList)
                            {
                                if (count > 0)
                                {
                                    CarDetail cd = new CarDetail()
                                    {
                                        LotYear = cddto.LotYear,
                                        LotMake = cddto.LotMake,
                                        LotModel = cddto.LotModel,
                                        LotRunCondition = cddto.LotRunCondition,
                                        DamageTypeDescription = cddto.DamageTypeDescription,
                                        CopartFacilityName = cddto.CopartFacilityName,
                                        SaleTitleState = cddto.SaleTitleState,
                                        SaleTitleType = cddto.SaleTitleType,
                                        DamageType = cddto.DamageType,
                                        LotColor = cddto.LotColor,
                                        HasKey = cddto.HasKey,
                                        OdometerReading = cddto.OdometerReading,
                                        SalePrice = cddto.SalePrice,
                                        RepairCost = cddto.RepairCost,

                                        IsActive = 1,
                                        CreatedAt = gp.DateTimeNow()
                                    };


                                    bool chk = new CarDetailBL().AddCarDetail(cd, de);

                                    if (chk == false)
                                    {
                                        flag = 1;
                                    }
                                }
                                count++;
                            }

                            if (flag == 0)
                            {
                                return RedirectToAction("ImportCsv", "Admin", new { msg = "File imported successfully.", color = "green" });
                            }
                            else
                            {
                                return RedirectToAction("ImportCsv", "Admin", new { msg = "File imported with errors.", color = "orange" });
                            }
                        }
                        catch (Exception ex)
                        {
                            return RedirectToAction("ImportCsv", "Admin", new { msg = ex.Message, color = "red" });
                        }
                    }

            #endregion
    }
}