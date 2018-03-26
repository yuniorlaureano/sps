using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Berry.DBConsultor;
using Berry.Models;
using Berry.Utils;

namespace Berry.Controllers
{
    public class RegionalReportController : Controller
    {
        BerryDB db;
        //
        // GET: /RegionalReport/
        public ActionResult Index()
        {
            return View();
        }

        public DataTable GetRegionDetail(int canvEdition, string startWeek, string endWeek)
        {
            db = new BerryDB();
            try
            {
                DataTable dt = db.GetRegionSalesDetail(canvEdition, startWeek, endWeek);
                return dt;

            }
            catch (Exception e)
            {
                throw;
            }

        }

        public DataTable GetOfficeDetail(int canvEdition, string startWeek, string endWeek)
        {
            db = new BerryDB();
            try
            {
                DataTable dt = db.GetOfficeDetail(canvEdition, startWeek, endWeek);
                return dt;

            }
            catch (Exception e)
            {
                throw;
            }

        }

        public DataTable GetVisitsDetail(int canvEdition, string startWeek, string endWeek)
        {
            db = new BerryDB();
            try
            {
                DataTable dt = db.GetVisitsDetail(canvEdition, startWeek, endWeek);
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

                ReportDataSource rdsRegion = new ReportDataSource();
                ReportDataSource rdsOD = new ReportDataSource();
                ReportDataSource rdsVisits = new ReportDataSource();

                rdsRegion.Name = "RegionalDS";
                rdsOD.Name = "OfficeDetail";
                rdsVisits.Name = "VisitsDS";

                var serverUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                    (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                LocalReport localReport = new LocalReport();
                localReport.ReportPath = Server.MapPath("~/Reports/Regional.rdlc");

               
                ReportParameter[] parameters = new ReportParameter[3];
                parameters[0] = new ReportParameter("startDate", startWeek);
                parameters[1] = new ReportParameter("endDate", endWeek);
                parameters[2] = new ReportParameter("canvEdition", canvEdition.ToString());

                localReport.SetParameters(parameters);

                DataTable dtRegion = GetRegionDetail(canvEdition, startWeek, endWeek);
                DataTable dtOfficeDetail = GetOfficeDetail(canvEdition, startWeek, endWeek);
                DataTable dtVisits = GetVisitsDetail(canvEdition, startWeek, endWeek);


                List<RegionalReport.RegionSection> rptRegion = Converter.DataTableToList<RegionalReport.RegionSection>(dtRegion);
                List<RegionalReport.OfficeDetail> rptOfficeDetail = Converter.DataTableToList<RegionalReport.OfficeDetail>(dtOfficeDetail);
                List<RegionalReport.VisitsDetail> rptVisits = Converter.DataTableToList<RegionalReport.VisitsDetail>(dtVisits);

                rdsRegion.Value = rptRegion;
                rdsOD.Value = rptOfficeDetail;
                rdsVisits.Value = rptVisits;

                localReport.DataSources.Add(rdsRegion);
                localReport.DataSources.Add(rdsOD);
                localReport.DataSources.Add(rdsVisits);

                string reportType = "PDF";
                string mimeType = "";
                string encoding = "";
                string fileNameExtension = "";

                Warning[] warnings = null;
                string[] streams = null;
                byte[] renderedBytes = null;

                string fileName = string.Format("Reporte Regional");

                fileNames.Add(fileName);


                //Render the report            
                renderedBytes = localReport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                using (FileStream fs = new FileStream(Server.MapPath("~/GenerateReports/" + fileName + ".PDF"), FileMode.Create))
                {
                    fs.Write(renderedBytes, 0, renderedBytes.Length);
                }

                //    }

                //}

                foreach (string filename in fileNames)
                {
                    filePaths.Add(String.Format("{0}{1}", serverUrl, Url.Content("~/RegionalReport/GetPDF?filename=" + filename)));
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