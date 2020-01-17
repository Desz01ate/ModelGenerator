using ModelGenerator.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGenerator.Core.Interface
{
    public interface IModelBuilderProvider
    {
        string FileExtension { get; }
        string? GenerateModelFile(string @namespace, Table table);
        string? GeneratePartialModelFile(string @namespace, Table table);
    }
}
