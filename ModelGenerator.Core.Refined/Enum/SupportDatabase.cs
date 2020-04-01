using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ModelGenerator.Core.Enum
{
    public enum SupportDatabase
    {
        [Description("Microsoft SQL Server")]
        SQLServer,
        Oracle,
        MySQL,
        PostgreSQL,
        SQLite
    }
}
