using ModelGenerator.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.Classes;
using Utilities.Enum;
using Utilities.Shared;
using Utilities.SQL.Extension;

namespace ModelGenerator.Core.Helper
{
    public static class SqlDefinition
    {
        /// <summary>
        /// Designed for SQL Server, might not work with other DB Provider.
        /// </summary>
        /// <typeparam name="TDatabase"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DatabaseDefinition GetDatabaseDefinition<TDatabase, TParameter>(string connectionString, Func<string, string> tableNameTransformer = null)
            where TDatabase : DbConnection, new()
            where TParameter : DbParameter, new()
        {

            using var connection = new TDatabase()
            {
                ConnectionString = connectionString
            };
            connection.Open();
            var result = new DatabaseDefinition();
            result.Tables = new List<Table>();
            result.ConnectionString = connectionString;
            result.DatabaseProvider = typeof(TDatabase);
            result.DatabaseProviderParameterType = typeof(TParameter);
            result.StoredProcedures = connection.GetStoredProcedures().ToList();
            using var tables = connection.GetSchema(SchemaRestriction.Tables);
            foreach (DataRow row in tables.Rows)
            {
                var database = row[0].ToString();
                var schema = row[1].ToString();
                var tableName = row[2].ToString();
                var type = row[3].ToString();

                var schemaSearchName = string.Empty;
                if (tableNameTransformer != null) schemaSearchName = tableNameTransformer(tableName);
                var columns = connection.GetTableSchema(schemaSearchName);
                string? primaryKey = null;
                using var indexes = connection.GetSchema("IndexColumns", new[] { null, null, tableName });
                if (indexes != null)
                {
                    foreach (DataRow rowInfo in indexes.Rows)
                    {
                        primaryKey = rowInfo["column_name"].ToString();
                    }
                }
                var table = new Table()
                {
                    Name = tableName,
                    Columns = columns,
                    PrimaryKey = primaryKey,
                    ConnectionProvider = typeof(TDatabase).FullName,
                    ConnectionProviderParameterType = typeof(TParameter).FullName
                };
                result.Tables.Add(table);
            }
            return result;
        }
        public static Table? GetStoredProcedureDefinition<TDatabase, TParameter>(string connectionString, StoredProcedureSchema storedProcedure, Func<string, string> tableNameTransformer = null)
            where TDatabase : DbConnection, new()
            where TParameter : DbParameter, new()
        {

            try
            {
                using var con = new TDatabase()
                {
                    ConnectionString = connectionString
                };
                con.Open();
                var query = storedProcedure.SPECIFIC_NAME;
                var command = con.CreateCommand();
                command.CommandText = query;
                command.CommandType = CommandType.StoredProcedure;
                foreach (var param in storedProcedure.Parameters)
                {
                    var paramType = SQLServerMap(param.DATA_TYPE);
                    var defaultValue = GetDefault(paramType);
                    command.Parameters.Add(new TParameter()
                    {
                        ParameterName = param.PARAMETER_NAME,
                        Value = defaultValue
                    });
                }
                using var reader = command.ExecuteReader(behavior: CommandBehavior.SchemaOnly);
                var schema = reader.GetSchemaTable();
                var result = schema.ToEnumerable<TableSchema>().ToList();
                var table = new Table()
                {
                    ConnectionProvider = typeof(TDatabase).FullName,
                    ConnectionProviderParameterType = typeof(TParameter).FullName,
                    PrimaryKey = null,
                    Columns = result,
                    Name = storedProcedure.SPECIFIC_NAME
                };
                return table;
            }
            catch
            {
                return null;
            }
        }
        private static Type SQLServerMap(string type)
        {
            type = type.ToLower();
            switch (type)
            {
                case "bit":
                    return typeof(bool);

                case "tinyint":
                    return typeof(byte);
                case "smallint":
                    return typeof(short);
                case "int":
                case "integer":
                    return typeof(int);
                case "bigint":
                    return typeof(long);

                case "real":
                    return typeof(float);
                case "float":
                    return typeof(double);
                case "decimal":
                case "money":
                case "smallmoney":
                case "numeric":
                    return typeof(decimal);

                case "time":
                    return typeof(TimeSpan);
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return typeof(DateTime);
                case "datetimeoffset":
                    return typeof(DateTimeOffset);

                case "char":
                case "varchar":
                case "nchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "xml":
                    return typeof(string);

                case "binary":
                case "image":
                case "varbinary":
                case "timestamp":
                    return typeof(byte[]);

                case "uniqueidentifier":
                    return typeof(Guid);

                case "variant":
                //case "Udt":
                case "udt":
                case "blob": //for sqlite
                    return typeof(object);

                //case "Structured":
                case "structured":

                default:
                    // Fallback to be manually handled by user
                    return typeof(object);
            };
        }
        private static object? GetDefault(Type type)
        {
            if (type.IsValueType) return Activator.CreateInstance(type);
            return null;
        }
    }
}

