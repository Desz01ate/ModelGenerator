using Microsoft.Data.SqlClient;
using Microsoft.WindowsAPICodePack.Dialogs;
using ModelGenerator.Core.Builder;
using ModelGenerator.Core.Entity;
using ModelGenerator.Core.Enum;
using ModelGenerator.Core.Helper;
using ModelGenerator.Core.Interface;
using ModelGenerator.Core.Provider.ControllerProvider;
using ModelGenerator.Core.Provider.ModelProvider;
using ModelGenerator.Core.Provider.ServiceProvider;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModelGeneratorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SupportMode mode;
        private SupportLanguage language;
        private SupportDatabase database;
        public MainWindow()
        {
            InitializeComponent();
            Log("Components initializing...");
            Log("Checking .NET Core SDK version...");
            var netCoreVersion = Helper.SystemValidator.GetNetCoreVersion();
            var isNetCoreInstalled = !string.IsNullOrWhiteSpace(netCoreVersion);
            if (isNetCoreInstalled)
            {
                Log($"Found .NET Core {netCoreVersion}.");
                Template1.IsEnabled = true;
            }
            #region initializer
            cb_GeneratorMode.SelectedIndex = 1;
            cb_GeneratorMode.SelectedIndex = 0;
            var supportedDatabase = ModelGenerator.Core.Helper.EnumHelper.Expand<SupportDatabase>();
            foreach (var database in supportedDatabase)
            {
                cb_TargetDatabase.Items.Add(database.Name);
            }
            var supportedMode = EnumHelper.Expand<SupportMode>();
            foreach (var mode in supportedMode)
            {
                cb_GeneratorMode.Items.Add(mode.Name);
            }
            #endregion
            Log("Initiailized.");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedIndex = (SupportMode)cb_GeneratorMode.SelectedIndex;
            if (cb_TargetLang != null)
            {
                cb_TargetLang.Items.Clear();
                var supportedLanguages = ModelGenerator.Core.Helper.EnumHelper.Expand<SupportLanguage>();
                mode = selectedIndex;
                switch (selectedIndex)
                {
                    case SupportMode.Model: //model generator
                        break;
                    case SupportMode.BackendService: //unit of work generator
                        supportedLanguages = supportedLanguages.Where(x => x.IsModelGenerator);
                        break;
                    case SupportMode.Controller: //controller generator
                        supportedLanguages = supportedLanguages.Where(x => x.IsControllerGenerator);
                        break;
                    case SupportMode.FrontendService:
                        supportedLanguages = supportedLanguages.Where(x => x.IsConsumerServiceGenerator);
                        break;
                }
                foreach (var language in supportedLanguages)
                {
                    cb_TargetLang.Items.Add(language.Name);
                }
                cb_TargetLang.SelectedIndex = 0;
            }
        }

        private void Cb_TargetLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            language = (SupportLanguage)cb_TargetLang.SelectedIndex;
        }

        private void Cb_TargetDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            database = (SupportDatabase)cb_TargetDatabase.SelectedIndex;
            switch (database)
            {
                case SupportDatabase.SQLServer:
                    txt_connectionString.Text = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password = myPassword;";
                    break;
                case SupportDatabase.Oracle:
                    txt_connectionString.Text = "Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;";
                    break;
                case SupportDatabase.MySQL:
                    txt_connectionString.Text = "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
                    break;
                case SupportDatabase.PostgreSQL:
                    txt_connectionString.Text = "Server=myServerAddress;Port=5432;Database=myDataBase;User Id=myUsername;Password = myPassword;";
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var outputDir = txt_outputDir.Content.ToString();
            if (!Directory.Exists(outputDir))
            {
                MessageBox.Show("You must select an output directory before trying to generate!");
                return;
            }
            btn_Generate.IsEnabled = false;
            var connectionString = txt_connectionString.Text;
            var @namespace = txt_namespace.Text;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Log("Connection string is not provide, process aborted.");
            }
            Log($"Start generate with settings : mode : {mode} | language : {language}");
            Task.Run(async () =>
            {
                try
                {
                    DatabaseDefinition databaseDefinition = GetDatabaseDefinition(connectionString);
                    switch (mode)
                    {
                        case SupportMode.Model:
                            var provider = language switch
                            {
                                SupportLanguage.CSharp => CSharpModelProvider.Context,
                                SupportLanguage.TypeScript => TypeScriptModelProvider.Context,
                                SupportLanguage.VisualBasic => VisualBasicModelProvider.Context,
                                _ => throw new NotSupportedException()
                            };
                            var modelGenerator = new ModelBuilder(outputDir, @namespace, databaseDefinition);
                            modelGenerator.OnFileGenerated += (f) => Log($"Generated {f}");
                            modelGenerator.Generate(provider);

                            break;
                        case SupportMode.FrontendService:
                            var consumerServiceProvider = language switch
                            {
                                SupportLanguage.CSharp => CSharpConsumerServiceProvider.Context,
                                SupportLanguage.TypeScript => TypescriptServiceProvider.Context,
                                _ => throw new NotSupportedException()
                            };
                            var consumerGenerator = new ServiceBuilder(outputDir, @namespace, databaseDefinition);
                            consumerGenerator.OnFileGenerated += (f) => Log($"Geenrated {f}");
                            consumerGenerator.Generate(consumerServiceProvider);
                            break;
                        case SupportMode.BackendService:
                            IServiceBuilderProvider serviceProvider = language switch
                            {
                                SupportLanguage.CSharp => CSharpServiceProvider.Context,
                                _ => throw new NotSupportedException()
                            };
                            var serviceGenerator = new ServiceBuilder(outputDir, @namespace, databaseDefinition);
                            serviceGenerator.OnFileGenerated += (f) => Log($"Generated {f}");
                            serviceGenerator.Generate(serviceProvider);
                            //GeneratorFactory.PerformRepositoryGenerate(targetLanguage, targetDatabaseConnector, txt_connectionString.Text, outputDir, txt_namespace.Text);
                            break;
                        case SupportMode.Controller:
                            IControllerProvider controllerProvider = language switch
                            {
                                SupportLanguage.CSharp => CSharpControllerProvider.Context,
                                _ => throw new NotSupportedException()
                            };
                            var controllerGenerator = new ControllerBuilder(outputDir, @namespace, databaseDefinition);
                            controllerGenerator.OnFileGenerated += (f) => Log($"Generated {f}");
                            controllerGenerator.Generate(controllerProvider);
                            break;
                    }
                    Log($"All tasks are done.");
                    Process.Start("explorer.exe", outputDir);
                }
                catch (Exception ex)
                {
                    Log($"Error with message : {ex.Message}");
                }
            });
            btn_Generate.IsEnabled = true;
        }
        private void Log(string text)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.txt_richProgress.AppendText($"[{DateTime.Now.ToString("hh:mm:ss")}] {text} \n");
                this.txt_richProgress.ScrollToEnd();
            });
        }
        private DatabaseDefinition GetDatabaseDefinition(string connectionString)
        {
            try
            {
                return database switch
                {
                    SupportDatabase.SQLServer => SqlDefinition.GetDatabaseDefinition<SqlConnection, SqlParameter>(connectionString, (x) => $"[{x}]"),
                    SupportDatabase.MySQL => SqlDefinition.GetDatabaseDefinition<MySqlConnection, MySqlParameter>(connectionString),
                    SupportDatabase.Oracle => SqlDefinition.GetDatabaseDefinition<OracleConnection, OracleParameter>(connectionString),
                    SupportDatabase.PostgreSQL => SqlDefinition.GetDatabaseDefinition<NpgsqlConnection, NpgsqlParameter>(connectionString),
                    SupportDatabase.SQLite => SqlDefinition.GetDatabaseDefinition<SQLiteConnection, SQLiteParameter>(connectionString),
                    _ => null,
                };
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return null;
            }
        }

        private void Txt_outputDir_MouseDown(object sender, MouseButtonEventArgs e)
        {


        }

        private void Txt_outputDir_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new CommonOpenFileDialog
            {
                InitialDirectory = "C:\\Users",
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                txt_outputDir.Content = dialog.FileName;
                var predictedNamespace = dialog.FileName.Split(@"\").Last();
                txt_namespace.Text = predictedNamespace;
            }
        }

        private void Template1_Click(object sender, RoutedEventArgs e)
        {
            var outputDir = txt_outputDir.Content.ToString();
            if (!Directory.Exists(outputDir))
            {
                MessageBox.Show("You must select an output directory before trying to generate!");
                return;
            }
            var @namespace = txt_namespace.Text;
            var connectionString = txt_connectionString.Text;
            Task.Run(() =>
            {
                try
                {
                    Helper.TemplateGenerator.AspNetCoreAPIWithServiceAndControllers(outputDir, @namespace, connectionString, Log);
                    Process.Start("explorer.exe", outputDir);
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                }
            });
        }

        private void Template2_Click(object sender, RoutedEventArgs e)
        {
            var outputDir = txt_outputDir.Content.ToString();
            if (!Directory.Exists(outputDir))
            {
                MessageBox.Show("You must select an output directory before trying to generate!");
                return;
            }
            var @namespace = txt_namespace.Text;
            var connectionString = txt_connectionString.Text;
            Task.Run(() =>
            {
                try
                {
                    Helper.TemplateGenerator.AspNetCoreMVCWithService(outputDir, @namespace, connectionString, Log);
                    Process.Start("explorer.exe", outputDir);
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                }
            });
        }

        private void Template3_Click(object sender, RoutedEventArgs e)
        {
            var outputDir = txt_outputDir.Content.ToString();
            if (!Directory.Exists(outputDir))
            {
                MessageBox.Show("You must select an output directory before trying to generate!");
                return;
            }
            var @namespace = txt_namespace.Text;
            var connectionString = txt_connectionString.Text;
            Task.Run(() =>
            {
                try
                {
                    Helper.TemplateGenerator.AspNetCoreAPIWithEFCore(outputDir, connectionString, Log);
                    Process.Start("explorer.exe", outputDir);
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                }
            });
        }
    }
}
