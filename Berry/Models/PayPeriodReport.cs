using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Berry.Models
{
    public class PayPeriodReport
    {
        public List<PayPeriodDetailReport> DetailedReport { get; set; }
        public List<PayPeriodPivotReport> PivotReport { get; set; }
  
    }

    public class PayPeriodPivotReport
    {
        public string Supervisor { get; set; }
        public string Employee_ID { get; set; }
        public string Representative { get; set; }
        public double Prod_Co_Digital { get; set; }
        public double New_Digital { get; set; }
        public double Prod_Ci_Digital { get; set; }
        public double Prod_Co_Print { get; set; }
        public double New_Print { get; set; }
        public double Prod_Ci_Print { get; set; }
        public double Prod_Co { get; set; }
        public double New { get; set; }
        public double Prod_Ci { get; set; }
        public int? Year { get; set; }
        public int? PayPeriod { get; set; }
    }

    public class PayPeriodDetailReport
    {
        public string Canv_Code { get; set; }
        public int Canv_Edition { get; set; }
        public int Canv_Week { get; set; }
        public double Ctrl_Loss { get; set; }
        public double Decrease_Ci { get; set; }
        public string Employee_ID { get; set; }
        public string Name { get; set; }
        public double New { get; set; }
        public double Prod_Ci { get; set; }
        public double Prod_Co { get; set; }
        public string Prod_Grp_Code { get; set; }
        public double Renew_Ci { get; set; }
        public string Representative { get; set; }
        public string Subscr_ID { get; set; }
        public string Supervisor { get; set; }
        public string Team_ID { get; set; }     
        public double Unctrl_Loss { get; set; }
        public int? Year { get; set; }
        public int? PayPeriod { get; set; }
    }

    public enum FileType
    {
         PDF = 1,
         XLS = 2,
    }
}