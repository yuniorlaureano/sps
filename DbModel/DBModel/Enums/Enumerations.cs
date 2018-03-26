using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBModel.Enums
{
    public sealed class Connections { 
    
        private Connections(){}
            
        public const string RD = "RDConection";
        public const string PR = "PRConection";
        public const string BRRD = "BRRDConection";
        public const string BRPR = "BRPRConection";
        public const string SQLRD = "SQLRDConection";
        public const string SQLPR = "SQLPRConection";
        public const string ImagingRD = "SqlDbImaging_RD";
        
    }

}
