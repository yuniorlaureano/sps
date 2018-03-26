using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Berry.Utils;
using Berry.Models;
using System.IO;
using Newtonsoft.Json;
using Berry.DBConsultor;


namespace Weekly.Controllers
{
    public class UnitReportController : Controller
    {
        BerryDB db;

        //
        // GET: /UnitReport/
        public ActionResult Index()
        {
            return View();
        }

        public DataTable GetSalesDetail(int canvEdition, string startWeek, string endWeek)
        {
            db = new BerryDB();
            try
            {
                DataTable dt = db.GetUnitSalesDetail(canvEdition, startWeek, endWeek);
                return dt;

            }
            catch (Exception e)
            {
                throw;
            }

        }

        public FileContentResult GetPDF(string fileName)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".PDF");
                Response.TransmitFile(Server.MapPath("~/GenerateReports/" + fileName + ".PDF"));
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
                localReport.ReportPath = Server.MapPath("~/Reports/Unit.rdlc");

                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "UnitDS";

                ReportParameter[] parameters = new ReportParameter[3];
                parameters[0] = new ReportParameter("startDate", startWeek);
                parameters[1] = new ReportParameter("endDate", endWeek);
                parameters[2] = new ReportParameter("canvEdition", canvEdition.ToString());

                localReport.SetParameters(parameters);

                DataTable dtReports = GetSalesDetail(canvEdition, startWeek, endWeek);


                List<UnitReport> reports = Converter.DataTableToList<UnitReport>(dtReports);

                //foreach (var canvReport in reports.GroupBy(c => c.Canv_Code).ToList())
                //{

                //    foreach (var prodReport in canvReport.GroupBy(p => p.Prod_Grp_Code))
                //    {
                reportDataSource.Value = reports;

                        localReport.DataSources.Add(reportDataSource);
                        string reportType = "PDF";
                        string mimeType = "";
                        string encoding = "";
                        string fileNameExtension = "";

                        Warning[] warnings = null;
                        string[] streams = null;
                        byte[] renderedBytes = null;

                        //string canvass = reports.Select(c => c.Canv_Code).Distin  ct().FirstOrDefault().ToString();
                        //string week = reports.Select(w => w.Canv_Week).Distinct().FirstOrDefault().ToString();
                        //string prod = reports.Select(p => p.Prod_Grp_Code).Distinct().FirstOrDefault().ToString();

                        string fileName = string.Format("Reporte de Unidades");

                        fileNames.Add(fileName);

                        string reportPath = Server.MapPath("~/GenerateReports/" + fileName + ".PDF");

                
            // Delete the file if it exists.
            if (System.IO.File.Exists(reportPath))
            {
                System.IO.File.Delete(reportPath);
            }
               
                        //Render the report            
                        renderedBytes = localReport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                        using (FileStream fs = System.IO.File.Create(reportPath))
                        {
                            fs.Write(renderedBytes, 0, renderedBytes.Length);
                        }

                //    }

                //}

                foreach (string filename in fileNames)
                {
                    filePaths.Add(String.Format("{0}{1}", serverUrl, Url.Content("~/UnitReport/GetPDF?filename=" + filename)));
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