using ModelGenerator.Core.Services.Generator.Interfaces;
using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.Classes;
using Utilities.SQL.Extension;
namespace ModelGenerator.Core.Services.Generator
{
    public abstract class AbstractModelGenerator<TDatabase, TParameter> : IModelGenerator
        where TDatabase : DbConnection, new()
        where TParameter : DbParameter, new()
    {
        public string ConnectionString { get; private set; }
        public string DatabaseType { get; private set; }
        public string ParameterType { get; private set; }
        public string Directory { get; private set; }
        public string PartialDirectory { get; private set; }

        public string Namespace { get; private set; }

        public List<Table> Tables { get; } = new List<Table>();
        public List<StoredProcedureSchema> StoredProcedures { get; private set; } = new List<StoredProcedureSchema>();
        public Func<string, string> TableNameCleanser { get; private set; } = (x) => x;
        public AbstractModelGenerator(string connectionString, string directory, string @namespace, Func<string, string> Cleaner = null)
        {
            if (Cleaner != null)
            {
                TableNameCleanser = Cleaner;
            }
            ConnectionString = connectionString;
            Directory = directory;
            Namespace = @namespace;
            System.IO.Directory.CreateDirectory(directory);
            Initialization();
        }
        protected string RemoveSpecialChars(string input)
        {
            return Regex.Replace(input, @"(\s|\$|-)", "");
        }
        public AbstractModelGenerator(string connectionString, string directory, string partialDirectory, string @namespace, Func<string, string> Cleaner = null)
        {
            if (Cleaner != null)
            {
                TableNameCleanser = Cleaner;
            }
            ConnectionString = connectionString;
            Directory = directory;
            PartialDirectory = partialDirectory;
            Namespace = @namespace;
            System.IO.Directory.CreateDirectory(directory);
            System.IO.Directory.CreateDirectory(partialDirectory);
            Initialization();
        }
        private void Initialization()
        {
            DatabaseType = typeof(TDatabase).FullName;
            ParameterType = typeof(TParameter).FullName;
            using (var connection = new TDatabase()
            {
                ConnectionString = ConnectionString
            })
            {
                connection.Open();
                using (var tables = connection.GetSchema("Tables"))
                {
                    foreach (DataRow row in tables.Rows)
                    {
                        var database = row[0].ToString();
                        var schema = row[1].ToString();
                        var tableName = row[2].ToString();
                        var type = row[3].ToString();

                        var columns = connection.GetTableSchema(TableNameCleanser(tableName));
                        string primaryKey = null;
                        using (var indexes = connection.GetSchema("IndexColumns", new string[] { null, null, tableName }))
                        {
                            if (indexes != null)
                            {
                                foreach (DataRow rowInfo in indexes.Rows)
                                {
                                    primaryKey = rowInfo["column_name"].ToString();
                                }
                            }
                        }
                        var table = new Table()
                        {
                            Name = tableName,
                            Columns = columns,
                            PrimaryKey = primaryKey
                        };
                        Tables.Add(table);
                    }
                }

                try
                {
                    var restrictions = new string[] { null, null, null, "PROCEDURE" };
                    using (var procedures = connection.GetSchema("Procedures", restrictions))
                    {
                        StoredProcedures = connection.GetStoredProcedures().ToList();
                        foreach (var sp in StoredProcedures)
                        {
                            foreach (var param in sp.Parameters)
                            {
                                param.DATA_TYPE = DataTypeMapper(param.DATA_TYPE);
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }
        public void Generate()
        {
            foreach (var table in Tables)
            {
                if (!string.IsNullOrWhiteSpace(this.Directory))
                    GenerateCodeFile(table);
                if (!string.IsNullOrWhiteSpace(this.PartialDirectory))
                    GeneratePartialCodeFile(table);
            }
        }
        protected string ColumnNameCleanser(string value)
        {
            var v = new Regex("[-\\s]").Replace(value, "");
            return v;
        }
        protected abstract void GenerateCodeFile(Table table);
        protected virtual void GeneratePartialCodeFile(Table table)
        {

        }
        protected abstract string GetNullableDataType(TableSchema column);
        protected abstract string DataTypeMapper(string column);
    }
}
