using ModelGenerator.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ModelGenerator.Core.Enum
{
    public enum SupportLanguage
    {
        [ControllerGenerateEnabled]
        [FrontendServiceGenerateEnabled]
        [BackendServiceGenerateEnabled]
        [Description("C#")]
        CSharp,
        [FrontendServiceGenerateEnabled]
        TypeScript,
        [Description("Visual Basic")]
        VisualBasic
    }
}
