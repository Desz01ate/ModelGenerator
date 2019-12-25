using ModelGenerator.Core.Services.Generator.Interfaces;
using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.Classes;
using Utilities.SQL.Extension;

namespace ModelGenerator.Core.Service
{
    public abstract class ModelGeneratorBased : IModelGenerator
    {
        public string DatabaseType { get; private set; }

        public string ParameterType { get; private set; }

        public string ConnectionString { get; private set; }

        public string Directory { get; protected set; }

        public string PartialDirectory { get; protected set; }

        public string Namespace { get; protected set; }

        public List<Table> Tables { get; } = new List<Table>();
        private bool _initialized = false;

        public List<StoredProcedureSchema> StoredProcedures { get; private set; } = new List<StoredProcedureSchema>();
        public Func<string, string> TableNameCleanser { get; private set; } = (x) => x;
        public ModelGeneratorBased(string connectionString, string directory, string @namespace, Func<string, string> cleanser = null)
        {
            if (cleanser != null)
            {
                TableNameCleanser = cleanser;
            }
            ConnectionString = connectionString;
            Directory = directory;
            Namespace = @namespace;
            System.IO.Directory.CreateDirectory(directory);
        }
        public ModelGeneratorBased(string connectionString, string directory, string partialDirectory, string @namespace, Func<string, string> cleanser = null)
        {
            if (cleanser != null)
            {
                TableNameCleanser = cleanser;
            }
            ConnectionString = connectionString;
            Directory = directory;
            PartialDirectory = partialDirectory;
            Namespace = @namespace;
            System.IO.Directory.CreateDirectory(directory);
            System.IO.Directory.CreateDirectory(partialDirectory);
        }
        public void SetDbConnector(DbConnection connection, DbParameter parameterType)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            DatabaseType = connection.GetType().FullName;
            ParameterType = parameterType.GetType().FullName;
            connection.ConnectionString = ConnectionString;

            using (connection)
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
            _initialized = true;
        }
        public void Generate()
        {
            if (!_initialized) throw new InvalidOperationException("You must call SetDbConnector first in order to call Generate");
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
            var v = Regex.Replace(value, "[-\\s]", "");
            return v;
        }
        protected string RemoveSpecialChars(string input)
        {
            return Regex.Replace(input, @"(\s|\$|-)", "");
        }
        protected abstract void GenerateCodeFile(Table table);
        protected virtual void GeneratePartialCodeFile(Table table)
        {

        }
        protected abstract string GetNullableDataType(TableSchema column);
        protected abstract string DataTypeMapper(string column);
    }
}
