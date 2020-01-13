using ModelGenerator.Core.Services.Generator;
using ModelGenerator.Core.Services.Generator.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using Utilities.Classes;

namespace ModelGenerator.Core.Service.ControllerGenerator
{
    public class CSharpControllerGenerator<TDatabase, TParameter> : CSharpGenerator<TDatabase, TParameter>
        where TDatabase : DbConnection, new()
        where TParameter : DbParameter, new()
    {
        public CSharpControllerGenerator(string connectionString, string directory, string @namespace, Func<string, string> Cleaner = null) : base(connectionString, directory, @namespace, Cleaner)
        {
        }

        public CSharpControllerGenerator(string connectionString, string directory, string partialDirectory, string @namespace, Func<string, string> Cleaner = null) : base(connectionString, directory, partialDirectory, @namespace, Cleaner)
        {
        }

        protected override void GenerateCodeFile(Table table)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using Microsoft.AspNetCore.Http;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine();
            if (!string.IsNullOrWhiteSpace(Namespace))
            {
                sb.AppendLine($@"namespace {Namespace}.Controllers");
                sb.AppendLine("{");
            }
            sb.AppendLine($"[Route(\"api/[controller]\")]");
            sb.AppendLine($"[ApiController]");
            sb.AppendLine($@"public class {this.RemoveSpecialChars(table.Name)}Controller : ControllerBase");
            sb.AppendLine("{");
            sb.AppendLine(@"        [HttpGet]");
            sb.AppendLine($@"       public async Task<IEnumerable<object>> Get()");
            sb.AppendLine("       {");
            sb.AppendLine($@"           throw new NotImplementedException();");
            sb.AppendLine("       }");
            sb.AppendLine(@"        [HttpPost]");
            sb.AppendLine($@"       public async Task Post([FromBody] dynamic data)");
            sb.AppendLine("       {");
            sb.AppendLine($@"           throw new NotImplementedException();");
            sb.AppendLine("       }");
            sb.AppendLine(@"        [HttpPut]");
            sb.AppendLine($@"       public async Task Put([FromBody] dynamic data)");
            sb.AppendLine("       {");
            sb.AppendLine($@"           throw new NotImplementedException();");
            sb.AppendLine("       }");
            sb.AppendLine(@"        [HttpDelete]");
            sb.AppendLine($@"       public async Task Delete()");
            sb.AppendLine("       {");
            sb.AppendLine($@"           throw new NotImplementedException();");
            sb.AppendLine("       }");
            sb.AppendLine("}");
            if (!string.IsNullOrWhiteSpace(Namespace))
            {
                sb.AppendLine("}");
            }
            var filePath = Path.Combine(Directory, $@"{table.Name}Controller.cs");
            System.IO.File.WriteAllText(filePath, sb.ToString());
        }

        protected override void GeneratePartialCodeFile(Table table)
        {

        }
    }
}
