using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGenerator.Core.Refined.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class ServiceGenerateEnabledAttribute : Attribute
    {
        public ServiceGenerateEnabledAttribute()
        {
        }
    }
}
