using ModelGenerator.Core.Refined.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Refined.Interface
{
    public interface IServiceBuilderProvider : IModelBuilderProvider
    {
        string? GenerateRepositoryFile(string @namespace, Table table);
        string? GeneratePartialRepositoryFile(string @namespace, Table table);
        string? GenerateRepositoryBasedFile(string @namespace);
        string? GenerateServiceFile(string @namespace, IEnumerable<Table> tables, IEnumerable<StoredProcedureSchema> storedProcedures);
    }
}
