using ModelGenerator.Core.Refined.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Utilities.Classes;
using Utilities.Enum;
using Utilities.SQL.Extension;

namespace ModelGenerator.Core.Refined.Helper
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
        public static List<Table> GetTablesDefinition<TDatabase, TParameter>(string connectionString, Func<string, string> tableNameTransformer = null)
            where TDatabase : DbConnection, new()
            where TParameter : DbParameter, new()
        {
            var result = new List<Table>();
            using var connection = new TDatabase()
            {
                ConnectionString = connectionString
            };
            connection.Open();
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
                result.Add(table);
            }
            return result;
        }
        /// <summary>
        /// Designed for SQL Server, might not work with other DB Provider.
        /// </summary>
        /// <typeparam name="TDatabase"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static List<StoredProcedureSchema> GetStoredProcedureSchemasDefinition<TDatabase, TParameter>(string connectionString)
            where TDatabase : DbConnection, new()
            where TParameter : DbParameter, new()
        {
            List<StoredProcedureSchema> result = null;
            using var connection = new TDatabase()
            {
                ConnectionString = connectionString
            };
            connection.Open();
            using var procedures = connection.GetSchema(SchemaRestriction.Procedures, null, null, null, "PROCEDURE");
            result = connection.GetStoredProcedures().ToList();
            return result;
        }
    }
}
