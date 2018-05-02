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
    public class CorrectionTransController : Controller
    {
        BerryDB db;        

        public CorrectionTransController()
        {

        }
        //
        // GET: /CorrectionTrans/

        #region Views
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult _SearchTrans()
        {
            return View("_SearchTrans");
        }

        public ViewResult _AddTrans()
        {
            return View("_AddTrans");
        }

        #endregion


       
        public ContentResult GetHistDetail(int edition, string canvCode, string db, int subscrID)
        {
            this.db = new BerryDB();

            try
            {
                DataTable histDetail = this.db.GetHistDetail(edition, canvCode, db, subscrID);
                return Content(JsonConvert.SerializeObject(histDetail), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }

        }   
        public ContentResult GetActiveCanvass()
        {
            this.db = new BerryDB();

            try
            {
                DataTable histDetail = this.db.GetActiveCanvass();
                return Content(JsonConvert.SerializeObject(histDetail), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }
        }


        public ContentResult GetActiveBooks()
        {
            this.db = new BerryDB();

            try
            {
                DataTable histDetail = this.db.GetActiveBooks();
                return Content(JsonConvert.SerializeObject(histDetail), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ContentResult GetActiveRepSales()
        {
            this.db = new BerryDB();

            try
            {
                DataTable repSales = this.db.GetActiveRepSales();
                return Content(JsonConvert.SerializeObject(repSales), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }

        }  
        public ContentResult GetGrpCodeList()
        {
            this.db = new BerryDB();

            try
            {
                DataTable products = this.db.GetGrpCodeList();
                return Content(JsonConvert.SerializeObject(products), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }

        }        
        public ContentResult GetCanvWeek(int canvEdition, string canvCode, string db)
        {
            this.db = new BerryDB();

            try
            {
                DataTable products = this.db.GetCanvWeekDB(canvEdition, canvCode, db);
                return Content(JsonConvert.SerializeObject(products), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }

        }


        #region Actions
        public ContentResult InsertCommHistTrans(CommisionHistTrans transaction)
        {

            this.db = new BerryDB();

            try
            {
                
                DataTable result = this.db.InsertCommHistTrans(transaction, Security.CurrentUser.UserId);
                return Content(JsonConvert.SerializeObject(result), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }
        }
        public ContentResult UpdateCommHistTrans(CommisionHistTrans transaction)
        {

            this.db = new BerryDB();

            try
            {
                string result = this.db.UpdateCommHistTrans(transaction, Security.CurrentUser.UserId);
                return Content(JsonConvert.SerializeObject(result), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }
        }
        public ContentResult DeleteCommHistTrans(CommisionHistTrans transaction)
        {

            this.db = new BerryDB();

            try
            {
                string result = this.db.DeleteCommHistTrans(transaction);
                return Content(JsonConvert.SerializeObject(result), "json/application");

            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

    }
}