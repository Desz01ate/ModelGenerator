using ModelGenerator.Core.Services.DesignPattern.Interfaces;
using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Interfaces;
using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Interfaces;

namespace ModelGenerator.Core.Services.DesignPattern.UnitOfWork.Strategy
{
    public class CSharpStrategy : IServiceGenerator
    {
        public CSharpStrategy(string connectionString, string directory, string @namespace)
        {
            ConnectionString = connectionString;
            Directory = directory;
            Namespace = @namespace;
            System.IO.Directory.CreateDirectory(PartialRepositoryDirectory);
            //Generator = new CSharpGenerator<TDatabase>(connectionString, ModelDirectory, $"{@namespace}.Models");
            //if (func != null) Generator.SetCleanser(func);
        }
        public void SetGenerator<TDatabase>(Func<string, string> parserFunction = null) where TDatabase : DbConnection, new()
        {
            this.ModelGenerator = new CSharpGenerator<TDatabase>(this.ConnectionString, this.ModelDirectory, this.PartialModelDirectory, $"{Namespace}.Models", parserFunction);
        }
        public string Directory { get; }

        public string Namespace { get; }

        public string ConnectionString { get; }

        public string ModelDirectory => Path.Combine(Directory, "Models");
        public string PartialModelDirectory => Path.Combine(ModelDirectory, "Partials");
        public string RepositoryDirectory => Path.Combine(Directory, "Repositories");
        public string PartialRepositoryDirectory => Path.Combine(RepositoryDirectory, "Partials");
        public string RepositoryComponentsDirectory => Path.Combine(RepositoryDirectory, "Components");

        public IModelGenerator ModelGenerator { get; private set; }
        private string TableNameCleanser(string tableName)
        {
            return tableName.Replace("-", "");
        }

        public void GenerateModel()
        {
            ModelGenerator.Generate();
        }

        public void GenerateRepository(Table table)
        {
            var tableName = TableNameCleanser(table.Name);
            var repositoryName = $"{tableName}Repository";
            var sb = new StringBuilder();
            sb.AppendLine($"using System.Data.SqlClient;");
            sb.AppendLine($"using Utilities.Interfaces;");
            sb.AppendLine($"using {Namespace}.Repositories.Components;");
            sb.AppendLine($"using {Namespace}.Models;");
            sb.AppendLine();
            sb.AppendLine($@"namespace {Namespace}.Repositories");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Data contractor for {tableName}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public partial class {repositoryName} : Repository<{tableName},SqlConnection,SqlParameter>");
            sb.AppendLine("    {");
            sb.AppendLine($"       public {repositoryName}(IDatabaseConnectorExtension<SqlConnection,SqlParameter> connector) : base(connector)");
            sb.AppendLine($"       {{");
            sb.AppendLine($"       }}");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            var outputFile = Path.Combine(RepositoryDirectory, $"{repositoryName}.cs");
            System.IO.File.WriteAllText(outputFile, sb.ToString(), Encoding.UTF8);
        }
        public void GeneratePartialRepository(Table table)
        {
            var tableName = TableNameCleanser(table.Name);
            var repositoryName = $"{tableName}Repository";
            var sb = new StringBuilder();
            sb.AppendLine($"using System.Data.SqlClient;");
            sb.AppendLine($"using Utilities.Interfaces;");
            sb.AppendLine($"using {Namespace}.Repositories.Components;");
            sb.AppendLine($"using {Namespace}.Models;");
            sb.AppendLine();
            sb.AppendLine($@"namespace {Namespace}.Repositories");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Data contractor for {tableName}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public partial class {repositoryName} : Repository<{tableName},SqlConnection,SqlParameter>");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            var outputFile = Path.Combine(PartialRepositoryDirectory, $"{repositoryName}.cs");
            if (!File.Exists(outputFile))
                System.IO.File.WriteAllText(outputFile, sb.ToString(), Encoding.UTF8);
        }
        public void GenerateService()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using System.Transactions;");
            sb.AppendLine("using Utilities.SQL;");
            sb.AppendLine("using Utilities.Interfaces;");
            sb.AppendLine("using System.Data.Common;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine($"using System.Data;");
            sb.AppendLine($"using {Namespace}.Repositories;");
            sb.AppendLine();
            sb.AppendLine($@"namespace {Namespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public sealed class Service : IDisposable");
            sb.AppendLine("    {");
            //sb.AppendLine("        private readonly static Lazy<Service> _lazyInstant = new Lazy<Service>(()=> new Service(),true);");
            //sb.AppendLine("        public readonly static Service Context = _lazyInstant.Value;");
            sb.AppendLine("        public readonly IDatabaseConnectorExtension<SqlConnection,SqlParameter> Connector;");
            sb.AppendLine("        public Service(string connectionString)");
            sb.AppendLine("        {");
            sb.AppendLine($"                Connector = new DatabaseConnector<SqlConnection,SqlParameter>(connectionString);");
            sb.AppendLine("        }");
            foreach (var table in ModelGenerator.Tables)
            {
                var tableName = TableNameCleanser(table.Name);
                var repositoryName = $"{tableName}Repository";
                sb.AppendLine($"        private {repositoryName} _{tableName} {{ get; set; }}");
                sb.AppendLine("         /// <summary>");
                sb.AppendLine($"        /// Data repository for {tableName} table");
                sb.AppendLine("         /// </summary>");
                sb.AppendLine($"        public {repositoryName} {tableName}");
                sb.AppendLine($"        {{");
                sb.AppendLine($"            get");
                sb.AppendLine($"            {{");
                sb.AppendLine($"                if(_{tableName} == null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    _{tableName} = new {repositoryName}(Connector);");
                sb.AppendLine($"                }}");
                sb.AppendLine($"                return _{tableName};");
                sb.AppendLine($"            }}");
                sb.AppendLine($"        }}");
            }
            sb.AppendLine($"            public void Dispose()");
            sb.AppendLine("            {");
            sb.AppendLine($"                Connector?.Dispose();");
            sb.AppendLine("            }");
            sb.AppendLine("#region Stored Procedure");
            foreach (var sp in ModelGenerator.StoredProcedures)
            {
                if (sp.SPECIFIC_NAME.StartsWith("sp") && sp.SPECIFIC_NAME.Contains("diagram"))
                {
                    sb.AppendLine("//This method might be a system stored procedure, maybe you might want to consider remove it.");
                }
                sb.Append($"            public dynamic {sp.SPECIFIC_NAME}(");
                List<string> paramArgs = new List<string>();
                List<string> paramFunc = new List<string>();
                foreach (var param in sp.Parameters)
                {
                    paramArgs.Add($"{param.DATA_TYPE} {param.PARAMETER_NAME}");
                    paramFunc.Add($"                     parameters.Add(new SqlParameter(\"{param.PARAMETER_NAME}\",{param.PARAMETER_NAME}));");
                }
                sb.Append(string.Join(",", paramArgs));
                sb.AppendLine(")");
                sb.AppendLine("            {");
                sb.AppendLine($"                     var command = \"{sp.SPECIFIC_NAME}\";");
                sb.AppendLine($"                     var parameters = new List<SqlParameter>();");
                foreach (var p in paramFunc)
                {
                    sb.AppendLine(p);
                }
                sb.AppendLine("                      var result = Connector.ExecuteReader(command,parameters, commandType: System.Data.CommandType.StoredProcedure);");
                sb.AppendLine("                      return result;");
                sb.AppendLine("            }");
            }
            sb.AppendLine("#endregion");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            var outputPath = Path.Combine(Directory, "Service.cs");
            System.IO.File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
        }
    }
}
