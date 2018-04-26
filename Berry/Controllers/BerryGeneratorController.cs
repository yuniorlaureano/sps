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
    public class BerryGeneratorController : Controller
    {
        BerryDB db;
        //
        // GET: /BerryProcess/

        public ActionResult BerryGenerator()
        {
            return View();
        }


        public JsonResult GenerateBerry(string startWeek, string endWeek)
        {
            db = new BerryDB();
            try
            {
                int userId = ((User)(Session["user"])).UserId;
                db.GenerateBerry(startWeek, endWeek, userId);
                return Json("Berry generado", JsonRequestBehavior.AllowGet);

            } catch(Exception){
                throw;
            }

        }

        //public ContentResult GenerateBerryByProduct(string canvCode, int canvEdition, int week1, string productGroup, string DB)
        //{
        //    db = new BerryDB();
        //    try
        //    {
        //        DataTable log = null;
        //        db.GenerateBerryByProduct(canvCode, canvEdition, week1, productGroup, DB);
        //        return Content(JsonConvert.SerializeObject(log), "json/application");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}


        //public JsonResult GetActiveWeekProdGrp(string database)
        //{
        //    db = new BerryDB();
        //    try
        //    {
        //        DataTable log = db.GetActiveWeekProdGrp(database);//(canvEdition, startWeek, endWeek);
        //        return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }

        //}

        public JsonResult AutoCompleteOpenCanvEdition(string database)
        {
            db = new BerryDB();
            try
            {
                DataTable log = db.AutoCompleteOpenCanvEdition(database);
                return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        /// <summary>
        /// Obtiene los canv_week
        /// </summary>
        /// <param name="database"></param>
        /// <returns>DataTable</returns>
        public JsonResult GetWeekByDb()
        {
            db = new BerryDB();

            DataTable log = db.GetWeekByDb();

            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Obtiene los canv_week
        /// </summary>
        /// <param name="database"></param>
        /// <returns>DataTable</returns>
        public JsonResult GetGenaratedWeekByDb()
        {
            db = new BerryDB();

            DataTable log = db.GetGenaratedWeekByDb();

            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene los detalles de los canvweek
        /// </summary>
        /// <param name="database"></param>
        /// <returns>DataTable</returns>
        public JsonResult GetWeekDetialByDb(string database)
        {
            db = new BerryDB();

            DataTable log = db.GetWeekDetialByDb(database);

            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Optine las fechas conrespondiente al canvweek
        /// </summary>
        /// <param name="db"></param>
        /// <param name="canvEdition"></param>
        /// <returns>DataTable</returns>
        public JsonResult GetActiveDateByCanvWeek(int week)
        {
            db = new BerryDB();

            DataTable log = db.GetActiveDateByCanvWeek(week);

            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Optine las fechas conrespondiente al canvweek
        /// </summary>
        /// <param name="db"></param>
        /// <param name="canvEdition"></param>
        /// <returns>DataTable</returns>
        public JsonResult GetActiveGnDateByCanvWeek(int week)
        {
            db = new BerryDB();

            DataTable log = db.GetActiveGnDateByCanvWeek(week);

            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Optine los registros de weekly por fecha
        /// </summary>
        /// <param name="db"></param>
        /// <param name="canvEdition"></param>
        /// <returns>DataTable</returns>
        //public JsonResult GetOpenedWeekByDate(string database, int week, string startDate, string endDate)
        //{
        //    db = new BerryDB();

        //    DataTable log = db.GetOpenedWeekByDate(database, week, startDate, endDate);

        //    return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// Obtiene los detalles de los canv_week por fecha.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetOpenedWeekDetailsByDate(string startDate, string endDate)
        {
            db = new BerryDB();
            DataTable log = db.GetOpenedWeekDetailsByDate(startDate, endDate);
            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene los detalles de los canv_week por fecha.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetOpenedGnWeekDetailsByDate(string startDate, string endDate)
        {
            db = new BerryDB();
            DataTable log = db.GetOpenedGnWeekDetailsByDate(startDate, endDate);
            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene un little reporte despues de que corre berry.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>DataTable</returns>
        public JsonResult GetPostGeneratedBerryByDate(string startDate, string endDate)
        {
            db = new BerryDB();
            DataTable log = db.GetPostGeneratedBerryByDate(startDate, endDate);
            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene un little reporte despues de que corre berry.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>DataTable</returns>
        public JsonResult GetPostGeneratedBerryBarByDate(string startDate, string endDate)
        {
            db = new BerryDB();
            DataTable log = db.GetPostGeneratedBerryBarByDate(startDate, endDate);
            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Descarga el reporte general pos generacion de berry
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPostGeneratedXlsxReport(string startDate, string endDate)
        {
            db = new BerryDB();
            int userId = ((User)(Session["user"])).UserId;
            string file = db.ExportPostGeneratedWeeklyReport(
                startDate, 
                endDate,
                "general",
                "reporte-general-"+ userId,
                Server.MapPath("~/Content/Files/")
            );

            return Json("OK");
        }

        /// <summary>
        /// Descarga el reporte general pos generacion de berry
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet]
        public FileResult GetPostGeneratedXlsxReport()
        {
            int userId = ((User)(Session["user"])).UserId;
            string file = Server.MapPath("~/Content/Files/reporte-general-"+ userId +".xlsx");
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporte-general-"+ userId +".xlsx");
        }

        public JsonResult GetGeneratedEditions()
        {
            db = new BerryDB();
            DataTable log = db.GetGeneratedEditions();
            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGeneratedWeekByEdition(int edition)
        {
            db = new BerryDB();
            DataTable log = db.GetGeneratedWeekByEdition(edition);
            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

    }
}