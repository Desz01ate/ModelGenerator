using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Interfaces;
using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Data.Common;

namespace ModelGenerator.Core.Services.DesignPattern.Interfaces
{
    public interface IServiceGenerator
    {
        string Directory { get; }
        string Namespace { get; }
        string ConnectionString { get; }
        string ModelDirectory { get; }
        string RepositoryDirectory { get; }
        string RepositoryComponentsDirectory { get; }
        IModelGenerator ModelGenerator { get; }
        void GenerateModel();
        void GenerateRepository(Table table);
        void GeneratePartialRepository(Table table);
        void GenerateService();
        void GenerateRepositoryDependencies();
        void SetGenerator<TDatabase, TParameter>(Func<string, string> parserFunction = null)
            where TDatabase : DbConnection, new()
            where TParameter : DbParameter, new();
    }
}
