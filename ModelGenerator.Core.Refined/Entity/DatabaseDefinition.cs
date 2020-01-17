using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Refined.Entity
{
    public class DatabaseDefinition
    {
        public string ConnectionString { get; set; }
        public Type DatabaseProvider { get; set; }
        public Type DatabaseProviderParameterType { get; set; }
        public List<Table> Tables { get; set; }
        public List<StoredProcedureSchema> StoredProcedures { get; set; }
    }
}
