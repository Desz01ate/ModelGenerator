using ModelGenerator.Core.Services.DesignPattern.Interfaces;
using ModelGenerator.Core.Services.DesignPattern.UnitOfWork.Generator;
using ModelGenerator.Core.Services.DesignPattern.UnitOfWork.Strategy;
using ModelGenerator.Core.Services.Generator;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Data.SqlClient;

namespace ModelGenerator.Core.Enum
{
    public enum TargetLanguage
    {
        [Description("C#")]
        CSharp = 0,
        [Description("Visual Basic")]
        VisualBasic = 1,
        [Description("TypeScript")]
        TypeScript = 2,
        [Description("PHP")]
        PHP = 3,
        [Description("Python")]
        Python = 4,
        [Description("Python 3.7")]
        Python37 = 5,
        [Description("Java")]
        Java = 6,
        [Description("C++")]
        CPP = 7
    }
    public static class LangaugesData
    {
        public static void PerformModelGenerate(TargetLanguage targetLanguage, TargetDatabaseConnector targetDatabaseConnector, string connectionString, string directory, string @namespace)
        {
            switch (targetLanguage)
            {
                case TargetLanguage.CSharp:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            new CSharpGenerator<SqlConnection>(connectionString, directory, @namespace, (x) => $"[{x}]").GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.Oracle:
                            new CSharpGenerator<OracleConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.MySQL:
                            new CSharpGenerator<MySqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.PostgreSQL:
                            new CSharpGenerator<NpgsqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                    }
                    break;
                case TargetLanguage.VisualBasic:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            new VisualBasicGenerator<SqlConnection>(connectionString, directory, @namespace, (x) => $"[{x}]").GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.Oracle:
                            new VisualBasicGenerator<OracleConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.MySQL:
                            new VisualBasicGenerator<MySqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.PostgreSQL:
                            new VisualBasicGenerator<NpgsqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                    }
                    break;
                case TargetLanguage.TypeScript:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            new TypeScriptGenerator<SqlConnection>(connectionString, directory, @namespace, (x) => $"[{x}]").GenerateAllTable();

                            return;
                        case TargetDatabaseConnector.Oracle:
                            new TypeScriptGenerator<OracleConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.MySQL:
                            new TypeScriptGenerator<MySqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.PostgreSQL:
                            new TypeScriptGenerator<NpgsqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                    }
                    break;
                case TargetLanguage.PHP:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            new PHPGenerator<SqlConnection>(connectionString, directory, @namespace, (x) => $"[{x}]").GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.Oracle:
                            new PHPGenerator<OracleConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.MySQL:
                            new PHPGenerator<MySqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.PostgreSQL:
                            new PHPGenerator<NpgsqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                    }
                    break;
                case TargetLanguage.Python:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            new PythonGenerator<SqlConnection>(connectionString, directory, @namespace, (x) => $"[{x}]").GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.Oracle:
                            new PythonGenerator<OracleConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.MySQL:
                            new PythonGenerator<MySqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.PostgreSQL:
                            new PythonGenerator<NpgsqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                    }
                    break;
                case TargetLanguage.Python37:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            new Python37Generator<SqlConnection>(connectionString, directory, @namespace, (x) => $"[{x}]").GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.Oracle:
                            new Python37Generator<OracleConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.MySQL:
                            new Python37Generator<MySqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.PostgreSQL:
                            new Python37Generator<NpgsqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                    }
                    break;
                case TargetLanguage.Java:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            new JavaGenerator<SqlConnection>(connectionString, directory, @namespace, (x) => $"[{x}]").GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.Oracle:
                            new JavaGenerator<OracleConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.MySQL:
                            new JavaGenerator<MySqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.PostgreSQL:
                            new JavaGenerator<NpgsqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                    }
                    break;
                case TargetLanguage.CPP:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            new CPPGenerator<SqlConnection>(connectionString, directory, @namespace, (x) => $"[{x}]").GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.Oracle:
                            new CPPGenerator<OracleConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.MySQL:
                            new CPPGenerator<MySqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                        case TargetDatabaseConnector.PostgreSQL:
                            new CPPGenerator<NpgsqlConnection>(connectionString, directory, @namespace).GenerateAllTable();
                            return;
                    }
                    break;
            }
        }
        public static void PerformStrategyGenerate(TargetLanguage targetLanguage, TargetDatabaseConnector targetDatabaseConnector, string connectionString, string directory, string @namespace)
        {
            IGeneratorStrategy strategy = default;
            var generator = new UnitOfWorkGenerator();
            switch (targetLanguage)
            {
                case TargetLanguage.CSharp:
                    strategy = new CSharpStrategy(connectionString, directory, @namespace);
                    break;
                case TargetLanguage.VisualBasic:
                    strategy = new VisualBasicStrategy(connectionString, directory, @namespace);
                    break;
            }
            switch (targetDatabaseConnector)
            {
                case TargetDatabaseConnector.SQLServer:
                    strategy.SetGenerator<SqlConnection>((x) => $"[{x}]");
                    break;
                case TargetDatabaseConnector.Oracle:
                    strategy.SetGenerator<OracleConnection>((x) => $"[{x}]");
                    break;
                case TargetDatabaseConnector.MySQL:
                    strategy.SetGenerator<MySqlConnection>((x) => $"[{x}]");
                    break;
                case TargetDatabaseConnector.PostgreSQL:
                    strategy.SetGenerator<NpgsqlConnection>((x) => $"[{x}]");
                    break;
            }
            generator.UseStrategy(strategy);
            generator.Generate();
        }
    }
}
