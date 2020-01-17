using ModelGenerator.Core.Refined.Builder;
using ModelGenerator.Core.Refined.Entity;
using ModelGenerator.Core.Refined.Entity.ModelProvider;
using ModelGenerator.Core.Refined.Entity.ServiceProvider;
using ModelGenerator.Core.Refined.Enum;
using ModelGenerator.Core.Refined.Helper;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelGenerator
{
    public partial class GeneratorForm : Form
    {
        private SupportMode mode;
        private SupportLanguage language;
        private SupportDatabase database;
        private readonly string Namespace;
        private readonly string Directory;
        public GeneratorForm(string @namespace, string directory)
        {
            this.Namespace = @namespace;
            this.Directory = directory;
            InitializeComponent();
            cb_GeneratorMode.SelectedIndex = 1;
            cb_GeneratorMode.SelectedIndex = 0;
            var supportedDatabase = ModelGenerator.Core.Refined.Helper.EnumHelper.Expand<SupportDatabase>();
            foreach (var database in supportedDatabase)
            {
                cb_TargetDatabase.Items.Add(database.Name);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(Directory))
            {
                MessageBox.Show("You must select an output directory before trying to generate!");
                return;
            }
            var connectionString = this.txt_ConnectionString.Text;
            Task.Run(() =>
            {
                DatabaseDefinition databaseDefinition = GetDatabaseDefinition(connectionString);
                try
                {
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
                            var modelGenerator = new ModelBuilder(Directory, Namespace, databaseDefinition);
                            modelGenerator.Generate(provider);
                            break;
                        case SupportMode.Service:
                            var serviceProvider = language switch
                            {
                                SupportLanguage.CSharp => CSharpServiceProvider.Context,
                                SupportLanguage.TypeScript => TypescriptServiceProvider.Context,
                                _ => throw new NotSupportedException()
                            };
                            var serviceGenerator = new ServiceBuilder(Directory, Namespace, databaseDefinition);
                            var totalFiles = serviceGenerator.Generate(serviceProvider);
                            //GeneratorFactory.PerformRepositoryGenerate(targetLanguage, targetDatabaseConnector, txt_connectionString.Text, outputDir, txt_namespace.Text);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MessageBox.Show("Done");
                this.Dispose();
            });

        }


        private DatabaseDefinition GetDatabaseDefinition(string connectionString)
        {
            return database switch
            {
                SupportDatabase.SQLServer => SqlDefinition.GetDatabaseDefinition<SqlConnection, SqlParameter>(connectionString, (x) => $"[{x}]"),
                SupportDatabase.MySQL => SqlDefinition.GetDatabaseDefinition<MySqlConnection, MySqlParameter>(connectionString),
                SupportDatabase.Oracle => SqlDefinition.GetDatabaseDefinition<OracleConnection, OracleParameter>(connectionString),
                SupportDatabase.PostgreSQL => SqlDefinition.GetDatabaseDefinition<NpgsqlConnection, NpgsqlParameter>(connectionString),
                _ => null,
            };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void Cb_GeneratorMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = cb_GeneratorMode.SelectedIndex;
            if (cb_TargetLang != null)
            {
                cb_TargetLang.Items.Clear();
                IEnumerable<(int Index, string Name, bool IsModelGenerator, bool IsControllerGenerator)> supportedLanguages = ModelGenerator.Core.Refined.Helper.EnumHelper.Expand<SupportLanguage>();

                switch (selectedIndex)
                {
                    case 0: //model generator
                        mode = SupportMode.Model;
                        break;
                    case 1: //unit of work generator
                        mode = SupportMode.Service;
                        supportedLanguages = supportedLanguages.Where(x => x.IsModelGenerator);
                        break;
                    case 2: //controller generator
                        throw new NotImplementedException();
                        //generatorType = TargetGeneratorType.Controller;
                        supportedLanguages = supportedLanguages.Where(x => x.IsControllerGenerator);
                        break;
                }
                foreach (var language in supportedLanguages)
                {
                    cb_TargetLang.Items.Add(language.Name);
                }
                cb_TargetLang.SelectedIndex = 0;
            }
        }
        private void Cb_TargetLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            language = (SupportLanguage)cb_TargetLang.SelectedIndex;
        }

        private void Cb_TargetDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            database = (SupportDatabase)cb_TargetDatabase.SelectedIndex;
            switch (database)
            {
                case SupportDatabase.SQLServer:
                    txt_ConnectionString.Text = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password = myPassword;";
                    break;
                case SupportDatabase.Oracle:
                    txt_ConnectionString.Text = "Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;";
                    break;
                case SupportDatabase.MySQL:
                    txt_ConnectionString.Text = "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
                    break;
                case SupportDatabase.PostgreSQL:
                    txt_ConnectionString.Text = "Server=myServerAddress;Port=5432;Database=myDataBase;User Id=myUsername;Password = myPassword;";
                    break;
            }
        }
    }
}
