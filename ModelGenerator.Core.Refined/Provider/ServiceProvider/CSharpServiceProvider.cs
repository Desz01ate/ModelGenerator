﻿using ModelGenerator.Core.Entity;
using ModelGenerator.Core.Interface;
using ModelGenerator.Core.Provider.ModelProvider;
using ModelGenerator.Core.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Classes;

namespace ModelGenerator.Core.Provider.ServiceProvider
{
    public class CSharpServiceProvider : CSharpModelProvider, IServiceBuilderProvider
    {
        private static readonly Lazy<CSharpServiceProvider> CSharpService = new Lazy<CSharpServiceProvider>(() => new CSharpServiceProvider(), true);
        public new static IServiceBuilderProvider Context => CSharpService.Value;
        internal CSharpServiceProvider()
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
            var template = new Repository_CSharp();
            template.Table = table;
            template.Namespace = @namespace;
            template.DataTypeMap = DataTypeMapper;
            template.TableNameTransformer = TableNameTransformer;
            template.IsPartial = true;
            return template.TransformText();
        }

        public string? GenerateRepositoryBasedFile(string @namespace)
        {
            var template = new RepositoryBased_CSharp();
            template.Namespace = @namespace;
            return template.TransformText();
        }

        public string? GenerateRepositoryFile(string @namespace, Table table)
        {
            var template = new Repository_CSharp();
            template.Table = table;
            template.Namespace = @namespace;
            template.DataTypeMap = DataTypeMapper;
            template.TableNameTransformer = TableNameTransformer;
            return template.TransformText();
        }

        public string? GenerateServiceFile(string @namespace, IEnumerable<Table> tables, IEnumerable<StoredProcedureSchema> storedProcedures)
        {
            var template = new Service_CSharp();
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
