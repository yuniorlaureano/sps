using Berry.DBConsultor;
using Berry.Models;
using Berry.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Berry
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            BerryDB db = new BerryDB();
            Security security = new Security();
            User user = db.GetRoles(security.GetWinUser(), ConfigurationManager.AppSettings["moduleCode"].ToString());
            HttpContext.Current.Session.Add("user", user);
        }
    }
}
