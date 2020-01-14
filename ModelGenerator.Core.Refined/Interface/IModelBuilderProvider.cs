using ModelGenerator.Core.Refined.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGenerator.Core.Refined.Interface
{
    public interface IModelBuilderProvider
    {
        string FileExtension { get; }
        string? GenerateModelFile(string @namespace, Table table);
        string? GeneratePartialModelFile(string @namespace, Table table);
    }
}
