using Microsoft.Data.SqlClient;
using ModelGenerator.Core.Refined.Builder;
using ModelGenerator.Core.Refined.Entity.ModelProvider;

namespace ModelGenerator.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var tableDef = ModelGenerator.Core.Refined.Helper.SqlDefinition.GetTablesDefinition<SqlConnection, SqlParameter>("server=localhost;database=local;user=sa;password=sa;");
            var modelBuilder = new ModelBuilder(@"C:\Users\TYCHE\Desktop\fmt", "test", tableDef);
            modelBuilder.Generate(new CsharpModelProvider());
        }
    }
}
