using ModelGenerator.Core.Entity;
using ModelGenerator.Core.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static ModelGenerator.Core.Event.Delegate.File;

namespace ModelGenerator.Core.Builder
{
    public sealed class ControllerBuilder
    {
        private readonly string _directory;
        private readonly string? _namespace;
        private readonly DatabaseDefinition _databaseDefinition;
        public event OnFileGenerated? OnFileGenerated;
        public ControllerBuilder(string directory, string? @namespace, DatabaseDefinition databaseDefinition)
        {
            this._directory = Path.Combine(directory, "Controllers");
            Directory.CreateDirectory(_directory);
            this._namespace = @namespace;
            this._databaseDefinition = databaseDefinition;
        }
        public void Generate(IControllerProvider provider)
        {
            foreach (var table in this._databaseDefinition.Tables)
            {
                var code = provider.GenerateControllerFile(this._namespace, table);
                var fileName = $"{Utilities.String.ToLeadingUpper(table.Name)}Controller.{provider.FileExtension}";
                if (!string.IsNullOrWhiteSpace(code))
                {
                    var fileLoc = Path.Combine(_directory, fileName);
                    File.WriteAllText(fileLoc, code);
                    this.OnFileGenerated?.Invoke(fileLoc);
                }
            }
        }
    }
}
