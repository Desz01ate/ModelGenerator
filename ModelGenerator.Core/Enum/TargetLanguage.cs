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
using System.Data.SQLite;

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
                            generator = new CSharpGenerator<SqlConnection, SqlParameter>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new CSharpGenerator<OracleConnection, OracleParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new CSharpGenerator<MySqlConnection, MySqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new CSharpGenerator<NpgsqlConnection, NpgsqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.SQLite:
                            generator = new CSharpGenerator<SQLiteConnection, SQLiteParameter>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.VisualBasic:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new VisualBasicGenerator<SqlConnection, SqlParameter>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new VisualBasicGenerator<OracleConnection, OracleParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new VisualBasicGenerator<MySqlConnection, MySqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new VisualBasicGenerator<NpgsqlConnection, NpgsqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.SQLite:
                            generator = new VisualBasicGenerator<SQLiteConnection, SQLiteParameter>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.TypeScript:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new TypeScriptGenerator<SqlConnection, SqlParameter>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new TypeScriptGenerator<OracleConnection, OracleParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new TypeScriptGenerator<MySqlConnection, MySqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new TypeScriptGenerator<NpgsqlConnection, NpgsqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.SQLite:
                            generator = new TypeScriptGenerator<SQLiteConnection, SQLiteParameter>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.PHP:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new PHPGenerator<SqlConnection, SqlParameter>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new PHPGenerator<OracleConnection, OracleParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new PHPGenerator<MySqlConnection, MySqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new PHPGenerator<NpgsqlConnection, NpgsqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.SQLite:
                            generator = new PHPGenerator<SQLiteConnection, SQLiteParameter>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.Python:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new PythonGenerator<SqlConnection, SqlParameter>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new PythonGenerator<OracleConnection, OracleParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new PythonGenerator<MySqlConnection, MySqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new PythonGenerator<NpgsqlConnection, NpgsqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.SQLite:
                            generator = new PythonGenerator<SQLiteConnection, SQLiteParameter>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.Python37:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new Python37Generator<SqlConnection, SqlParameter>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new Python37Generator<OracleConnection, OracleParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new Python37Generator<MySqlConnection, MySqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new Python37Generator<NpgsqlConnection, NpgsqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.SQLite:
                            generator = new Python37Generator<SQLiteConnection, SQLiteParameter>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.Java:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new JavaGenerator<SqlConnection, SqlParameter>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new JavaGenerator<OracleConnection, OracleParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new JavaGenerator<MySqlConnection, MySqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new JavaGenerator<NpgsqlConnection, NpgsqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.SQLite:
                            generator = new JavaGenerator<SQLiteConnection, SQLiteParameter>(connectionString, directory, @namespace);
                            break;
                    }
                    break;
                case TargetLanguage.CPP:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new CPPGenerator<SqlConnection, SqlParameter>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new CPPGenerator<OracleConnection, OracleParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new CPPGenerator<MySqlConnection, MySqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new CPPGenerator<NpgsqlConnection, NpgsqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.SQLite:
                            generator = new CPPGenerator<SQLiteConnection, SQLiteParameter>(connectionString, directory, @namespace);
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
                    strategy.SetGenerator<SqlConnection, SqlParameter>((x) => $"[{x}]");
                    break;
                case TargetDatabaseConnector.Oracle:
                    strategy.SetGenerator<OracleConnection, OracleParameter>();
                    break;
                case TargetDatabaseConnector.MySQL:
                    strategy.SetGenerator<MySqlConnection, MySqlParameter>();
                    break;
                case TargetDatabaseConnector.PostgreSQL:
                    strategy.SetGenerator<NpgsqlConnection, NpgsqlParameter>();
                    break;
                case TargetDatabaseConnector.SQLite:
                    strategy.SetGenerator<SQLiteConnection, SQLiteParameter>();
                    break;
            }
            generator.UseStrategy(strategy);
            generator.Generate();
        }
    }
}
