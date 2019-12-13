using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Data.Common;
using System.IO;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Services.Generator
{
    public class PythonGenerator<TDatabase, TParameter> : AbstractModelGenerator<TDatabase, TParameter>
        where TDatabase : DbConnection, new()
        where TParameter : DbParameter, new()
    {
        public PythonGenerator(string connectionString, string directory, string @namespace, Func<string, string> Cleaner = null) : base(connectionString, directory, @namespace, Cleaner)
        {

        }
        protected override string DataTypeMapper(string column)
        {
            throw new System.NotImplementedException();
        }

        protected override void GenerateCodeFile(Table table)
        {
            var sb = new StringBuilder();
            sb.AppendLine($@"class {table.Name}:");
            sb.AppendLine($@"  def __init__(self):");
            foreach (var column in table.Columns)
            {
                var col = ColumnNameCleanser(column.ColumnName);
                sb.AppendLine($@"    self.{col} = None");
            }
            var filePath = Path.Combine(Directory, $@"{table.Name}.py");
            System.IO.File.WriteAllText(filePath, sb.ToString());
        }

        protected override string GetNullableDataType(TableSchema column)
        {
            throw new System.NotImplementedException();
        }
    }
}
