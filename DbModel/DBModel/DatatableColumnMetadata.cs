using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBModel
{
    public enum DtColumnType
    {
        Cadena,
        Numerico
    }
    public class DatatableColumnMetadata
    {
        public string ColumnName { get; set; }
        public DtColumnType DtColmnType { get; set; }
    }
}
