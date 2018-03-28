using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBModel
{
    public class DatatableIEnumerableMetadata
    {
        List<DatatableColumnMetadata> ColumnMetadata { get; set; } 
        List<string> ColumnHeader { get; set; } //Serve with purpose of representing a header
    }
}
