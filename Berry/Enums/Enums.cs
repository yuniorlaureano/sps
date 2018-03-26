using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Berry.Enums
{
    [Flags]
    public enum Roles
    {
        Administrator,
        SalesRepresentative,
        SalesSupervisor,
        SalesDirector
    }
}