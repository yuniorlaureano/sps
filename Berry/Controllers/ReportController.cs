using Berry.DBConsultor;
using Berry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Berry.Controllers
{
    public class ReportController : Controller
    {
        BerryDB db;
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DimmasVsSps(string startDate, string endDate)
        {
            db = new BerryDB();
            int userId = ((User)(Session["user"])).UserId;
            string file = db.GetDimmasVsSpsReport(
                startDate,
                endDate,
                "general",
                "reporte-dimmas-vs-weekly-" + userId,
                Server.MapPath("~/Content/Files/")
            );

            return Json("OK");
        }

        [HttpGet]
        public FileResult DimmasVsSps()
        {
            int userId = ((User)(Session["user"])).UserId;
            string file = Server.MapPath("~/Content/Files/reporte-dimmas-vs-weekly-" + userId + ".xlsx");
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporte-dimmas-vs-weekly-" + userId + ".xlsx");
        }

        [HttpPost]
        public JsonResult GetFinancialReport(string startDate, string endDate)
        {
            db = new BerryDB();
            int userId = ((User)(Session["user"])).UserId;
            string file = db.GetFinancialReport(
                startDate,
                endDate,
                "financial",
                "reporte-financial-" + userId,
                Server.MapPath("~/Content/Files/")
            );

            return Json("OK");
        }

        [HttpGet]
        public FileResult GetFinancialReport()
        {
            int userId = ((User)(Session["user"])).UserId;
            string file = Server.MapPath("~/Content/Files/reporte-financial-" + userId + ".xlsx");
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporte-financial-" + userId + ".xlsx");
        }
    }
}