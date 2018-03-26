using Berry.DBConsultor;
using Berry.Enums;
using Berry.Models;
using Berry.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Berry.Controllers
{
    [CustomAuthorize(Roles.Administrator)]
    public class CompareReportsController : Controller
    {
        BerryDB consultor;
        
        //
        // GET: /CompareReports/
        public ActionResult Compares()
        {
            return View();
        }

        public ActionResult UniverseCompares()
        {
            return View();
        }



        #region SMS Compare
        public ContentResult SMSVsDimmas()
        {
            consultor = new BerryDB();
            try
            {
                DataSet ds = consultor.GetSMSvsDimmasReport();

                return Content(JsonConvert.SerializeObject(ds), "application/json");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ContentResult DimmasVsSMS()
        {
            consultor = new BerryDB();
            try
            {
                DataSet ds = consultor.GetDimmasVsSMSReport();

                return Content(JsonConvert.SerializeObject(ds), "application/json");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ContentResult SMSvsDimmasRejects()
        {
            consultor = new BerryDB();
            try
            {
                DataSet ds = consultor.GetSMSvsDimmasRejectsReport();

                return Content(JsonConvert.SerializeObject(ds), "application/json");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ContentResult AuditTransaction(List<Audit> transactions)
        {
            consultor = new BerryDB();
            Security control = new Security();
            string comment = string.Empty;
            AuditProcess process = new AuditProcess();

            try{

                foreach (Audit audit in transactions)
            {
                //Change
                switch (audit.Action)
                {
                    case AuditAction.Devolver:
                        comment = process.RejectTransaction(audit.Info);
                       // total++;
                        break;
                    case AuditAction.Auditar:
                        comment = process.AuditTransaction(audit.Info);
                        //total++;
                        break;
                    default:
                        break;
                }
            }
            
                return Content(JsonConvert.SerializeObject(comment), "application/json");

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion SMS Compare

        #region Universe Compare
        public ContentResult DimmasBerry()
        {
            consultor = new BerryDB();
            try
            {
                DataTable ds = consultor.GetComparesDimmasBerry();

                return Content(JsonConvert.SerializeObject(ds), "application/json");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ContentResult DimmasBerryAcum()
        {
            consultor = new BerryDB();
            try
            {
                DataTable dt = consultor.GetDimmasBerryAcum();

                return Content(JsonConvert.SerializeObject(dt), "application/json");

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ContentResult DimmasNoBerry()
        {
            consultor = new BerryDB();
            try
            {
                DataTable dt = consultor.GetCompareDimmasNoBerry();

                return Content(JsonConvert.SerializeObject(dt), "application/json");

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion Universe Compare

        public ContentResult GetTotalDifCompares()
        {
            consultor = new BerryDB();
            try
            {
                DataSet ds = Unpivot(consultor.GetTotalDifCompares());



                return Content(JsonConvert.SerializeObject(ds), "application/json");

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ContentResult GetTotalStatusCompares()
        {
            consultor = new BerryDB();
            try
            {
                DataTable dt = consultor.GetTotalStatusCompares();

                return Content(JsonConvert.SerializeObject(dt), "application/json");

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public DataSet Unpivot(DataSet ds)
        {
            DataRow row ;
            DataTable table = new DataTable();
            table.Columns.Add("DIFERENCIA");
            table.Columns.Add("TOTAL");

            DataSet result = new DataSet();

            foreach (DataTable dt in ds.Tables)
            {
                table.Clear();

                foreach (DataColumn column in dt.Columns)
                {
                    table.TableName = dt.TableName;
                    row = table.NewRow();
                    row["DIFERENCIA"] = column.ColumnName;
                    row["TOTAL"] = int.Parse(dt.Rows[0][column.ColumnName].ToString());
                    table.Rows.Add(row);
                }
                result.Tables.Add(table.Copy());
            }
            return result;
        }
     
	}
}