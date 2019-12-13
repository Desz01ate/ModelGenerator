using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Data.Common;
using System.IO;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Services.Generator
{
    public class CSharpGenerator<TDatabase, TParameter> : AbstractModelGenerator<TDatabase, TParameter>
        where TDatabase : DbConnection, new()
        where TParameter : DbParameter, new()
    {
        public CSharpGenerator(string connectionString, string directory, string @namespace, Func<string, string> Cleaner = null) : base(connectionString, directory, @namespace, Cleaner)
        {
        }
        public CSharpGenerator(string connectionString, string directory, string partialDirectory, string @namespace, Func<string, string> Cleaner = null) : base(connectionString, directory, partialDirectory, @namespace, Cleaner)
        {
        }
        protected override string GetNullableDataType(TableSchema column)
        {
            var typecs = DataTypeMapper(column.DataTypeName);
            var addNullability = column.AllowDBNull && typecs != "string" && typecs != "byte[]";
            return addNullability ? typecs + "?" : typecs;
        }
        protected override void GenerateCodeFile(Table table)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine();
            if (!string.IsNullOrWhiteSpace(Namespace))
            {
                sb.AppendLine($@"namespace {Namespace}");
                sb.AppendLine("{");
            }
            sb.AppendLine("//You can get Utilities package via nuget : Install-Package Deszolate.Utilities.Lite");
            sb.AppendLine($"//[Utilities.Attributes.SQL.Table(\"{TableNameCleanser(table.Name)}\")]");
            sb.AppendLine($@"public partial class {this.RemoveSpecialChars(table.Name)}");
            sb.AppendLine("{");
            foreach (var column in table.Columns)
            {
                var col = ColumnNameCleanser(column.ColumnName);
                if (column.ColumnName == table.PrimaryKey)
                {
                    sb.AppendLine($"    //[Utilities.Attributes.SQL.PrimaryKey]");
                }
                sb.AppendLine($"    public {GetNullableDataType(column)} {col} {{ get; set; }}");
            }
            sb.AppendLine("}");
            if (!string.IsNullOrWhiteSpace(Namespace))
            {
                sb.AppendLine("}");
            }
            var filePath = Path.Combine(Directory, $@"{table.Name}.cs");
            System.IO.File.WriteAllText(filePath, sb.ToString());
        }
        protected override void GeneratePartialCodeFile(Table table)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine();
            if (!string.IsNullOrWhiteSpace(Namespace))
            {
                sb.AppendLine($@"namespace {Namespace}");
                sb.AppendLine("{");
            }
            sb.AppendLine("//You can get Utilities package via nuget : Install-Package Deszolate.Utilities.Lite");
            sb.AppendLine($"//[Utilities.Attributes.SQL.Table(\"{TableNameCleanser(table.Name)}\")]");
            sb.AppendLine($@"public partial class {this.RemoveSpecialChars(table.Name)}");
            sb.AppendLine("{");
            sb.AppendLine("}");
            if (!string.IsNullOrWhiteSpace(Namespace))
            {
                sb.AppendLine("}");
            }
            var filePath = Path.Combine(PartialDirectory, $@"{table.Name}.cs");
            if (!File.Exists(filePath))
                System.IO.File.WriteAllText(filePath, sb.ToString());
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
                    return "short";
                case "int":
                case "integer":
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
                case "numeric":
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
                //case "Udt":
                case "udt":
                case "blob": //for sqlite
                    return "object";

                //case "Structured":
                case "structured":
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
