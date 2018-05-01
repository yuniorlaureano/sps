using Berry.DBConsultor;
using Berry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System.IO;

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

        [HttpGet]
        public FileResult GetDimmasVsSpsData(string startDate = "01/01/2018", string endDate = "04/01/2018")
        {
            string mime = string.Empty;
            string enconding = string.Empty;
            string extension = string.Empty;
            string[] streamIds;
            db = new BerryDB();
            //int userId = ((User)(Session["user"])).UserId;
            DataTable tbl = db.GetDimmasVsSpsData(startDate, endDate);

            Warning[] warnings;
            LocalReport viewer = new LocalReport();
            viewer.ReportPath = Server.MapPath("~/Reports/DimmasVsSps/dimmasVsSps.rdlc");
            ReportDataSource source = new ReportDataSource("dmsvssps", tbl);
            source.Name = "dmsvssps";
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

        [HttpGet]
        public FileResult GetUnitData(string startDate = "01/01/2018", string endDate = "04/01/2018")
        {
            string mime = string.Empty;
            string enconding = string.Empty;
            string extension = string.Empty;
            string[] streamIds;
            db = new BerryDB();
            //int userId = ((User)(Session["user"])).UserId;
            DataTable tbl = db.GetUnitData(startDate, endDate);

            Warning[] warnings;
            LocalReport viewer = new LocalReport();
            viewer.ReportPath = Server.MapPath("~/Reports/Unit/Unit.rdlc");
            ReportDataSource source = new ReportDataSource("UnitDS", tbl);
            source.Name = "UnitDS";
            //viewer.EnableExternalImages = true;
            viewer.SetParameters(new List<ReportParameter>
            {
                new ReportParameter("startDate", startDate),
                new ReportParameter("endDate", endDate),
                //new ReportParameter("Logo", new Uri(Server.MapPath("~/img/logoPA.PNG")).AbsoluteUri)
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

        public ViewResult ReporteDeVentas()
        {
            return View();
        }

        public JsonResult GenerateSalesReport(string startDate = "01/01/2018", string endDate = "04/01/2018")
        {
            return Json( new { startDate = startDate, endDate = endDate }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesReportData(string startDate = "01/01/2018", string endDate = "04/01/2018")
        {
            db = new BerryDB();
            User user = ((User)(Session["user"]));
            string roles = string.Join(",", user.Roles);

            DataTable tbl = db.GetSalesReportData(roles.ToLower() ,user.UserId ,startDate, endDate);

            return Json(JsonConvert.SerializeObject(tbl), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesReportSubscriberData(int employeeId, string startDate = "01/01/2018", string endDate = "04/01/2018")
        {
            db = new BerryDB();
            
            DataTable tbl = db.GetSalesReportSubscriberData(employeeId, startDate, endDate);

            return Json(JsonConvert.SerializeObject(tbl), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPayPeriodData(string reportType, int edition, int canvWeek, string startDate = "01/01/2018", string endDate = "04/01/2018")
        {
            List<ReportParameter> prm = new List<ReportParameter>()
            {
                new ReportParameter("year", edition.ToString()),
                new ReportParameter("payperiod", canvWeek.ToString()),
                new ReportParameter("startDate", startDate),
                new ReportParameter("endDate", endDate)
            };

            string format = string.Empty;
            List<FileResultViewModel> fileResult = null;

            switch(reportType)
            {
                case "EXCEL": format = ".xls";
                    break;
                case "PDF": format = ".pdf";
                    break;
            }

            db = new BerryDB();
            User user = ((User)(Session["user"]));

            string roles = string.Join(",", user.Roles);
            string savingPath = Server.MapPath("~/Content/Files/");
            string rdlcPath = Server.MapPath("~/Reports/");


            DataTable payPeriodGeneral = db.GetFinalSalesReportData("SUMMARY", roles.ToLower(), user.UserId, startDate, endDate);
            DataTable payPeriodDetails = db.GetFinalSalesReportData("SUMMARY_FUCK", roles.ToLower(), user.UserId, startDate, endDate);

            ReportHelper(prm, "PayPeriodDS", rdlcPath + "PayPeriod.rdlc", "PayPeriodDS", false, reportType, payPeriodGeneral, savingPath + "payperiod-" + user.UserId + format);
            ReportHelper(prm, "PayPeriodDetailDS", rdlcPath + "PayPeriodDetail.rdlc", "PayPeriodDetailDS", false, reportType, payPeriodDetails, savingPath + "payperiod-details-" + user.UserId + format);

            fileResult = new List<FileResultViewModel>
            {
                new FileResultViewModel
                {
                    FileName = "payperiod-" + user.UserId,
                    Format = format,
                    FullPath = "~/Content/Files/",
                },
                new FileResultViewModel
                {
                    FileName = "payperiod-details-" + user.UserId,
                    Format = format,
                    FullPath = "~/Content/Files/",
                }
            };

            return Json(fileResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        private string ReportHelper(List<ReportParameter> prm, string dataSource, string rdlcPath, string dataSet, bool hasImage, string format, DataTable data, string savingPath)
        {
            string mime = string.Empty;
            string enconding = string.Empty;
            string extension = string.Empty;
            string[] streamIds;

            Warning[] warnings;
            LocalReport viewer = new LocalReport();
            viewer.ReportPath = rdlcPath;
            ReportDataSource source = new ReportDataSource(dataSource, data);
            source.Name = dataSet;

            if (hasImage)
            {
                viewer.EnableExternalImages = hasImage;
            }
            

            if (prm != null)
            {
                viewer.SetParameters(prm);
            }

            viewer.DataSources.Add(source);
            viewer.Refresh();

            byte[] file = viewer.Render(
                format: format,
                deviceInfo: null,
                mimeType: out mime,
                encoding: out enconding,
                fileNameExtension: out extension,
                streams: out streamIds,
                warnings: out warnings);

            using (FileStream fs = new FileStream(savingPath, FileMode.Create))
            {
                fs.Write(file, 0, file.Length);
            }

            return savingPath;
        }

        public FileResult DownloadFile(string fullFile, string mime, string fileName)
        {
            return File(Server.MapPath(fullFile), mime, fileName);
        }
    }
}