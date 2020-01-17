using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGenerator.Core.Entity
{
    public class StoredProcedureSchemaExplain
    {
        public string ColumnName { get; set; }
        public int ColumnOrdinal { get; set; }
        public int ColumnSize { get; set; }
        public int NumericPrecision { get; set; }
        public int NumericScale { get; set; }
        public bool IsUnique { get; set; }
    }
}
