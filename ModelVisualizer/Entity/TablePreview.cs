using ModelGenerator.Core.Entity;
using ModelGenerator.Core.Provider.ModelProvider;
using ModelVisualizer.Interface;
using System.Data;

namespace ModelVisualizer.Entity
{
    internal class TablePreview : IPreviewable
    {
        public readonly Table Table;
        public readonly string Description;
        private string _content;
        public string Content
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_content) && Table != null)
                {
                    _content = GenerateContent();
                }
                return _content;
            }
        }
        public DataTable Structure { get; }
        public TablePreview(Table table)
        {
            Table = table;
            Description = table.Name;
            var structure = new DataTable();
            structure.Columns.Add("Field Name");
            structure.Columns.Add("Field Type");
            structure.Columns.Add("CLR Type");
            structure.Columns.Add("Primary Key");
            structure.Columns.Add("Is Key");
            structure.Columns.Add("Is Auto-Increment");
            foreach (var field in table.Columns)
            {
                var row = structure.NewRow();
                row[0] = field.ColumnName;
                row[1] = field.DataTypeName;
                row[2] = field.DataType;
                row[3] = table.PrimaryKey == field.ColumnName ? "Yes" : "";
                row[4] = field.IsKey ? "Yes" : "";
                row[5] = field.IsAutoIncrement ? "Yes" : "";
                structure.Rows.Add(row);
            }
            this.Structure = structure;
            //var head = structure.NewRow();
            //head.new
        }
        public TablePreview(Utilities.Classes.StoredProcedureSchema storedProcedure)
        {
            Description = storedProcedure.SPECIFIC_NAME;
            _content = "Unable to resolve type for stored procedure as it can return in any form according to its parameter.";
            var structure = new DataTable();
            structure.Columns.Add("Parameter Name");
            structure.Columns.Add("Parameter Type");
            structure.Columns.Add("CLR Type");
            structure.Columns.Add("Nullable");
            foreach (var param in storedProcedure.Parameters)
            {
                var row = structure.NewRow();
                row[0] = param.PARAMETER_NAME;
                row[1] = param.DATA_TYPE;
                row[2] = (CSharpModelProvider.Context as CSharpModelProvider).Mapper(param.DATA_TYPE);
                structure.Rows.Add(row);
            }
            this.Structure = structure;
        }
        public override string ToString()
        {
            return Description;
        }
        private string GenerateContent()
        {
            var template = CSharpModelProvider.Context;
            return template.GenerateModelFile("CustomNameSpace", this.Table);
        }
    }
}
