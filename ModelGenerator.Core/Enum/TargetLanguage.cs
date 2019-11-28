using ModelGenerator.Core.Services.DesignPattern.Interfaces;
using ModelGenerator.Core.Services.DesignPattern.UnitOfWork.Generator;
using ModelGenerator.Core.Services.DesignPattern.UnitOfWork.Strategy;
using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Interfaces;
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
    public static class LanguagesData
    {
        public static void PerformModelGenerate(TargetLanguage targetLanguage, TargetDatabaseConnector targetDatabaseConnector, string connectionString, string directory, string @namespace)
        {
            IModelGenerator generator = default;
            switch (targetLanguage)
            {
                case TargetLanguage.CSharp:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new CSharpGenerator<SqlConnection>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new CSharpGenerator<OracleConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new CSharpGenerator<MySqlConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new CSharpGenerator<NpgsqlConnection>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.VisualBasic:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new VisualBasicGenerator<SqlConnection>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new VisualBasicGenerator<OracleConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new VisualBasicGenerator<MySqlConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new VisualBasicGenerator<NpgsqlConnection>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.TypeScript:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new TypeScriptGenerator<SqlConnection>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new TypeScriptGenerator<OracleConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new TypeScriptGenerator<MySqlConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new TypeScriptGenerator<NpgsqlConnection>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.PHP:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new PHPGenerator<SqlConnection>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new PHPGenerator<OracleConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new PHPGenerator<MySqlConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new PHPGenerator<NpgsqlConnection>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.Python:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new PythonGenerator<SqlConnection>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new PythonGenerator<OracleConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new PythonGenerator<MySqlConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new PythonGenerator<NpgsqlConnection>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.Python37:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new Python37Generator<SqlConnection>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new Python37Generator<OracleConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new Python37Generator<MySqlConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new Python37Generator<NpgsqlConnection>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.Java:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new JavaGenerator<SqlConnection>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new JavaGenerator<OracleConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new JavaGenerator<MySqlConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new JavaGenerator<NpgsqlConnection>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.CPP:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new CPPGenerator<SqlConnection>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new CPPGenerator<OracleConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new CPPGenerator<MySqlConnection>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new CPPGenerator<NpgsqlConnection>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
            }
            generator.Generate();
        }
        public static void PerformRepositoryGenerate(TargetLanguage targetLanguage, TargetDatabaseConnector targetDatabaseConnector, string connectionString, string directory, string @namespace)
        {
            IServiceGenerator strategy = default;
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
                    strategy.SetGenerator<OracleConnection>();
                    break;
                case TargetDatabaseConnector.MySQL:
                    strategy.SetGenerator<MySqlConnection>();
                    break;
                case TargetDatabaseConnector.PostgreSQL:
                    strategy.SetGenerator<NpgsqlConnection>();
                    break;
            }
            generator.UseStrategy(strategy);
            generator.Generate();
        }
    }
}
