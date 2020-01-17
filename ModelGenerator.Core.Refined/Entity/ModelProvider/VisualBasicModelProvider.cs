using ModelGenerator.Core.Interface;
using ModelGenerator.Core.Template;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.Classes;

namespace ModelGenerator.Core.Entity.ModelProvider
{
    public class VisualBasicModelProvider : IModelBuilderProvider
    {
        private static readonly Lazy<VisualBasicModelProvider> VbModel = new Lazy<VisualBasicModelProvider>(() => new VisualBasicModelProvider(), true);
        public static IModelBuilderProvider Context => VbModel.Value;
        internal VisualBasicModelProvider()
        {

        }
        public string FileExtension => "vb";

        public virtual string? GenerateModelFile(string @namespace, Table table)
        {
            var template = new Model_VisualBasic();
            template.ClassName = table.Name;
            template.Namespace = @namespace;
            template.PrimaryKey = table.PrimaryKey;
            template.Columns = table.Columns;
            template.DataTypeMap = DataTypeMapper;
            template.TableNameTransformer = TableNameTransformer;
            return template.TransformText();
        }
        public virtual string? GeneratePartialModelFile(string @namespace, Table table)
        {
            var template = new Model_VisualBasic();
            template.ClassName = table.Name;
            template.Namespace = @namespace;
            template.PrimaryKey = table.PrimaryKey;
            template.Columns = table.Columns;
            template.DataTypeMap = DataTypeMapper;
            template.TableNameTransformer = TableNameTransformer;
            template.IsPartial = true;
            return template.TransformText();
        }
        protected string TableNameTransformer(string tableName)
        {
            var v = Regex.Replace(tableName, "[-\\s]", "");
            return v;
        }
        protected string DataTypeMapper(TableSchema column)
        {
            var typevb = Mapper(column.DataTypeName);
            var addNullable = column.AllowDBNull && typevb != "String" && typevb != "Byte()";
            return addNullable ? $"Nullable(Of {typevb})" : typevb;
        }
        protected string Mapper(string columnType)
        {
            columnType = columnType.ToLower();
            switch (columnType)
            {
                case "bit":
                    return "Boolean";

                case "tinyint":
                    return "Byte";
                case "smallint":
                    return "Short";
                case "int":
                    return "Integer";
                case "bigint":
                    return "Long";

                case "real":
                    return "Single";
                case "float":
                    return "Double";
                case "decimal":
                case "money":
                case "smallmoney":
                    return "Decimal";

                case "time":
                    return "TimeSpan";
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return "Date";
                case "datetimeoffset":
                    return "Date";

                case "char":
                case "varchar":
                case "nchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "xml":
                    return "String";

                case "binary":
                case "image":
                case "varbinary":
                case "timestamp":
                    return "Byte()";

                case "uniqueidentifier":
                    return "Guid";

                case "variant":
                case "Udt":
                    return "Object";

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
