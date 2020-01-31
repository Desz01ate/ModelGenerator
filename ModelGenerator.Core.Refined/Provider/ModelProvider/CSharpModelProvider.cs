using ModelGenerator.Core.Entity;
using ModelGenerator.Core.Interface;
using ModelGenerator.Core.Template;
using System;
using System.Text.RegularExpressions;
using Utilities.Classes;

namespace ModelGenerator.Core.Provider.ModelProvider
{
    public class CSharpModelProvider : IModelBuilderProvider
    {
        private static readonly Lazy<CSharpModelProvider> CSharpModel = new Lazy<CSharpModelProvider>(() => new CSharpModelProvider(), true);
        public static IModelBuilderProvider Context => CSharpModel.Value;
        internal CSharpModelProvider()
        {

        }
        public string FileExtension => "cs";

        public virtual string? GenerateModelFile(string @namespace, Table table)
        {
            var template = new Model_CSharp();
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
            var template = new Model_CSharp();
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
            var typecs = Mapper(column.DataTypeName);
            var addNullability = column.AllowDBNull && typecs != "string" && typecs != "byte[]";
            return addNullability ? typecs + "?" : typecs;
        }
        protected string Mapper(string columnType)
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
