using ModelGenerator.Core.AttributeHelper;
using ModelGenerator.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelGenerator
{
    public partial class GeneratorForm : Form
    {
        public event EventHandler OnSubmitGenerate;
        public string ConnectionString => this.txt_ConnectionString.Text;
        public TargetGeneratorType GeneratorType { get; private set; }
        public TargetLanguage TargetLanguage { get; private set; }
        public TargetDatabaseConnector TargetDatabaseConnector { get; private set; }
        public bool AutomaticReload => this.cb_AutoReload.Checked;
        public GeneratorForm()
        {
            InitializeComponent();
            var supportedDatabase = typeof(TargetDatabaseConnector).GetMembers(BindingFlags.Static | BindingFlags.Public);
            foreach (var database in supportedDatabase)
            {
                if (!(database.GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute description))
                {
                    cb_TargetDatabase.Items.Add(database.Name);
                }
                else
                {
                    cb_TargetDatabase.Items.Add(description.Description);
                }
            }

            cb_GeneratorMode.SelectedIndex = 1;
            cb_TargetLang.SelectedIndex = 0;
            cb_TargetDatabase.SelectedIndex = 0;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OnSubmitGenerate.Invoke(this, e);
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
                IEnumerable<(int Index, string Name, bool IsModelGenerator, bool IsControllerGenerator)> supportedLanguages = ModelGenerator.Core.Helpers.EnumHelper.Expand<TargetLanguage>();

                switch (selectedIndex)
                {
                    case 0: //model generator
                        GeneratorType = TargetGeneratorType.Model;
                        break;
                    case 1: //unit of work generator
                        GeneratorType = TargetGeneratorType.UnitOfWork;
                        supportedLanguages = supportedLanguages.Where(x => x.IsModelGenerator);
                        break;
                    case 2: //controller generator
                        GeneratorType = TargetGeneratorType.Controller;
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
            TargetLanguage = (TargetLanguage)cb_TargetLang.SelectedIndex;
        }

        private void Cb_TargetDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            TargetDatabaseConnector = (TargetDatabaseConnector)cb_TargetDatabase.SelectedIndex;
            switch (TargetDatabaseConnector)
            {
                case TargetDatabaseConnector.SQLServer:
                    txt_ConnectionString.Text = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password = myPassword;";
                    break;
                case TargetDatabaseConnector.Oracle:
                    txt_ConnectionString.Text = "Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;";
                    break;
                case TargetDatabaseConnector.MySQL:
                    txt_ConnectionString.Text = "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
                    break;
                case TargetDatabaseConnector.PostgreSQL:
                    txt_ConnectionString.Text = "Server=myServerAddress;Port=5432;Database=myDataBase;User Id=myUsername;Password = myPassword;";
                    break;
                case TargetDatabaseConnector.SQLite:
                    txt_ConnectionString.Text = @"Data Source=C:/path/to/yourdb.db;Version=3;";
                    break;
            }
        }
    }
}
