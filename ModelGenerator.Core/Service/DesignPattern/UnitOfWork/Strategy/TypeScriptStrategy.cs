using ModelGenerator.Core.Services.DesignPattern.Interfaces;
using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Interfaces;
using ModelGenerator.Core.Services.Generator.Model;
using ModelGenerator.Core.TextTemplates;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelGenerator.Core.Service.DesignPattern.UnitOfWork.Strategy
{
    public class TypeScriptStrategy : IServiceGenerator
    {
        public string Directory { get; }

        public string Namespace { get; }

        public string ConnectionString { get; }

        public string ModelDirectory => Path.Combine(Directory, "Models");

        public string RepositoryDirectory => Path.Combine(Directory, "Repositories");

        public string RepositoryComponentsDirectory => Path.Combine(RepositoryDirectory, "Components");

        public IModelGenerator ModelGenerator { get; private set; }
        private string TableNameCleanser(string tableName)
        {
            return Regex.Replace(tableName, @"(\s|\$|-)", "");
        }

        public TypeScriptStrategy(string connectionString, string directory, string @namespace)
        {
            ConnectionString = connectionString;
            Directory = directory;
            Namespace = @namespace;
        }

        public void GenerateModel()
        {
            ModelGenerator.Generate();
        }

        public void GeneratePartialRepository(Table table)
        {

        }

        public void GenerateRepository(Table table)
        {
            var tableName = TableNameCleanser(table.Name);
            var repositoryName = $"{tableName}Repository";
            var sb = new StringBuilder();
            sb.AppendLine(@"import { Repository } from './Components/Repository';");
            sb.AppendLine($"import {{ I{tableName} }} from '../Models/I{table.Name}';");
            sb.AppendLine($"export default class {repositoryName} extends Repository<I{tableName}>");
            sb.AppendLine(@"{");
            sb.AppendLine(@"}");
            var outputFile = Path.Combine(RepositoryDirectory, $"{repositoryName}.ts");
            System.IO.File.WriteAllText(outputFile, sb.ToString(), Encoding.UTF8);
        }

        public void GenerateService()
        {
            var sb = new StringBuilder();
            foreach (var table in ModelGenerator.Tables)
            {
                var tableName = TableNameCleanser(table.Name);
                var repositoryName = $"{tableName}Repository";
                sb.AppendLine($"import {repositoryName} from \"./Repositories/{repositoryName}\";");
            }
            sb.AppendLine($"export default class Service");
            sb.AppendLine(@"{");
            sb.AppendLine(@"        static Context : Service = new Service();");
            sb.AppendLine(@"        private constructor()");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"        }");
            foreach (var table in ModelGenerator.Tables)
            {
                var tableName = TableNameCleanser(table.Name);
                var repositoryName = $"{tableName}Repository";
                sb.AppendLine($"        readonly {tableName} : {repositoryName} = new {repositoryName}();");
            }
            sb.AppendLine(@"}");
            var outputPath = Path.Combine(Directory, "Service.ts");
            System.IO.File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
        }

        public void SetGenerator<TDatabase, TParameter>(Func<string, string> parserFunction = null)
            where TDatabase : DbConnection, new()
            where TParameter : DbParameter, new()
        {
            ModelGenerator = new TypeScriptGenerator<TDatabase, TParameter>(this.ConnectionString, this.ModelDirectory, "", parserFunction);
        }

        public void GenerateRepositoryDependencies()
        {
            var repositoryFile = System.IO.Path.Combine(this.RepositoryComponentsDirectory, "Repository.ts");
            var repositoryTemplate = new TS_RepositoryTemplate();
            repositoryTemplate.Namespace = this.Namespace;
            var content = repositoryTemplate.TransformText();
            System.IO.File.WriteAllText(repositoryFile, content);
        }
    }
}
