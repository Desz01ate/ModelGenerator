using Microsoft.Data.SqlClient;
using ModelGenerator.Core.Refined.Builder;
using ModelGenerator.Core.Refined.Entity.ModelProvider;
using ModelGenerator.Core.Refined.Entity.ServiceProvider;
using ModelGenerator.Core.Refined.Helper;
using System;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var tableDef = SqlDefinition.GetTablesDefinition<SqlConnection, SqlParameter>("server=localhost;database=local;user=sa;password=sa;", x => $"[{x}]");
            var spDef = SqlDefinition.GetStoredProcedureSchemasDefinition<SqlConnection, SqlParameter>("server=localhost;database=local;user=sa;password=sa;");
            //var gen = new ModelBuilder(@"C:\Users\TYCHE\Desktop\fmt", "test", tableDef);
            //gen.Generate(new CSharpModelProvider());
            //gen.Generate(new TypeScriptModelProvider());
            var sgen = new ServiceBuilder(@"C:\Users\TYCHE\Desktop\fmt", "test", tableDef, spDef);
            sgen.Generate(new CSharpServiceProvider());
        }
    }
}
