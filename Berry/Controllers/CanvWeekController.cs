using Berry.DBConsultor;
using Berry.Enums;
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
    public class CanvWeekController : Controller
    {
        BerryDB db;
        //
        // GET: /CanvWeek/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Close()
        {
            return View();
        }

        public ActionResult Open()
        {
            return View();
        }

        /// <summary>
        /// Retorna las semanas abiertas.
        /// </summary>
        /// <param name="database">BR, O YP</param>
        /// <returns>JsonResult</returns>
        public JsonResult GetOpenedWeek()
        {
            db = new BerryDB();
            DataTable log = db.GetOpenedWeek();

            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Permite cerrar una semana
        /// </summary>
        /// <param name="database">BR, O YP</param>
        /// <param name="startWeek">fecha de inicio</param>
        /// <param name="endWeek">fecha final</param>
        public JsonResult CloseWeek(string startWeek, string endWeek, int week)
        {
            db = new BerryDB();
            db.CloseWeek(startWeek, endWeek, week);
            
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna las semanas cerradas.
        /// </summary>
        /// <param name="database">BR, O YP</param>
        /// <returns>JsonResult</returns>
        public JsonResult GetClosedWeek()
        {
            db = new BerryDB();
            DataTable log = db.GetClosedWeek();

            return Json(JsonConvert.SerializeObject(log), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Permite abrir una semana
        /// </summary>
        /// <param name="database">BR, O YP</param>
        /// <param name="startWeek">fecha de inicio</param>
        /// <param name="endWeek">fecha final</param>
        public JsonResult OpenWeek(string startWeek, string endWeek, int week)
        {
            db = new BerryDB();
            db.OpenWeek(startWeek, endWeek, week);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

	}
}