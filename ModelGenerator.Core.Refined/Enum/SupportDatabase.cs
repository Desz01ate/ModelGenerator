using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ModelGenerator.Core.Refined.Enum
{
    public enum SupportDatabase
    {
        [Description("Microsoft SQL Server")]
        SQLServer,
        Oracle,
        MySQL,
        PostgreSQL
    }
}
