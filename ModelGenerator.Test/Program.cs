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
            var model = File.ReadAllLines(@"C:\Users\TYCHE\Source\Repos\ModelGenerator.Core\ModelGenerator.Core\Resources\csharp.fmt");
            var repo = File.ReadAllLines(@"C:\Users\TYCHE\Source\Repos\ModelGenerator.Core\ModelGenerator.Core\Resources\csharp_repo.fmt");
            var uow = File.ReadAllLines(@"C:\Users\TYCHE\Source\Repos\ModelGenerator.Core\ModelGenerator.Core\Resources\csharp_unitofwork.fmt");
            var repoBase = File.ReadAllLines(@"C:\Users\TYCHE\Source\Repos\ModelGenerator.Core\ModelGenerator.Core\Resources\csharp_repo_based.fmt");
            var baseDir = $@"C:\Users\TYCHE\Documents\Visual Studio 2015\Projects\Playground\Local";

            var genericGenerator = new GenericGenerator("server=localhost;database=local;user=sa;password=sa;", $@"{baseDir}\Models", "Local", model, x => $@"[{x}]");
            genericGenerator.SetDbConnector(new SqlConnection(), new SqlParameter());
            genericGenerator.Generate();
            genericGenerator.ChangeDirectory($@"{baseDir}\Repositories");
            genericGenerator.ReloadResource(repo);
            genericGenerator.Generate();
            genericGenerator.ChangeDirectory($@"{baseDir}");
            genericGenerator.ReloadResource(uow);
            genericGenerator.Generate();
            genericGenerator.ChangeDirectory($@"{baseDir}\Repositories\Components");
            genericGenerator.ReloadResource(repoBase);
            genericGenerator.Generate();
        }
    }
}
