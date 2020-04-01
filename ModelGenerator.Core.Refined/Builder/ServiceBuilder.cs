using ModelGenerator.Core.Entity;
using ModelGenerator.Core.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utilities.Classes;
using static ModelGenerator.Core.Event.Delegate.File;

namespace ModelGenerator.Core.Builder
{
    public sealed class ServiceBuilder
    {
        private readonly string _directory;
        private readonly string? _namespace;
        private readonly DatabaseDefinition _databaseDefinition;
        private string _modelDirectory => Path.Combine(_directory, "Models");
        private string _partialModelDirectory => Path.Combine(_modelDirectory, "Partials");
        private readonly bool _allowGeneratePartialModel;
        private string _repositoryDirectory => Path.Combine(_directory, "Repositories");
        private string _partialRepositoryDirectory => Path.Combine(_repositoryDirectory, "Partials");
        private string _repositoryBasedDirectory => Path.Combine(_repositoryDirectory, "Based");
        private readonly bool _allowGeneratePartialRepository;
        public event OnFileGenerated OnFileGenerated;

        public ServiceBuilder(string directory, string? @namespace, DatabaseDefinition databaseDefinition)
        {
            this._directory = directory;
            this._namespace = @namespace;
            this._databaseDefinition = databaseDefinition;
            Directory.CreateDirectory(_directory);
            Directory.CreateDirectory(_modelDirectory);
            Directory.CreateDirectory(_repositoryDirectory);
            Directory.CreateDirectory(_repositoryBasedDirectory);
            _allowGeneratePartialModel = true;//!Directory.Exists(_partialModelDirectory);
            _allowGeneratePartialRepository = true;//!Directory.Exists(_partialRepositoryDirectory);
        }
        public void Generate(IServiceBuilderProvider provider)
        {
            foreach (var table in this._databaseDefinition.Tables)
            {
                var modelCode = provider.GenerateModelFile(this._namespace, table);
                var modelFileName = $"{table.Name}.{provider.FileExtension}";
                if (!string.IsNullOrWhiteSpace(modelCode))
                {
                    var fileLoc = Path.Combine(_modelDirectory, modelFileName);
                    File.WriteAllText(fileLoc, modelCode);
                    this.OnFileGenerated?.Invoke(fileLoc);
                }
                if (_allowGeneratePartialModel)
                {
                    Directory.CreateDirectory(_partialModelDirectory);
                    var partialModelFileName = $"{table.Name}.{provider.FileExtension}";
                    var fileLoc = Path.Combine(_partialModelDirectory, modelFileName);
                    if (!File.Exists(fileLoc))
                    {
                        var partialModelCode = provider.GeneratePartialModelFile(this._namespace, table);

                        if (!string.IsNullOrWhiteSpace(partialModelCode))
                        {
                            File.WriteAllText(fileLoc, partialModelCode);
                            this.OnFileGenerated?.Invoke(fileLoc);
                        }
                    }
                }

                var repositoryName = $"{table.Name}Repository";
                var repositoryCode = provider.GenerateRepositoryFile(this._namespace, table);
                var repositoryFileName = $"{repositoryName}.{provider.FileExtension}";
                if (!string.IsNullOrWhiteSpace(repositoryCode))
                {
                    var fileLoc = Path.Combine(_repositoryDirectory, repositoryFileName);
                    File.WriteAllText(fileLoc, repositoryCode);
                    this.OnFileGenerated?.Invoke(fileLoc);
                }
                if (_allowGeneratePartialRepository)
                {
                    Directory.CreateDirectory(_partialRepositoryDirectory);
                    var partialRepoFileName = $"{table.Name}.{provider.FileExtension}";
                    var partialServiceFileLoc = Path.Combine(_partialRepositoryDirectory, repositoryFileName);
                    if (!File.Exists(partialServiceFileLoc))
                    {
                        var partialRepoCode = provider.GeneratePartialRepositoryFile(this._namespace, table);
                        if (!string.IsNullOrWhiteSpace(partialRepoCode))
                        {
                            File.WriteAllText(partialServiceFileLoc, partialRepoCode);
                            this.OnFileGenerated?.Invoke(partialServiceFileLoc);
                        }
                    }
                }
            }
            var repoBasedCode = provider.GenerateRepositoryBasedFile(this._namespace);
            var repoBasedFile = Path.Combine(this._repositoryBasedDirectory, $"Repository.{provider.FileExtension}");
            File.WriteAllText(repoBasedFile, repoBasedCode);
            this.OnFileGenerated?.Invoke(repoBasedFile);
            var serviceCode = provider.GenerateServiceFile(this._namespace, this._databaseDefinition.Tables, this._databaseDefinition.StoredProcedures);
            var serviceFile = Path.Combine(this._directory, $"Service.{provider.FileExtension}");
            File.WriteAllText(serviceFile, serviceCode);
            this.OnFileGenerated?.Invoke(serviceFile);
        }

    }
}
