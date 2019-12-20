using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Service.Generator
{
    public class GolangGenerator<TDatabase, TParameter> : AbstractModelGenerator<TDatabase, TParameter>
        where TDatabase : DbConnection, new()
        where TParameter : DbParameter, new()
    {
        public GolangGenerator(string connectionString, string directory, string @namespace, Func<string, string> Cleaner = null) : base(connectionString, directory, @namespace, Cleaner)
        {
        }
        public GolangGenerator(string connectionString, string directory, string partialDirectory, string @namespace, Func<string, string> Cleaner = null) : base(connectionString, directory, partialDirectory, @namespace, Cleaner)
        {
        }

        protected override string DataTypeMapper(string columnType)
        {
            columnType = columnType.ToLower();
            switch (columnType)
            {
                case "bit":
                    return "bool";

                case "tinyint":
                    return "byte";
                case "smallint":
                    return "int8";
                case "int":
                case "integer":
                    return "int";
                case "bigint":
                    return "int64";

                case "real":
                case "float":
                    return "float32";
                case "decimal":
                case "money":
                case "smallmoney":
                case "numeric":
                    return "float64";

                case "time":
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                case "datetimeoffset":
                    return "time.Time";

                case "char":
                case "varchar":
                case "nchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "xml":
                    return "string";

                case "binary":
                case "image":
                case "varbinary":
                case "timestamp":
                    return "[]byte";

                case "uniqueidentifier":
                    return "string";

                case "variant":
                //case "Udt":
                case "udt":
                case "blob": //for sqlite
                    return "interface{}";

                //case "Structured":
                case "structured":
                    return "interface{}";

                case "geography":
                    return "interface{}";
                default:
                    // Fallback to be manually handled by user
                    return columnType;
            };
        }

        protected override void GenerateCodeFile(Table table)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"type {this.RemoveSpecialChars(table.Name)} struct {{");
            foreach (var column in table.Columns)
            {
                var col = ColumnNameCleanser(column.ColumnName);
                var type = GetNullableDataType(column);
                sb.AppendLine($"{col} {type}");
            }
            sb.AppendLine(@"}");
            var filePath = Path.Combine(Directory, $@"{table.Name}.go");
            System.IO.File.WriteAllText(filePath, sb.ToString());
        }

        protected override string GetNullableDataType(TableSchema column)
        {
            var type = DataTypeMapper(column.DataTypeName);
            if (column.AllowDBNull)
            {
                return $"*{type}";
            }
            return type;
        }
    }
}
