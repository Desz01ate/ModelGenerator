using Microsoft.Data.SqlClient;
using ModelGenerator.Core.Builder;
using ModelGenerator.Core.Helper;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbDef = SqlDefinition.GetDatabaseDefinition<SqlConnection, SqlParameter>("server=localhost;database=local;user=sa;password=sa;", x => $"[{x}]");

            var sgen = new ModelBuilder(@"C:\Users\TYCHE\Desktop\fmt", "test", dbDef);
            //Console.ReadLine();
        }
    }
}
