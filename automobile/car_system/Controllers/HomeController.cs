//using CountryData.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CarSystem.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Offer()
        {  

            return View();
        }

        public ActionResult ResultPage(string price = "")
        {
            ViewBag.Price = price;
            return View();
        }

    }
}