using ModelGenerator.Core.Services.DesignPattern.Interfaces;
using ModelGenerator.Core.TextTemplates;
using System;
using System.Data.Common;

namespace ModelGenerator.Core.Services.DesignPattern.UnitOfWork.Generator
{
    public class UnitOfWorkGenerator
    {
        IServiceGenerator _strategy;
        public UnitOfWorkGenerator()
        {

        }
        public UnitOfWorkGenerator(IServiceGenerator strategy)
        {
            UseStrategy(strategy);
        }
        public void UseStrategy(IServiceGenerator strategy)
        {
            _strategy = strategy;
        }
        public void Generate()
        {
            if (_strategy == null) throw new NullReferenceException("Strategy must not be null.");
            System.IO.Directory.CreateDirectory(_strategy.RepositoryComponentsDirectory);
            System.IO.Directory.CreateDirectory(_strategy.ModelDirectory);
            System.IO.Directory.CreateDirectory(_strategy.RepositoryDirectory);
            GenerateRepositoryDependencies();
            GenerateModel();
            GenerateRepositories();
            GenerateService();
        }

        private void GenerateRepositoryDependencies()
        {
            _strategy.GenerateRepositoryDependencies();
            //var repositoryFile = System.IO.Path.Combine(_strategy.RepositoryComponentsDirectory, "Repository.cs");
            //var repositoryTemplate = new RepositoryTemplate();
            //repositoryTemplate.Namespace = _strategy.Namespace;
            //var content = repositoryTemplate.TransformText();
            //System.IO.File.WriteAllText(repositoryFile, content);

        }

        private void GenerateModel()
        {
            _strategy.GenerateModel();
        }
        private void GenerateRepositories()
        {
            foreach (var table in _strategy.ModelGenerator.Tables)
            {
                _strategy.GenerateRepository(table);
                _strategy.GeneratePartialRepository(table);
            }
        }
        private void GenerateService()
        {
            _strategy.GenerateService();
        }
    }
}
