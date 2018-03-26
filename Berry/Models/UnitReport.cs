using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Berry.Models
{
    public class UnitReport
    {
        public int StaffID { get; set; }
        public int Unit { get; set; }
        public string Ejecutivo { get; set; }
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
        public double Cteact { get; set; }
        public double Cteaum { get; set; }
        public double Ctered { get; set; }
        public double Ctecan { get; set; }
        public double Nvisit { get; set; }
        public double Nvend { get; set; }
        public double Cteren { get; set; }
        public double Devo { get; set; }
        public double DiasPot { get; set; }
        public double DiasCargos { get; set; }
    }
}