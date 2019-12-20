using System.ComponentModel;

namespace ModelGenerator.Core.Enum
{
    public enum TargetDatabaseConnector
    {
        [Description("SQL Server")]
        SQLServer,
        Oracle,
        MySQL,
        PostgreSQL,
        SQLite
    }
}
