using ModelGenerator.Core.Entity;
using ModelGenerator.Core.Entity.ModelProvider;
using ModelVisualizer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelVisualizer.Entity
{
    internal class TablePreview : IPreviewable
    {
        public readonly Table Table;
        private string _content;
        public string Content
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_content))
                {
                    _content = GenerateContent();
                }
                return _content;
            }
        }
        public TablePreview(Table table)
        {
            Table = table;
        }
        public override string ToString()
        {
            return this.Table.Name;
        }
        private string GenerateContent()
        {
            var template = CSharpModelProvider.Context;
            return template.GenerateModelFile("CustomNameSpace", this.Table);
        }
    }
}
