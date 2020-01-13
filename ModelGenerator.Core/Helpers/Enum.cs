using ModelGenerator.Core.AttributeHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModelGenerator.Core.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<(int Index, string Name, bool IsModelGenerator,bool IsControllerGenerator)> Expand<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            var members = typeof(T).GetMembers(BindingFlags.Static | BindingFlags.Public).Select((x, i) => (i, AttributeValidator.Description(x), AttributeValidator.IsModelGeneratorEnabled(x), AttributeValidator.IsControllerGeneratorEnabled(x)));
            foreach (var member in members)
            {
                yield return member;
            };
        }
    }
}
