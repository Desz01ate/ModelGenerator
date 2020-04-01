using ModelGenerator.Core.Entity;
using ModelGenerator.Core.Interface;
using ModelGenerator.Core.Provider.ModelProvider;
using ModelGenerator.Core.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Classes;

namespace ModelGenerator.Core.Provider.ServiceProvider
{
    public class CSharpConsumerServiceProvider : CSharpModelProvider, IServiceBuilderProvider
    {
        private static readonly Lazy<CSharpConsumerServiceProvider> CSharpService = new Lazy<CSharpConsumerServiceProvider>(() => new CSharpConsumerServiceProvider(), true);
        public new static IServiceBuilderProvider Context => CSharpService.Value;
        internal CSharpConsumerServiceProvider()
        {

        }
        public override string? GenerateModelFile(string @namespace, Table table)
        {
            return base.GenerateModelFile(@namespace + ".Models", table);
        }
        public override string? GeneratePartialModelFile(string @namespace, Table table)
        {
            return base.GeneratePartialModelFile(@namespace + ".Models", table);
        }
        public string? GeneratePartialRepositoryFile(string @namespace, Table table)
        {
            var template = new ConsumerServiceComponent_CSharp();
            template.Table = table;
            template.Namespace = @namespace;
            template.DataTypeMap = DataTypeMapper;
            template.TableNameTransformer = TableNameTransformer;
            template.IsPartial = true;
            return template.TransformText();
        }

        public string? GenerateRepositoryBasedFile(string @namespace)
        {
            var template = new ConsumerServiceBased_CSharp();
            template.Namespace = @namespace;
            return template.TransformText();
        }

        public string? GenerateRepositoryFile(string @namespace, Table table)
        {
            var template = new ConsumerServiceComponent_CSharp();
            template.Table = table;
            template.Namespace = @namespace;
            template.DataTypeMap = DataTypeMapper;
            template.TableNameTransformer = TableNameTransformer;
            return template.TransformText();
        }

        public string? GenerateServiceFile(string @namespace, IEnumerable<Table> tables, IEnumerable<StoredProcedureSchema> storedProcedures)
        {
            var template = new ConsumerService_CSharp();
            template.Tables = tables;
            template.Namespace = @namespace;
            template.DatabaseType = tables.First().ConnectionProvider;
            template.DatabaseParamType = tables.First().ConnectionProviderParameterType;
            template.TableNameTransformer = this.TableNameTransformer;
            template.DataTypeMap = this.Mapper;
            template.StoredProcedures = storedProcedures;
            return template.TransformText();
        }
    }
}
