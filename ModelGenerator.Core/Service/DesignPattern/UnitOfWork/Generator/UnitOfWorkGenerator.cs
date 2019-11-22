using ModelGenerator.Core.Services.DesignPattern.Interfaces;
using System;
using System.Data.Common;

namespace ModelGenerator.Core.Services.DesignPattern.UnitOfWork.Generator
{
    public class UnitOfWorkGenerator
    {
        IGeneratorStrategy _strategy;
        public UnitOfWorkGenerator()
        {

        }
        public UnitOfWorkGenerator(IGeneratorStrategy strategy)
        {
            UseStrategy(strategy);
        }
        public void UseStrategy(IGeneratorStrategy strategy)
        {
            _strategy = strategy;
        }
        public void Generate()
        {
            if (_strategy == null) throw new NullReferenceException("Strategy must not be null.");
            System.IO.Directory.CreateDirectory(_strategy.ModelDirectory);
            System.IO.Directory.CreateDirectory(_strategy.RepositoryDirectory);
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(_strategy.ModelDirectory, "Partials"));
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(_strategy.RepositoryDirectory, "Partials"));
            GenerateModel();
            GenerateRepositories();
            GenerateService();
        }
        private void GenerateModel()
        {
            _strategy.GenerateModel();
        }
        private void GenerateRepositories()
        {
            foreach (var table in _strategy.Generator.Tables)
            {
                _strategy.Generator.GenerateFromSpecificTable(table, _strategy.GenerateRepository);
            }
        }
        private void GenerateService()
        {
            _strategy.GenerateService();
        }
    }
}
