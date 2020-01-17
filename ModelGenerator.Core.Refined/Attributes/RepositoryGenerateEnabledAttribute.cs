using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGenerator.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class ServiceGenerateEnabledAttribute : Attribute
    {
        public ServiceGenerateEnabledAttribute()
        {
        }
    }
}
