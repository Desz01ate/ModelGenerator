using ModelGenerator.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ModelGenerator.Core.Enum
{
    public enum SupportLanguage
    {
        [ServiceGenerateEnabled]
        [ControllerGenerateEnabled]
        [Description("C#")]
        CSharp,
        [ServiceGenerateEnabled]
        TypeScript,
        [Description("Visual Basic")]
        VisualBasic
    }
}
