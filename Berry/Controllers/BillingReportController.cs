using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Berry.DBConsultor;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using Berry.Models;
using System.Reflection;
using Berry.Utils;
using Berry.Enums;

namespace Berry.Controllers
{
    [CustomAuthorize(Roles.Administrator)]
    public class BillingReportController : Controller
    {
        BerryDB db = new BerryDB();
        
        //
        // GET: /BillingReport/
        public ActionResult Report()
        {
            return View();
        }


        public ContentResult GetBillingPivotReport(int canvEdition, string startWeek, string endWeek)
        {
            db = new BerryDB();
            try
            {
                DataTable dt = db.GetBillingPivotReport(canvEdition, startWeek, endWeek);
                return Content(JsonConvert.SerializeObject(dt), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }

        }

        public DataTable GetBillingReport(int canvEdition, string startWeek, string endWeek)
        {
            db = new BerryDB();
            try
            {
                DataTable dt = db.GetBillingReport(canvEdition, startWeek, endWeek);
                return dt;

            }
            catch (Exception e)
            {
                throw;
            }

        }

        public FileContentResult GetPDF(string filename)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".PDF");
                Response.TransmitFile(Server.MapPath("~/GenerateReports/" + filename + ".PDF"));
                Response.End();
            }
            catch (Exception e)
            {
                throw;
            }
            return null;
        }


        public JsonResult GenerateAndDisplayReport(int canvEdition, string startWeek, string endWeek)
        {
            try
            {
                List<string> fileNames = new List<string>();
                List<string> filePaths = new List<string>();
                
                var serverUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                    (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                LocalReport localReport = new LocalReport();
                localReport.ReportPath = Server.MapPath("~/Reports/Billing.rdlc");

                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "BillingDS";

                DataTable dtReports = GetBillingReport(canvEdition, startWeek, endWeek);

               
                List<BillingReport> reports = Converter.DataTableToList<BillingReport>(dtReports);

                foreach (var canvReport in reports.GroupBy(c => c.Canv_Code).ToList())
                {

                    foreach (var prodReport in canvReport.GroupBy(p => p.Prod_Grp_Code))
                    {

                        foreach (var weekReport in prodReport.GroupBy(p => p.Canv_Week))
                        {
                            reportDataSource.Value = weekReport;

                            localReport.DataSources.Add(reportDataSource);
                            string reportType = "PDF";
                            string mimeType = "";
                            string encoding = "";
                            string fileNameExtension = "";

                            Warning[] warnings = null;
                            string[] streams = null;
                            byte[] renderedBytes = null;

                            string canvass = weekReport.Select(c => c.Canv_Code).Distinct().FirstOrDefault().ToString();
                            string week = weekReport.Select(w => w.Canv_Week).Distinct().FirstOrDefault().ToString();
                            string prod = weekReport.Select(p => p.Prod_Grp_Code).Distinct().FirstOrDefault().ToString();

                            string fileName = string.Format("{0} {1} {2}", canvass, week, prod.Trim());

                            fileNames.Add(fileName);


                            //Render the report            
                            renderedBytes = localReport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                            using (FileStream fs = new FileStream(Server.MapPath("~/GenerateReports/" + fileName + ".PDF"), FileMode.Create))
                            {
                                fs.Write(renderedBytes, 0, renderedBytes.Length);
                            }

                        }
                    }

                }

                foreach (string filename in fileNames)
                {
                    filePaths.Add(String.Format("{0}{1}", serverUrl, Url.Content("~/BillingReport/GetPDF?filename="+filename)));
                }

                return Json(new { file = filePaths });
            }
            catch (Exception e)
            {
               // return Json(e);
                throw e;
            }
        }
    

	}
}