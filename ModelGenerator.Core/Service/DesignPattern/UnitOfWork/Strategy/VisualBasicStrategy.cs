using ModelGenerator.Core.Services.DesignPattern.Interfaces;
using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Interfaces;
using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.Interfaces;

namespace ModelGenerator.Core.Services.DesignPattern.UnitOfWork.Strategy
{
    public class VisualBasicStrategy : IServiceGenerator
    {
        public VisualBasicStrategy(string connectionString, string directory, string @namespace)
        {
            ConnectionString = connectionString;
            Directory = directory;
            Namespace = @namespace;
        }
        public void SetGenerator<TDatabase, TParameter>(Func<string, string> parserFunction = null)
    where TDatabase : DbConnection, new()
    where TParameter : DbParameter, new()
        {
            this.ModelGenerator = new VisualBasicGenerator<TDatabase, TParameter>(this.ConnectionString, this.ModelDirectory, Namespace, parserFunction);
        }
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

        public void GenerateModel()
        {
            ModelGenerator.Generate();
        }

        public void GenerateRepository(Table table)
        {
            var tableName = TableNameCleanser(table.Name);
            var repositoryName = $"{tableName}Repository";
            var sb = new StringBuilder();
            sb.AppendLine($"Imports System.Data.SqlClient");
            sb.AppendLine($"Imports Utilities.Interfaces");
            sb.AppendLine($"Imports Utilities.DesignPattern.UnitOfWork.Components");
            sb.AppendLine($"Imports {Namespace}.Models");
            sb.AppendLine();
            sb.AppendLine($@"Namespace {Namespace}.Repositories");
            sb.AppendLine($"    public Class {repositoryName}");
            sb.AppendLine($"    Inherits Repository(Of {tableName},SqlConnection,SqlParameter)");
            sb.AppendLine($"       Public Sub New(connector As IDatabaseConnectorExtension(Of SqlConnection,SqlParameter))");
            sb.AppendLine($"           MyBase.New(connector)");
            sb.AppendLine($"       End Sub");
            sb.AppendLine("    End Class");
            sb.AppendLine("End Namespace");
            var outputFile = Path.Combine(RepositoryDirectory, $"{repositoryName}.vb");
            System.IO.File.WriteAllText(outputFile, sb.ToString(), Encoding.UTF8);
        }
        public void GeneratePartialRepository(Table table)
        {

        }
        public void GenerateService()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Imports System");
            sb.AppendLine("Imports System.Data.SqlClient");
            sb.AppendLine("Imports Utilities.SQL");
            sb.AppendLine("Imports Utilities.Interfaces");
            sb.AppendLine("Imports System.Data.Common");
            sb.AppendLine($"Imports {Namespace}.Repositories");
            sb.AppendLine();
            sb.AppendLine($@"Namespace {Namespace}");
            sb.AppendLine($"    Public NotInheritable Class Service");
            sb.AppendLine($"                                Implements IDisposable");
            //sb.AppendLine("        Private ReadOnly Shared _lazyInstant As Lazy(Of Service) = new Lazy(Of Service)(Function() new Service())");
            //sb.AppendLine("        Public ReadOnly Shared Context As Service  = _lazyInstant.Value");
            sb.AppendLine("        Private _connection As IDatabaseConnectorExtension(Of SqlConnection,SqlParameter)");
            sb.AppendLine("        Public Sub New(connectionString As String)");
            sb.AppendLine($"                _connection = new DatabaseConnector(Of SqlConnection,SqlParameter)(connectionString)");
            sb.AppendLine("        End Sub");
            foreach (var table in ModelGenerator.Tables)
            {
                var tableName = TableNameCleanser(table.Name);
                var repositoryName = $"{tableName}Repository";
                sb.AppendLine($"        Private  _{tableName} As {repositoryName}");
                sb.AppendLine($"        Public ReadOnly Property {tableName}() As {repositoryName}");
                sb.AppendLine($"            Get");
                sb.AppendLine($"                If(_{tableName} Is Nothing) Then");
                sb.AppendLine($"                    _{tableName} = new {repositoryName}(_connection)");
                sb.AppendLine($"                End If");
                sb.AppendLine($"                return _{tableName}");
                sb.AppendLine($"            End Get");
                sb.AppendLine($"        End Property");
            }
            sb.AppendLine($"            Public Sub Dispose() Implements IDisposable.Dispose");
            sb.AppendLine($"                _connection.Dispose()");
            sb.AppendLine("             End Sub");
            sb.AppendLine("    End Class");
            sb.AppendLine("End Namespace");
            var outputPath = Path.Combine(Directory, "Service.vb");
            System.IO.File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
        }
    }
}
