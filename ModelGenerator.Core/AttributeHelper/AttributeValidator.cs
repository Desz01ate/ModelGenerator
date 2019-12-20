using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace ModelGenerator.Core.AttributeHelper
{
    public static class AttributeValidator
    {
        public static bool IsModelGeneratorEnabled(MemberInfo member)
        {
            var isAssigned = member.GetCustomAttribute(typeof(ModelGenerator.Core.Attributes.RepositoryGenerateEnabledAttribute)) != null;
            return isAssigned;
        }
        public static string Description(MemberInfo member)
        {
            var descAttrib = member.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (descAttrib != null)
            {
                return descAttrib.Description;
            }
            return member.Name;
        }
    }
}
