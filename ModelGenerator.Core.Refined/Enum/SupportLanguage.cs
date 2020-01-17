using ModelGenerator.Core.Refined.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ModelGenerator.Core.Refined.Enum
{
    public enum SupportLanguage
    {
        [ServiceGenerateEnabled]
        [Description("C#")]
        CSharp,
        [ServiceGenerateEnabled]
        TypeScript,
        [Description("Visual Basic")]
        VisualBasic
    }
}
