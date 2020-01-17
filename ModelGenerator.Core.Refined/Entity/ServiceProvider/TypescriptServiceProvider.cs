using ModelGenerator.Core.Refined.Entity.ModelProvider;
using ModelGenerator.Core.Refined.Interface;
using ModelGenerator.Core.Refined.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Refined.Entity.ServiceProvider
{
    public class TypescriptServiceProvider : TypeScriptModelProvider, IServiceBuilderProvider
    {
        private static readonly Lazy<TypescriptServiceProvider> TypescriptService = new Lazy<TypescriptServiceProvider>(() => new TypescriptServiceProvider(), true);
        public new static IServiceBuilderProvider Context => TypescriptService.Value;
        internal TypescriptServiceProvider()
        {

        }
        public override string? GenerateModelFile(string @namespace, Table table)
        {
            return base.GenerateModelFile(null, table);
        }
        public override string? GeneratePartialModelFile(string @namespace, Table table)
        {
            return null;
        }
        public string? GeneratePartialRepositoryFile(string @namespace, Table table)
        {
            return null;
        }

        public string? GenerateRepositoryBasedFile(string @namespace)
        {
            var template = new RepositoryBased_TypeScript();
            return template.TransformText();
        }

        public string? GenerateRepositoryFile(string @namespace, Table table)
        {
            var template = new Repository_TypeScript();
            template.Table = table;
            template.DataTypeMap = DataTypeMapper;
            template.TableNameTransformer = TableNameTransformer;
            return template.TransformText();
        }

        public string? GenerateServiceFile(string @namespace, IEnumerable<Table> tables, IEnumerable<StoredProcedureSchema> storedProcedures)
        {
            var template = new Service_TypeScript();
            template.Tables = tables;
            template.Namespace = @namespace;
            template.TableNameTransformer = this.TableNameTransformer;
            template.DataTypeMap = this.Mapper;
            template.StoredProcedures = storedProcedures;
            return template.TransformText();
        }
    }
}
