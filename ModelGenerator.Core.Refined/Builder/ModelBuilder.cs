﻿using ModelGenerator.Core.Refined.Entity;
using ModelGenerator.Core.Refined.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModelGenerator.Core.Refined.Builder
{
    public sealed class ModelBuilder
    {
        private readonly string _directory;
        private readonly string _partialDirectory;
        private readonly bool _allowGeneratePartial;
        private readonly string? _namespace;
        private readonly DatabaseDefinition _databaseDefinition;
        public ModelBuilder(string directory, string? @namespace, DatabaseDefinition databaseDefinition)
        {
            this._directory = directory;
            this._partialDirectory = Path.Combine(directory, "Partials");
            Directory.CreateDirectory(_directory);
            if (!Directory.Exists(_partialDirectory))
            {
                Directory.CreateDirectory(_partialDirectory);
                _allowGeneratePartial = true;
            }
            this._namespace = @namespace;
            this._databaseDefinition = databaseDefinition;
        }
        public int Generate(IModelBuilderProvider provider)
        {
            var totalFiles = 0;
            foreach (var table in this._databaseDefinition.Tables)
            {
                var modelCode = provider.GenerateModelFile(this._namespace, table);
                var fileName = $"{table.Name}.{provider.FileExtension}";
                if (!string.IsNullOrWhiteSpace(modelCode))
                {
                    var fileLoc = Path.Combine(_directory, fileName);
                    File.WriteAllText(fileLoc, modelCode);
                    totalFiles++;
                }
                if (!_allowGeneratePartial) continue;
                var partialModelCode = provider.GeneratePartialModelFile(this._namespace, table);
                if (!string.IsNullOrWhiteSpace(partialModelCode))
                {
                    var fileLoc = Path.Combine(Path.Combine(_directory, "Partials"), fileName);
                    File.WriteAllText(fileLoc, partialModelCode);
                    totalFiles++;
                }
            }
            return totalFiles;
        }
    }
}
