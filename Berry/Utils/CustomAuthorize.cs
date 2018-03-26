using Berry.DBConsultor;
using Berry.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
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
           
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            Security security = new Security();

            var isAuthorized = true;
            try
            {
                var username = security.GetWinUser();

                //Trae todo los usuarios y roles del usuario que intenta hacer request
                var userInroles = db.GetRoles("", username, ConfigurationManager.AppSettings["moduleCode"].ToString());

                System.Web.HttpContext.Current.Session.Add("user", userInroles);


                foreach (var role in Roles.Split(','))
                {
                    if (userInroles.grp_codigo.ToLower() == role.Trim().ToLower())
                    {
                        isAuthorized = true;
                        break;
                    }
                }
            }catch(Exception){

            }
            return isAuthorized; 
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
