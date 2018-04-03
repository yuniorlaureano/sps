using Berry.DBConsultor;
using Berry.Enums;
using Berry.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Berry.Utils
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        BerryDB db = new BerryDB();
        public CustomAuthorize(params Roles[] roles)
        {
            foreach (var rol in roles) 
            {
                var rolName = Enum.GetName(typeof(Roles), rol);
                if (Roles != "" && Roles != null)
                {
                    Roles += ",";
                }
                Roles += ConfigurationManager.AppSettings[rolName].ToString();
            }

            Roles = Roles.ToLower();
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            User user = (User) System.Web.HttpContext.Current.Session["user"];
            bool isAuthorize = false;

            foreach (string role in user.Roles)
            {
                if (Roles.Contains(role.ToLower()))
                {
                    isAuthorize = true;
                    break;
                }
            }

            return isAuthorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                                            new RouteValueDictionary
                                       {
                                           { "action", "Unauthorized" },
                                           { "controller", "Error" }
                                       });
        }
    }
}
