using Microsoft.Data.SqlClient;
using ModelGenerator.Core.Service.Generator;
using ModelGenerator.Core.TextTemplates;
using System;
using System.Collections.Generic;
using System.IO;
using Utilities.Classes;

namespace ModelGenerator.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var resources = File.ReadAllLines(@"C:\Users\TYCHE\Source\Repos\ModelGenerator.Core\ModelGenerator.Core\Resources\csharp_unitofwork.fmt");
            var genericGenerator = new GenericGenerator("server=localhost;database=local;user=sa;password=sa;", @"C:\Users\TYCHE\Desktop\fmt", "myns", resources, x => $@"[{x}]");
            genericGenerator.SetDbConnector(new SqlConnection(), new SqlParameter());
            genericGenerator.Generate();
        }
    }
}
