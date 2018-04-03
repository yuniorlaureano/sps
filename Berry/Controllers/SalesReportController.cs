
using Berry.DBConsultor;
using Berry.Enums;
using Berry.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using Berry.Models;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace Berry.Controllers
{

    [CustomAuthorize(Roles.Administrator, Roles.SalesRepresentative, Roles.SalesSupervisor, Roles.SalesDirector)]
    public class SalesReportController : Controller
    {
        BerryDB db;
        //
        // GET: /PayPeriod/

        public ActionResult PayPeriod()
        {
            return View();
        }
        public ContentResult GetPayPeriod(int edition)
        {
            db = new BerryDB();
            try
            {
                DataTable payPeriod = db.GetPayPeriod(edition);
                return Content(JsonConvert.SerializeObject(payPeriod), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }

        }

        //public ContentResult GetPivotReport(int edition, int periodID)
        //{
        //    var user = (User)Session["user"];


        //    db = new BerryDB();
        //    try
        //    {
        //        DataSet payPeriod = db.GetPayPeriodPivotReport(edition, periodID, user.grp_codigo, user.usr_codigo);
        //        return Content(JsonConvert.SerializeObject(payPeriod), "json/application");


        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }

        //}

        //public DataSet GetPivotReportDT(int edition, int periodID)
        //{
        //    var user = (User)Session["user"];


        //    db = new BerryDB();
        //    try
        //    {
        //        DataSet payPeriod = db.GetPayPeriodPivotReport(edition, periodID, user.grp_codigo, user.usr_codigo);
        //        return payPeriod;


        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }

        //}


        public FileContentResult GetFile(string filename)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename );
                Response.TransmitFile(Server.MapPath("~/GenerateReports/" + filename));
                Response.End();
            }
            catch (Exception e)
            {
                throw;
            }
            return null;
        }

        public JsonResult GenerateAndDisplayReport(int edition, int periodID, string startDate, string endDate, PayPeriodReport payPeriodReports, int action)
        {
            try
            {

                FileType fileType = (FileType)action;
                string[] reportFormat = new string[2];
                List<string> filePaths = new List<string>();
                List<PayPeriodPivotReport> pivot = payPeriodReports.PivotReport;
                List<PayPeriodDetailReport> detailed = payPeriodReports.DetailedReport;

                if (fileType == FileType.PDF)
                {
                    reportFormat[0] = "PDF";
                    reportFormat[1] = ".PDF";
                }
                else
                {
                    reportFormat[0] = "EXCEL";
                    reportFormat[1] = ".XLS";
                }

                filePaths.Add(GenerateReports(edition, periodID, startDate, endDate, pivot, reportFormat, "PayPeriod", "PayPeriodDS"));
                filePaths.Add(GenerateReports(edition, periodID, startDate, endDate, detailed, reportFormat, "PayPeriodDetail", "PayPeriodDetailDS"));


                return Json(new { file = filePaths });
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public string GenerateReports<T>(int edition, int periodID, string startDate, string endDate, List<T> report, string[] format, string reportName, string dataSetName)
        {

            var serverUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/" + reportName + ".rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = dataSetName;

            ReportParameter[] parameters = new ReportParameter[4];
            parameters[0] = new ReportParameter("year", edition.ToString());
            parameters[1] = new ReportParameter("payperiod", periodID.ToString());
            parameters[2] = new ReportParameter("startDate", startDate);
            parameters[3] = new ReportParameter("endDate", endDate);

            localReport.SetParameters(parameters);

            reportDataSource.Value = report;

            localReport.DataSources.Add(reportDataSource);

            string reportType = format[0];
            string mimeType = "";
            string encoding = "";
            string fileNameExtension = "";

            Warning[] warnings = null;
            string[] streams = null;
            byte[] renderedBytes = null;

            string fileName = string.Format(" {0} {1} {2}", reportName, periodID, edition);


            //Render the report            
            renderedBytes = localReport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            using (FileStream fs = new FileStream(Server.MapPath("~/GenerateReports/" + fileName + format[1]), FileMode.Create))
            {
                fs.Write(renderedBytes, 0, renderedBytes.Length);
            }

            string filePath = String.Format("{0}{1}", serverUrl, Url.Content("~/SalesReport/GetFile?filename=" + fileName + format[1]));

            return filePath;
        }

    }
}