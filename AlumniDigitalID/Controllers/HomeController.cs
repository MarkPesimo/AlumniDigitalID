using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumniDigitalID.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Success()
        {
            return View();
        }


        //[Authorize]
        //public ActionResult Perks()
        //{
        //    return View();
        //}

        //[Authorize]
        //public ActionResult Privacy()
        //{
        //    return View();
        //}



        //[Authorize]
        //public ActionResult About()
        //{
        //    return View();
        //}



    }
}