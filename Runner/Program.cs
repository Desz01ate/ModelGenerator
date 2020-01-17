using Microsoft.Data.SqlClient;
using ModelGenerator.Core.Refined.Builder;
using ModelGenerator.Core.Refined.Entity.ModelProvider;
using ModelGenerator.Core.Refined.Entity.ServiceProvider;
using ModelGenerator.Core.Refined.Helper;
using System;
using System.Data;
using System.Linq;

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
