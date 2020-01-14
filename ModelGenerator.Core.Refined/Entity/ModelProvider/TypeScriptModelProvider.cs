using ModelGenerator.Core.Refined.Interface;
using ModelGenerator.Core.Refined.Template;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.Classes;

namespace ModelGenerator.Core.Refined.Entity.ModelProvider
{
    public class TypeScriptModelProvider : IModelBuilderProvider
    {
        public string FileExtension => "ts";

        public virtual string? GenerateModelFile(string @namespace, Table table)
        {
            var template = new Model_TypeScript();
            template.ClassName = table.Name;
            template.Namespace = @namespace;
            template.PrimaryKey = table.PrimaryKey;
            template.Columns = table.Columns;
            template.DataTypeMap = DataTypeMapper;
            template.TableNameTransformer = TableNameTransformer;
            return template.TransformText();
        }

        protected string TableNameTransformer(string tableName)
        {
            var v = Regex.Replace(tableName, "[-\\s]", "");
            return v;
        }

        protected string DataTypeMapper(TableSchema column)
        {
            var typets = Mapper(column.DataTypeName);
            if (column.AllowDBNull)
            {
                return $"{typets} | undefined | null";
            }
            return typets;
        }

        protected string Mapper(string dataTypeName)
        {
            switch (dataTypeName)
            {
                case "bit":
                    return "boolean";

                case "tinyint":
                case "smallint":
                case "int":
                case "bigint":
                case "real":
                case "float":
                case "decimal":
                case "money":
                case "smallmoney":
                    return "number";

                case "time":
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                case "datetimeoffset":
                    return "Date";

                case "char":
                case "varchar":
                case "nchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "xml":
                    return "string";

                case "uniqueidentifier":
                case "binary":
                case "image":
                case "varbinary":
                case "timestamp":
                case "variant":
                case "Udt":
                default:
                    // Fallback to be manually handled by user
                    return "any";
            };

        }

        public virtual string? GeneratePartialModelFile(string @namespace, Table table)
        {
            return null;
        }
    }
}
