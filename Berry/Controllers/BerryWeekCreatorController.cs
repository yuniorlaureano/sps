using Berry.DBConsultor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Berry.Controllers
{
    public class BerryWeekCreatorController : Controller
    {
        BerryDB db;
        //
        // GET: /BerryWeekCreator/
        public ActionResult Index()
        {
            return View();
        }

        public ContentResult CreateCanvWeek(int canvEdition, string canvCode, DateTime endDate)
        {
            db = new BerryDB();
            try
            {
                string log = db.CreateCanvWeek(canvEdition, canvCode, endDate);
                return Content(JsonConvert.SerializeObject(log), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }

        }

       
	}
}