using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Model;
using System.Data.Common;

namespace ModelGenerator.Core.Services.DesignPattern.Interfaces
{
    public interface IGeneratorStrategy<TDatabase> where TDatabase : DbConnection, new()
    {
        string Directory { get; }
        string Namespace { get; }
        string ConnectionString { get; }
        string ModelDirectory { get; }
        string RepositoryDirectory { get; }
        AbstractModelGenerator<TDatabase> Generator { get; }
        void GenerateModel();
        void GenerateRepository(Table table);
        void GenerateService();
    }
}
