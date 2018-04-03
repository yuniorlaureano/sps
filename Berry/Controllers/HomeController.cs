using Berry.DBConsultor;
using Berry.Enums;
using Berry.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Berry.Controllers
{
    [CustomAuthorize(Roles.Administrator, Roles.SalesRepresentative, Roles.SalesSupervisor, Roles.SalesDirector)]
    public class HomeController : Controller
    {
        BerryDB db;

        public ActionResult Index()
        {
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


        //public ContentResult GetFullAccess()
        //{
        //    Security security = new Security();
        //    BerryDB db = new BerryDB();
        //    var isAuthorized = true;

        //    try
        //    {
        //        var username = security.GetWinUser();

        //        //Trae todo los usuarios y roles del usuario que intenta hacer request
        //        var userInroles = db.GetRoles("", username, ConfigurationManager.AppSettings["moduleCode"].ToString());

        //        //System.Web.HttpContext.Current.Session.Add("user", userInroles);

        //        //if (userInroles.grp_codigo != null)
        //        //{
        //        //    if (userInroles.grp_codigo.ToLower() == "adm")
        //        //    {
        //        //        isAuthorized = true;
        //        //    }
        //        //}
        //        return Content(JsonConvert.SerializeObject(isAuthorized), "json/application");

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        public ActionResult GetLastGeneratedWeek()
        {
            db = new BerryDB();
            DataTable log = db.GetLastGeneratedWeek();

            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDashboardRnCount()
        {
            db = new BerryDB();
            DataTable log = db.GetDashboardRnCount();

            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

    }
}