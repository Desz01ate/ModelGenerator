using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Data.Common;
using System.IO;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Services.Generator
{
    public class PHPGenerator<TDatabase, TParameter> : AbstractModelGenerator<TDatabase, TParameter>
        where TDatabase : DbConnection, new()
        where TParameter : DbParameter, new()
    {
        public PHPGenerator(string connectionString, string directory, string @namespace, Func<string, string> Cleaner = null) : base(connectionString, directory, @namespace, Cleaner)
        {

        }
        protected override string DataTypeMapper(string column)
        {
            throw new System.NotImplementedException();
        }

        protected override void GenerateCodeFile(Table table)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<?php");
            sb.AppendLine($"class {table.Name}");
            sb.AppendLine("{");
            foreach (var column in table.Columns)
            {
                var col = ColumnNameCleanser(column.ColumnName);
                sb.AppendLine($"    var ${col};");
                sb.AppendLine($"    function get{col}(){{");
                sb.AppendLine($"        return $this->{col};");
                sb.AppendLine($"    }}");
                sb.AppendLine($"    function set{col}($value){{");
                sb.AppendLine($"        $this->0h{col} = $value;");
                sb.AppendLine($"    }}");
            }
            sb.AppendLine("}");
            sb.AppendLine("?>");
            var filePath = Path.Combine(Directory, $@"{table.Name}.php");
            System.IO.File.WriteAllText(filePath, sb.ToString());
        }

        protected override string GetNullableDataType(TableSchema column)
        {
            throw new System.NotImplementedException();
        }
    }
}
