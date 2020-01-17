using ModelGenerator.Core.Refined.Entity;
using ModelGenerator.Core.Refined.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Refined.Builder
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
        public ServiceBuilder(string directory, string? @namespace, DatabaseDefinition databaseDefinition)
        {
            this._directory = directory;
            this._namespace = @namespace;
            this._databaseDefinition = databaseDefinition;
            Directory.CreateDirectory(_directory);
            Directory.CreateDirectory(_modelDirectory);
            Directory.CreateDirectory(_repositoryDirectory);
            Directory.CreateDirectory(_repositoryBasedDirectory);
            _allowGeneratePartialModel = !Directory.Exists(_partialModelDirectory);
            _allowGeneratePartialRepository = !Directory.Exists(_partialRepositoryDirectory);
        }
        public int Generate(IServiceBuilderProvider provider)
        {
            int totalFiles = 0;
            foreach (var table in this._databaseDefinition.Tables)
            {
                var modelCode = provider.GenerateModelFile(this._namespace, table);
                var modelFileName = $"{table.Name}.{provider.FileExtension}";
                if (!string.IsNullOrWhiteSpace(modelCode))
                {
                    var fileLoc = Path.Combine(_modelDirectory, modelFileName);
                    File.WriteAllText(fileLoc, modelCode);
                    totalFiles++;
                }
                if (_allowGeneratePartialModel)
                {
                    Directory.CreateDirectory(_partialModelDirectory);
                    var partialModelCode = provider.GeneratePartialModelFile(this._namespace, table);
                    var partialModelFileName = $"{table.Name}.{provider.FileExtension}";
                    if (!string.IsNullOrWhiteSpace(partialModelCode))
                    {
                        var fileLoc = Path.Combine(_partialModelDirectory, modelFileName);
                        File.WriteAllText(fileLoc, partialModelCode);
                        totalFiles++;
                    }
                }


                var repositoryCode = provider.GenerateRepositoryFile(this._namespace, table);
                var repositoryFileName = $"{table.Name}Repository.{provider.FileExtension}";
                if (!string.IsNullOrWhiteSpace(repositoryCode))
                {
                    var fileLoc = Path.Combine(_repositoryDirectory, repositoryFileName);
                    File.WriteAllText(fileLoc, repositoryCode);
                    totalFiles++;
                }
                if (_allowGeneratePartialRepository)
                {
                    Directory.CreateDirectory(_partialRepositoryDirectory);
                    var partialRepoCode = provider.GeneratePartialRepositoryFile(this._namespace, table);
                    var partialRepoFileName = $"{table.Name}.{provider.FileExtension}";
                    if (!string.IsNullOrWhiteSpace(partialRepoCode))
                    {
                        var fileLoc = Path.Combine(_partialRepositoryDirectory, repositoryFileName);
                        File.WriteAllText(fileLoc, partialRepoCode);
                        totalFiles++;
                    }
                }

            }
            var repoBasedCode = provider.GenerateRepositoryBasedFile(this._namespace);
            var repoBasedFile = Path.Combine(this._repositoryBasedDirectory, $"Repository.{provider.FileExtension}");
            File.WriteAllText(repoBasedFile, repoBasedCode);
            totalFiles++;
            var serviceCode = provider.GenerateServiceFile(this._namespace, this._databaseDefinition.Tables, this._databaseDefinition.StoredProcedures);
            var serviceFile = Path.Combine(this._directory, $"Service.{provider.FileExtension}");
            File.WriteAllText(serviceFile, serviceCode);
            totalFiles++;
            return totalFiles;
        }

    }
}
