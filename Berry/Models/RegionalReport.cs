using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Berry.Models
{
    public class RegionalReport
    {
        public class RegionSection
        {
            public string Type { get; set; }
            public double Lpitem { get; set; }
            public double Lpprice { get; set; }
            public double Lnuevo { get; set; }
            public double Laumento { get; set; }
            public double Lreduc { get; set; }
            public double Lcanc { get; set; }
            public double Lnitem { get; set; }
            public double Lneto { get; set; }
            public double Cuota { get; set; }
            public double Dventas { get; set; }
            public double DiasPot { get; set; }
            public double DiasCargos { get; set; }
            public double Cteact { get; set; }
            public double Cteaum { get; set; }
            public double Ctecan { get; set; }
            public double Nvisit { get; set; }
            public double Nvend { get; set; }
            public double Cteren { get; set; }
           
        }

        public class OfficeDetail
        {
            public string Type { get; set; }
            public double Accounts { get; set; }
            public double Incr_OB { get; set; }
            public double Decr_OB { get; set; }
            public double Loss_OB { get; set; }
            public double PI_OB { get; set; }
            public double Incr_OD { get; set; }
            public double Decr_OD { get; set; }
            public double Loss_OD { get; set; }
            public double NI_OD { get; set; }
            public double Visits { get; set; }    
          
        }

        public class VisitsDetail
        {
            public string PublID { get; set; }
            public double VisitsTotalPot { get; set; }
            public double VisitsTotalAmount { get; set; }
            public double VisitsPotWeek { get; set; }
            public double VisitsPotAccum { get; set; }
            public double VisitsAmountAccum { get; set; }
            public double VisitsPendPot { get; set; }
        }
    }
}