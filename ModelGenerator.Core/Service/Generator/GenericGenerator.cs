using ModelGenerator.Core.Classes;
using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Service.Generator
{
    public class GenericGenerator : ModelGeneratorBased
    {
        private TypeDefinition[] typeDefinitions;
        private string[] formats;
        private bool nullableEnabled;
        private string nullableFormat;
        private string extension;
        private bool partialFilesEnabled;
        private bool renderOnce;
        private bool renderOnceExecuted;
        public GenericGenerator(string connectionString, string directory, string @namespace, string[] formatResources, Func<string, string> cleaner = null) : base(connectionString, directory, @namespace, cleaner)
        {
            ChangeDirectory(directory);
            ReloadResource(formatResources);
        }
        public void ChangeDirectory(string directory)
        {
            this.Directory = directory;
            this.PartialDirectory = Path.Combine(Directory, "Partials");
            System.IO.Directory.CreateDirectory(directory);
            System.IO.Directory.CreateDirectory(PartialDirectory);
        }
        public void ChangeNamespace(string @namespace)
        {
            if (string.IsNullOrWhiteSpace(@namespace))
            {
                throw new ArgumentNullException(nameof(@namespace));
            }
            this.Namespace = @namespace;
        }
        public void ReloadResource(string[] formatResources)
        {
            InitialState();
            if (formatResources == null || formatResources.Length == 0) throw new ArgumentNullException(nameof(formatResources));
            formats = formatResources;
            foreach (var line in formats)
            {
                if (line.StartsWith("@typedef"))
                {
                    var typedefResource = SplitDirective(line);
                    if (typedefResource.Length != 2) throw new FormatException("@typedef");
                    typeDefinitions = TypeDefinition.LoadFromResource(typedefResource[1]);
                }
                else if (line.StartsWith("@nullable"))
                {
                    var nullableResource = SplitDirective(line);
                    if (nullableResource.Length != 2) throw new FormatException("@nullable");
                    nullableEnabled = true;
                    nullableFormat = nullableResource[1];
                }
                else if (line.StartsWith("@ext"))
                {
                    var extResources = SplitDirective(line);
                    if (extResources.Length != 2) throw new FormatException("@ext");
                    extension = extResources[1];
                }
                else if (line.StartsWith("@partial"))
                {
                    partialFilesEnabled = true;
                }
                else if (line.StartsWith("@once"))
                {
                    renderOnce = true;
                }
            }
            if (typeDefinitions == null)
            {
                throw new ArgumentException("@typedef");
            }
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ArgumentNullException("@ext");
            }
        }

        private void InitialState()
        {
            typeDefinitions = null;
            formats = null;
            nullableEnabled = false;
            extension = null;
            partialFilesEnabled = false;
            renderOnce = false;
            renderOnceExecuted = false;
        }

        private string[] SplitDirective(string line)
        {
            var lines = line.Split(new char[] { ':' }, 2);
            return lines;
        }
        protected override string DataTypeMapper(string column)
        {
            foreach (var typedef in typeDefinitions)
            {
                if (typedef.TryGetValue(column, out var value))
                {
                    return value;
                }
            }
            return column;
        }
        static bool partialIgnoring = false;
        private string Interprete(Table table, TableSchema column, StoredProcedureSchema sp, string line, bool isPartialRendering = false)
        {
            var buffer = line;
            if (line.Contains("@partial-ignore")) partialIgnoring = true;
            if (line.Contains("@#partial-ignore")) partialIgnoring = false;
            if (partialIgnoring && isPartialRendering)
                return string.Empty;
            if (line.StartsWith("@typedef")
                || line.StartsWith("@nullable")
                || line.StartsWith("@ext")
                || line.StartsWith("@partial")
                || line.Contains("@once")
                || line.Contains("@partial-ignore")
                || line.Contains("@#partial-ignore"))
                return string.Empty;
            if (line.Contains("@namespace") && !string.IsNullOrWhiteSpace(Namespace))
            {
                var splitNamespace = SplitDirective(buffer);
                if (splitNamespace.Length != 2) throw new FormatException("@namespace");
                var replaced = splitNamespace[1].Replace("@namespace_name", Namespace);
                buffer = replaced;
            }
            if (line.Contains("@#namespace") && !string.IsNullOrWhiteSpace(Namespace))
            {
                var splitEndNs = SplitDirective(buffer);
                if (splitEndNs.Length != 2) throw new FormatException("@#namespace");
                buffer = splitEndNs[1];
            }
            if (line.Contains("@class_name"))
            {
                var replaced = buffer.Replace("@class_name", this.RemoveSpecialChars(table.Name));
                buffer = replaced;
            }
            if (line.Contains("@property_type"))
            {
                var replaced = buffer.Replace("@property_type", GetNullableDataType(column));
                buffer = replaced;
            }
            if (line.Contains("@property_name"))
            {
                var replaced = buffer.Replace("@property_name", ColumnNameCleanser(column.ColumnName));
                buffer = replaced;
            }
            if (line.Contains("@database_type"))
            {
                var replaced = buffer.Replace("@database_type", this.DatabaseType);
                buffer = replaced;
            }
            if (line.Contains("@database_parameter_type"))
            {
                var replaced = buffer.Replace("@database_parameter_type", this.ParameterType);
                buffer = replaced;
            }
            if (line.Contains("@sp_name"))
            {
                var replaced = buffer.Replace("@sp_name", sp.SPECIFIC_NAME);
                buffer = replaced;
            }
            if (line.Contains("@sp_args"))
            {
                var args = string.Join(",", sp.Parameters.Select(x => $"{x.DATA_TYPE} {x.PARAMETER_NAME}"));
                var replaced = buffer.Replace("@sp_args", args);
                buffer = replaced;
            }
            if (line.Contains("@sp_params_name"))
            {
                var sb = new StringBuilder();
                foreach (var param in sp.Parameters)
                {
                    var replaced = buffer.Replace("@sp_params_name", param.PARAMETER_NAME);
                    sb.AppendLine(replaced);
                }
                buffer = sb.ToString();
            }
            return buffer;
        }
        protected override void GenerateCodeFile(Table table)
        {
            if (renderOnce && renderOnceExecuted)
            {
                return;
            }
            var sb = new StringBuilder();
            bool repeatColRendered = false;
            bool repeatTabRendered = false;
            bool repeatSpRendered = false;
            foreach (var line in formats)
            {
                if (line.Contains("@repeat-col"))
                {
                    if (repeatColRendered) continue;
                    var repeatCols = formats.Where(x => x.Contains("@repeat-col")).ToArray();
                    foreach (var col in table.Columns)
                    {
                        foreach (var pdrLine in repeatCols)
                        {
                            var splitProps = SplitDirective(pdrLine);
                            if (splitProps.Length != 2) throw new FormatException("@repeat-col");
                            var replaced = Interprete(table, col, null, splitProps[1]);
                            if (!string.IsNullOrWhiteSpace(replaced))
                                sb.AppendLine(replaced);
                        }
                    }
                    repeatColRendered = true;
                }
                else if (line.Contains("@repeat-tab"))
                {
                    if (repeatTabRendered) continue;
                    var repeatTabs = formats.Where(x => x.Contains("@repeat-tab")).ToArray();
                    foreach (var _table in Tables)
                    {
                        foreach (var _line in repeatTabs)
                        {
                            var splitProps = SplitDirective(_line);
                            if (splitProps.Length != 2) throw new FormatException("@repeat-tab");
                            var replaced = Interprete(_table, null, null, splitProps[1]);
                            if (!string.IsNullOrWhiteSpace(replaced))
                                sb.AppendLine(replaced);

                        }
                    }
                    repeatTabRendered = true;
                }
                else if (line.Contains("@repeat-sp"))
                {
                    if (repeatSpRendered) continue;
                    var repeatSps = formats.Where(x => x.Contains("@repeat-sp")).ToArray();
                    foreach (var sp in this.StoredProcedures)
                    {
                        foreach (var _line in repeatSps)
                        {
                            var splitProps = SplitDirective(_line);
                            if (splitProps.Length != 2) throw new FormatException("@repeat-tab");
                            var replaced = Interprete(table, null, sp, splitProps[1]);
                            if (!string.IsNullOrWhiteSpace(replaced))
                                sb.AppendLine(replaced);
                        }
                    }
                    repeatSpRendered = true;
                }
                else
                {
                    var replaced = Interprete(table, null, null, line);
                    if (!string.IsNullOrWhiteSpace(replaced))
                        sb.AppendLine(replaced);
                }
            }
            var text = sb.ToString();
            string path = Path.Combine(Directory, renderOnce ? extension : $@"{table.Name}{extension}");
            File.WriteAllText(path, text);
            renderOnceExecuted = true;
        }
        protected override void GeneratePartialCodeFile(Table table)
        {
            if (partialFilesEnabled)
            {
                var sb = new StringBuilder();
                foreach (var line in formats)
                {
                    if (line.Contains("@repeat-col"))
                    {
                        continue;
                    }
                    else if (line.Contains("@repeat-tab"))
                    {
                        continue;
                    }
                    else if (line.Contains("@repeat-sp"))
                    {
                        continue;
                    }
                    else
                    {
                        var replaced = Interprete(table, null, null, line, true);
                        if (!string.IsNullOrWhiteSpace(replaced))
                            sb.AppendLine(replaced);
                    }
                }
                var text = sb.ToString();
                string path = Path.Combine(PartialDirectory, renderOnce ? extension : $@"{table.Name}{extension}");
                File.WriteAllText(path, text);
            }
        }
        protected override string GetNullableDataType(TableSchema column)
        {
            if (column == null) return string.Empty;
            var type = DataTypeMapper(column.DataTypeName);
            if (nullableEnabled && column.AllowDBNull)
            {
                return nullableFormat.Replace("@type", type);
            }
            return type;
        }
    }
}
