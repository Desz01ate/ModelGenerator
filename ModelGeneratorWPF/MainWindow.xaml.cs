using Microsoft.WindowsAPICodePack.Dialogs;
using ModelGenerator.Core.AttributeHelper;
using ModelGenerator.Core.Enum;
using ModelGenerator.Core.Factory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private TargetGeneratorType generatorType { get; set; }
        private TargetLanguage targetLanguage { get; set; }
        private TargetDatabaseConnector targetDatabaseConnector { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            #region initializer
            cb_GeneratorMode.SelectedIndex = 1;
            cb_GeneratorMode.SelectedIndex = 0;
            var supportedDatabase = ModelGenerator.Core.Helpers.EnumHelper.Expand<TargetDatabaseConnector>();
            foreach (var database in supportedDatabase)
            {
                cb_TargetDatabase.Items.Add(database.Name);
            }
            #endregion
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedIndex = cb_GeneratorMode.SelectedIndex;
            if (cb_TargetLang != null)
            {
                cb_TargetLang.Items.Clear();
                IEnumerable<(int Index, string Name, bool IsModelGenerator)> supportedLanguages = ModelGenerator.Core.Helpers.EnumHelper.Expand<TargetLanguage>();
                switch (selectedIndex)
                {
                    case 0: //model generator
                        generatorType = TargetGeneratorType.Model;
                        break;
                    case 1: //unit of work generator
                        generatorType = TargetGeneratorType.UnitOfWork;
                        supportedLanguages = supportedLanguages.Where(x => x.IsModelGenerator);
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
            targetLanguage = (TargetLanguage)cb_TargetLang.SelectedIndex;
        }

        private void Cb_TargetDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            targetDatabaseConnector = (TargetDatabaseConnector)cb_TargetDatabase.SelectedIndex;
            switch (targetDatabaseConnector)
            {
                case TargetDatabaseConnector.SQLServer:
                    txt_connectionString.Text = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password = myPassword;";
                    break;
                case TargetDatabaseConnector.Oracle:
                    txt_connectionString.Text = "Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;";
                    break;
                case TargetDatabaseConnector.MySQL:
                    txt_connectionString.Text = "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
                    break;
                case TargetDatabaseConnector.PostgreSQL:
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
            btn_Generate.Content = "Generating...";
            try
            {
                switch (generatorType)
                {
                    case TargetGeneratorType.Model:
                        GeneratorFactory.PerformModelGenerate(targetLanguage, targetDatabaseConnector, txt_connectionString.Text, outputDir, txt_namespace.Text);
                        break;
                    case TargetGeneratorType.UnitOfWork:
                        GeneratorFactory.PerformRepositoryGenerate(targetLanguage, targetDatabaseConnector, txt_connectionString.Text, outputDir, txt_namespace.Text);
                        break;
                }
                Process.Start("explorer.exe", outputDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            btn_Generate.Content = "Generate";
            btn_Generate.IsEnabled = true;
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
    }
}
