using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ModelGenerator.Core.Enum
{
    public enum SupportMode
    {
        [Description("Model (Data-Transfer-Object)")]
        Model = 0,
        [Description("Back-End Service")]
        BackendService = 1,
        [Description("Front-End Service")]
        FrontendService = 2,
        Controller = 3,
    }
}
