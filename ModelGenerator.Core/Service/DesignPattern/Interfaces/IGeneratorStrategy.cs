using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Interfaces;
using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Data.Common;

namespace ModelGenerator.Core.Services.DesignPattern.Interfaces
{
    public interface IGeneratorStrategy
    {
        string Directory { get; }
        string Namespace { get; }
        string ConnectionString { get; }
        string ModelDirectory { get; }
        string RepositoryDirectory { get; }
        IModelGenerator Generator { get; }
        void GenerateModel();
        void GenerateRepository(Table table);
        void GenerateService();
        void SetGenerator<TDatabase>(Func<string, string> parserFunction = null) where TDatabase : DbConnection, new();
    }
}
