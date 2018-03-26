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
    
    public class CommonController : Controller
    {
        BerryDB db;

        public ContentResult GetActiveCanvEdition()
        {
            db = new BerryDB();
            try
            {
                DataTable edition = db.GetActiveCanvEdition();
                return Content(JsonConvert.SerializeObject(edition), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }

        }

        public ContentResult GetCanvassCodes()
        {
            this.db = new BerryDB();

            try
            {
                DataTable dt = this.db.GetActiveCanvass();
                return Content(JsonConvert.SerializeObject(dt), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }

        }
	}
}