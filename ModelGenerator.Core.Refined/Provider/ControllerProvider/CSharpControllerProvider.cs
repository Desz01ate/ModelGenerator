using ModelGenerator.Core.Entity;
using ModelGenerator.Core.Interface;
using ModelGenerator.Core.Provider.ModelProvider;
using ModelGenerator.Core.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGenerator.Core.Provider.ControllerProvider
{
    public class CSharpControllerProvider : CSharpModelProvider, IControllerProvider
    {
        private static readonly Lazy<CSharpControllerProvider> CSharpControllerContext = new Lazy<CSharpControllerProvider>(() => new CSharpControllerProvider(), true);
        public static IControllerProvider Context => CSharpControllerContext.Value;
        internal CSharpControllerProvider() { }
        public string? GenerateControllerFile(string @namespace, Table table)
        {
            var template = new Controller_CSharp();
            template.ClassName = table.Name;
            template.Namespace = @namespace;
            template.PrimaryKey = table.PrimaryKey;
            template.Columns = table.Columns;
            template.DataTypeMap = DataTypeMapper;
            template.TableNameTransformer = (x) => this.TableNameTransformer(x);
            return template.TransformText();
        }
    }
}
