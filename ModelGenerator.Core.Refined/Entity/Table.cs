using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Refined.Entity
{
    public class Table
    {
        public string? Name { get; set; }
        public string? PrimaryKey { get; set; }
        public string ConnectionProvider { get; set; }
        public string ConnectionProviderParameterType { get; set; }
        public IEnumerable<TableSchema>? Columns { get; set; }
    }
}
