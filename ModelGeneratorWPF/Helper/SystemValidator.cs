﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModelGeneratorWPF.Helper
{
    public static class SystemValidator
    {
        public static string GetNetCoreVersion()
        {
            var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
            var assemblyPath = assembly.CodeBase.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");
            if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
                return assemblyPath[netCoreAppIndex + 1];
            return null;
        }
        public static bool IsNetCoreInstalled()
        {
            return !string.IsNullOrWhiteSpace(GetNetCoreVersion());
        }
    }
}
