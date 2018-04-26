using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Berry.Models;

namespace Berry.Utils
{
    public class Security : System.Web.UI.MasterPage
    {
        public static User CurrentUser { get; set; }
        public string GetWinUser()
        {
            string user = HttpContext.Current.User.Identity.Name;

            if (user.Contains("CSID"))
            {
                user = user.Replace("CSID\\", "");
            }
            else if (user.ToUpper().Contains("AXESA"))
            {
                user = user.ToUpper().Replace("AXESA\\", "");
            }

            return user;

        }

    }
}