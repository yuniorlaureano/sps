using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Berry.Models
{
    [DataContract]
    public class CommisionHistTrans
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Subscr_ID { get; set; }
        [DataMember]
        public string Prod_Grp_Code { get; set; }
        [DataMember]
        public string Canv_Code { get; set; }

        [DataMember]
        public string Book_Code { get; set; }

        [DataMember]
        public string Canv_Edition { get; set; }
        [DataMember]
        public int Canv_Week { get; set; }
        [DataMember]
        public int Assigment_No { get; set; }
        [DataMember]
        public int Employee_id { get; set; }
        [DataMember]
        public string Employee_Name { get; set; }
        [DataMember]
        public double Prod_Co { get; set; }
        [DataMember]
        public double Prod_Ci { get; set; }
        [DataMember]
        public double Renew_Ci { get; set; }
        [DataMember]
        public double Increase_Ci { get; set; }
        [DataMember]
        public double Decrease_Ci { get; set; }
        [DataMember]
        public double Np_Ci { get; set; }
        [DataMember]
        public double Op_Ci { get; set; }
        [DataMember]
        public double Otc_Ci { get; set; }
        [DataMember]
        public double Ctrl_Loss { get; set; }
        [DataMember]
        public double Unctrl_Loss { get; set; }
        [DataMember]
        public string DB { get; set; }
    }
}