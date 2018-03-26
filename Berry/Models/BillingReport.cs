using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Berry.Models
{
    public class BillingReport
    {
        public string Rep_Info { get; set; }
        public string Subscr_ID { get; set; }
        public string Phone { get; set; }
        public string Subscr_Name { get; set; }
        public string Canv_Code { get; set; }
        public string Canv_Edition { get; set; }
        public string Prod_Grp_Code { get; set; }
        public string Prosp { get; set; }
        public double Prod_Co { get; set; }
        public double Prod_Ci { get; set; }
        public int CGI { get; set; }
        public double Renew_Ci { get; set; }
        public double Increase_Ci { get; set; }
        public double Decrease_Ci { get; set; }
        public double Np_Ci { get; set; }
        public double Op_Ci { get; set; }
        public double Otc_Ci { get; set; }
        public double Tot_Com { get; set; }
        public int Canv_Week { get; set; }
        public double Ctrl_Loss { get; set; }
        public double Unctrl_Loss { get; set; }
        public double Np_Cnt { get; set; }
        public double Op_Cnt { get; set; }
        public double Otc_Np_Cnt { get; set; }
        public int Channel { get; set; }
        public double Net_Sales { get; set; }
    }
}