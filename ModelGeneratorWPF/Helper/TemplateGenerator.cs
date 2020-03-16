using Microsoft.Data.SqlClient;
using ModelGenerator.Core.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ModelGeneratorWPF.Entity;
namespace ModelGeneratorWPF.Helper
{
    public static class TemplateGenerator
    {
        public static void AspNetCoreAPIWithServiceAndControllers(string directory, string @namespace, string connectionString, Action<string> log = null)
        {
            log?.Invoke("Begin template generate");
            log?.Invoke("Generating dotnet project and installing required packages...");
            Directory.SetCurrentDirectory(directory);
            RunCommand($"dotnet new webapi");
            RunCommand($"del WeatherForecast.cs");
            RunCommand($"del Controllers\\WeatherForecastController.cs");
            AlterLaunchSettings();
            AlterStartup();
            RunCommand($"dotnet add package Deszolate.Utilities.Lite --version 0.2.2.1");
            RunCommand($"dotnet add package Microsoft.Data.SqlClient");
            log?.Invoke("Gathering database structures...");
            var dbDef = ModelGenerator.Core.Helper.SqlDefinition.GetDatabaseDefinition<SqlConnection, SqlParameter>(connectionString, x => $"[{x}]");
            var svBuilder = new ServiceBuilder(directory, @namespace, dbDef);
            var controllerBuilder = new ControllerBuilder(directory, @namespace, dbDef);
            log?.Invoke("Scaffolding database connector service...");
            svBuilder.Generate(ModelGenerator.Core.Provider.ServiceProvider.CSharpServiceProvider.Context);
            log?.Invoke("Scaffolding required controllers...");
            controllerBuilder.Generate(ModelGenerator.Core.Provider.ControllerProvider.CSharpControllerProvider.Context);
            log?.Invoke("Done!");
            static void AlterStartup()
            {
                var filePath = @"Startup.cs";
                var content = File.ReadAllText(filePath);
                content = content.Replace("services.AddControllers();", "services.AddControllers();\n            services.AddScoped<Service>();");
                File.WriteAllText(filePath, content);
            }
            static void AlterLaunchSettings()
            {
                var filePath = @"Properties\launchSettings.json";
                var content = File.ReadAllText(filePath);
                var launchSettings = LaunchSettings.FromJson(content);
                launchSettings.Profiles.IisExpress.LaunchBrowser = false;
                launchSettings.Profiles.IisExpress.LaunchUrl = "";
                File.WriteAllText(filePath, launchSettings.ToJson());
            }
        }
        public static void AspNetCoreMVCWithService(string directory, string @namespace, string connectionString, Action<string> log = null)
        {
            log?.Invoke("Begin template generate");
            log?.Invoke("Generating dotnet project and installing required packages...");
            Directory.SetCurrentDirectory(directory);
            RunCommand($"dotnet new mvc");
            RunCommand($"dotnet add package Deszolate.Utilities.Lite --version 0.2.2.1");
            RunCommand($"dotnet add package Microsoft.Data.SqlClient");
            log?.Invoke("Gathering database structures...");
            var dbDef = ModelGenerator.Core.Helper.SqlDefinition.GetDatabaseDefinition<SqlConnection, SqlParameter>(connectionString, x => $"[{x}]");
            var svBuilder = new ServiceBuilder(directory, @namespace, dbDef);
            log?.Invoke("Scaffolding frontend service collections...");
            svBuilder.Generate(ModelGenerator.Core.Provider.ServiceProvider.CSharpConsumerServiceProvider.Context);
            log?.Invoke("Done!");
        }
        public static void AspNetCoreAPIWithEFCore(string directory, string connectionString, Action<string> log = null)
        {
            log?.Invoke("Begin template generate");
            log?.Invoke("Generating dotnet project and installing required packages...");
            Directory.SetCurrentDirectory(directory);
            RunCommand($"dotnet tool install --global dotnet-ef");
            RunCommand($"dotnet new webapi");
            RunCommand($"del WeatherForecast.cs");
            RunCommand($"del Controllers\\WeatherForecastController.cs");
            RunCommand($"dotnet add package Microsoft.EntityFrameworkCore.SqlServer");
            RunCommand($"dotnet add package Microsoft.EntityFrameworkCore.Design");
            log?.Invoke("Scaffolding entity framework...");
            RunCommand($"dotnet ef dbcontext scaffold \"{connectionString}\" Microsoft.EntityFrameworkCore.SqlServer -o Model --context-dir Contexts -c DatabaseContext");
            AlterStartup(connectionString);
            log?.Invoke("Done!");
            static void AlterStartup(string connectionString)
            {
                var filePath = @"Startup.cs";
                var content = File.ReadAllText(filePath);
                content = content.Replace("namespace", "using Microsoft.EntityFrameworkCore;\nnamespace");
                content = content.Replace($"services.AddControllers();", $"services.AddControllers();\n            services.AddDbContext<Contexts.DatabaseContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString(\"YourConnectionName\")));");
                File.WriteAllText(filePath, content);
            }
        }
        static void RunCommand(string command)
        {
            var procInfo = new ProcessStartInfo("cmd.exe");
            procInfo.Arguments = $"/C {command}";
            procInfo.UseShellExecute = false;
            procInfo.CreateNoWindow = true;
            var proc = Process.Start(procInfo);
            proc.WaitForExit();
        }
    }
}
