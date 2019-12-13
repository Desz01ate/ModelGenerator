using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Data.Common;
using System.IO;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Services.Generator
{
    public class CPPGenerator<TDatabase, TParameter> : AbstractModelGenerator<TDatabase, TParameter>
        where TDatabase : DbConnection, new()
        where TParameter : DbParameter, new()
    {
        public CPPGenerator(string connectionString, string directory, string @namespace, Func<string, string> Cleaner = null) : base(connectionString, directory, @namespace, Cleaner)
        {

        }
        protected override string GetNullableDataType(TableSchema column)
        {
            var typecpp = DataTypeMapper(column.DataTypeName);
            return typecpp;
        }
        protected override void GenerateCodeFile(Table table)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"#include <string>");
            if (!string.IsNullOrWhiteSpace(Namespace))
            {
                sb.AppendLine($@"namespace {Namespace}");
                sb.AppendLine("{");
            }
            sb.AppendLine($@"class {this.RemoveSpecialChars(table.Name)}");
            sb.AppendLine("{");
            sb.AppendLine("    public:");
            foreach (var column in table.Columns)
            {
                var col = ColumnNameCleanser(column.ColumnName);
                sb.AppendLine($"    {GetNullableDataType(column)} {col};");
            }
            sb.AppendLine("}");
            if (!string.IsNullOrWhiteSpace(Namespace))
            {
                sb.AppendLine("}");
            }
            var filePath = Path.Combine(Directory, $@"{table.Name}.cpp");
            System.IO.File.WriteAllText(filePath, sb.ToString());
        }
        protected override string DataTypeMapper(string columnType)
        {
            switch (columnType)
            {
                case "bit":
                    return "bool";

                case "tinyint":
                    return "byte";
                case "smallint":
                    return "short";
                case "int":
                    return "int";
                case "bigint":
                    return "long";

                case "real":
                    return "float";
                case "float":
                    return "double";
                case "decimal":
                case "money":
                case "smallmoney":
                    return "decimal";

                case "time":
                    return "TimeSpan";
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return "DateTime";
                case "datetimeoffset":
                    return "DateTimeOffset";

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
                    return "byte[]";

                case "uniqueidentifier":
                    return "Guid";

                case "variant":
                case "Udt":
                    return "object";

                case "Structured":
                    return "DataTable";

                case "geography":
                    return "geography";

                default:
                    // Fallback to be manually handled by user
                    return columnType;
            };
        }
    }
}
