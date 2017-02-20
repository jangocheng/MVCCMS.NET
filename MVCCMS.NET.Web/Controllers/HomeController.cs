using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCCMS.NET.Dal;

namespace MVCCMS.NET.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var bll = new Manager();


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}