using Berry.DBConsultor;
using Berry.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Berry.Models
{
    public class Audit
    {
        public AuditInfo Info { get; set; }

        public AuditAction Action { get; set; }

        public int StatusValue
        {
            get { return (int)this.Action; }
            set { this.Action = (AuditAction)value; }
        }
    }

    public class AuditInfo
    {
        public int AuditID { get; set; }
        public int SemNumero { get; set; }
        public int RepAccountID { get; set; }
        public int CanvEdition { get; set; }
        public string CanvCode { get; set; }
        public int RepSecuencia { get; set; }
        public string AuditComment { get; set; }
    }

    public enum AuditAction
    {
        Devolver = 1,
        Auditar = 2
    }

    public class AuditProcess
    {
        Security control = new Security();
         BerryDB consultor;
      
       
        public string RejectTransaction (AuditInfo transaction)
        {
            consultor = new BerryDB();
            string comment = string.Empty;

            return consultor.RejectCompareTransaction(transaction.SemNumero, transaction.RepAccountID, transaction.CanvEdition, transaction.CanvCode, transaction.RepSecuencia, control.GetWinUser(), transaction.AuditComment, transaction.AuditID);
        }

        public string AuditTransaction(AuditInfo transaction)
        {
            consultor = new BerryDB();
            string comment = string.Empty;

            return consultor.AuditCompareTransaction(transaction.SemNumero, transaction.RepAccountID, transaction.CanvEdition, transaction.CanvCode, transaction.RepSecuencia, transaction.AuditID, control.GetWinUser());
        }

    }
}