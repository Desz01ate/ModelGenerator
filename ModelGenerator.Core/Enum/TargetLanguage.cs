using ModelGenerator.Core.Attributes;
using System.ComponentModel;

namespace ModelGenerator.Core.Enum
{
    public enum TargetLanguage
    {
        [Description("C#")]
        [RepositoryGenerateEnabled]
        CSharp = 0,
        [Description("Visual Basic")]
        [RepositoryGenerateEnabled]
        VisualBasic = 1,
        [Description("TypeScript")]
        [RepositoryGenerateEnabled]
        TypeScript = 2,
        [Description("PHP")]
        PHP = 3,
        [Description("Python")]
        Python = 4,
        [Description("Python 3.7")]
        Python37 = 5,
        [Description("Java")]
        Java = 6,
        [Description("C++")]
        CPP = 7,
        [Description("Golang")]
        Golang = 8,
        [Description("Dart")]
        Dart = 9
    }
}
