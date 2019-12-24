using ModelGenerator.Core.Enum;
using ModelGenerator.Core.Service.DesignPattern.UnitOfWork.Strategy;
using ModelGenerator.Core.Service.Generator;
using ModelGenerator.Core.Services.DesignPattern.Interfaces;
using ModelGenerator.Core.Services.DesignPattern.UnitOfWork.Generator;
using ModelGenerator.Core.Services.DesignPattern.UnitOfWork.Strategy;
using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Interfaces;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace ModelGenerator.Core.Factory
{
    public static class _OldGeneratorFactory
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
                case TargetLanguage.Golang:
                    switch (targetDatabaseConnector)
                    {
                        case TargetDatabaseConnector.SQLServer:
                            generator = new GolangGenerator<SqlConnection, SqlParameter>(connectionString, directory, @namespace, x => $"[{x}]");
                            break;
                        case TargetDatabaseConnector.Oracle:
                            generator = new GolangGenerator<OracleConnection, OracleParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.MySQL:
                            generator = new GolangGenerator<MySqlConnection, MySqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.PostgreSQL:
                            generator = new GolangGenerator<NpgsqlConnection, NpgsqlParameter>(connectionString, directory, @namespace);
                            break;
                        case TargetDatabaseConnector.SQLite:
                            generator = new GolangGenerator<SQLiteConnection, SQLiteParameter>(connectionString, directory, @namespace);
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
                case TargetLanguage.TypeScript:
                    strategy = new TypeScriptStrategy(connectionString, directory, @namespace);
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
    public static class GeneratorFactory
    {
        public static void PerformModelGenerate(TargetLanguage targetLanguage, TargetDatabaseConnector targetDatabaseConnector, string connectionString, string directory, string @namespace)
        {
            string[] resources = null;
            switch (targetLanguage)
            {
                case TargetLanguage.CSharp:
                    resources = File.ReadAllLines(@".\Resources\csharp.fmt");
                    break;
                case TargetLanguage.VisualBasic:
                    resources = File.ReadAllLines(@".\Resources\vb.fmt");
                    break;
                case TargetLanguage.TypeScript:
                    resources = File.ReadAllLines(@".\Resources\typescript.fmt");
                    break;
                case TargetLanguage.PHP:
                    resources = File.ReadAllLines(@".\Resources\php.fmt");
                    break;
                case TargetLanguage.Python:
                    resources = File.ReadAllLines(@".\Resources\python.fmt");
                    break;
                case TargetLanguage.Python37:
                    resources = File.ReadAllLines(@".\Resources\python.fmt");
                    break;
                case TargetLanguage.Java:
                    resources = File.ReadAllLines(@".\Resources\java.fmt");
                    break;
                case TargetLanguage.CPP:
                    resources = File.ReadAllLines(@".\Resources\cpp.fmt");
                    break;
                case TargetLanguage.Golang:
                    resources = File.ReadAllLines(@".\Resources\go.fmt");
                    break;
            }
            PerformModelGenerate(resources, targetDatabaseConnector, connectionString, directory, @namespace);
        }
        public static void PerformModelGenerate(string[] resources, TargetDatabaseConnector targetDatabaseConnector, string connectionString, string directory, string @namespace)
        {
            DbConnection connector = null;
            DbParameter parameter = null;
            Func<string, string> cleanser = null;
            switch (targetDatabaseConnector)
            {
                case TargetDatabaseConnector.SQLServer:
                    connector = new SqlConnection();
                    parameter = new SqlParameter();
                    cleanser = (x) => $"[{x}]";
                    break;
                case TargetDatabaseConnector.Oracle:
                    connector = new OracleConnection();
                    parameter = new OracleParameter();
                    break;
                case TargetDatabaseConnector.MySQL:
                    connector = new MySqlConnection();
                    parameter = new MySqlParameter();
                    break;
                case TargetDatabaseConnector.PostgreSQL:
                    connector = new NpgsqlConnection();
                    parameter = new NpgsqlParameter();
                    break;
                case TargetDatabaseConnector.SQLite:
                    connector = new SQLiteConnection();
                    parameter = new SQLiteParameter();
                    break;
            }
            if (resources == null) throw new ArgumentNullException(nameof(resources));
            if (connector == null) throw new ArgumentNullException(nameof(connector));
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));
            var generator = new GenericGenerator(connectionString, directory, @namespace, resources, cleanser);
            generator.SetDbConnector(connector, parameter);
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
                case TargetLanguage.TypeScript:
                    strategy = new TypeScriptStrategy(connectionString, directory, @namespace);
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
