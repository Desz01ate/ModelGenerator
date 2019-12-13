using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Collections.Generic;
using Utilities.Classes;

namespace ModelGenerator.Core.Services.Generator.Interfaces
{
    public interface IModelGenerator
    {
        string DatabaseType { get; }
        string ParameterType { get; }
        string ConnectionString { get; }
        string Directory { get; }
        string PartialDirectory { get; }
        string Namespace { get; }
        List<Table> Tables { get; }
        List<StoredProcedureSchema> StoredProcedures { get; }
        void Generate();
    }
}
