using Berry.DBConsultor;
using Berry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Microsoft.Reporting.WebForms;


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

        [HttpGet]
        public FileResult GetFinancialData(string startDate = "01/01/2018", string endDate = "04/01/2018")
        {
            string mime = string.Empty;
            string enconding = string.Empty;
            string extension = string.Empty;
            string[] streamIds;
            db = new BerryDB();
            //int userId = ((User)(Session["user"])).UserId;
            DataTable tbl = db.GetFinancialData(startDate, endDate);

            Warning[] warnings;
            LocalReport viewer = new LocalReport();
            viewer.ReportPath = Server.MapPath("~/Reports/Rdl/finace.rdlc");
            ReportDataSource source = new ReportDataSource("finance", tbl);
            source.Name = "finance";
            viewer.EnableExternalImages = true;
            viewer.SetParameters(new List<ReportParameter>
            {
                new ReportParameter("FechaDesde", startDate),
                new ReportParameter("FechaHasta", endDate),
                new ReportParameter("Logo", new Uri(Server.MapPath("~/img/logoPA.PNG")).AbsoluteUri)
            });

            viewer.DataSources.Add(source);
            viewer.Refresh();

            byte[] file = viewer.Render(
                format: "PDF",
                deviceInfo: null,
                mimeType: out mime, 
                encoding: out enconding, 
                fileNameExtension: out extension, 
                streams: out streamIds, 
                warnings: out warnings);

            return File(file, mime);
        }


    }
}