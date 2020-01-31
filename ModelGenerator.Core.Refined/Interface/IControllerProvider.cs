using ModelGenerator.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGenerator.Core.Interface
{
    public interface IControllerProvider
    {
        string FileExtension { get; }
        string? GenerateControllerFile(string @namespace, Table table);
    }
}
