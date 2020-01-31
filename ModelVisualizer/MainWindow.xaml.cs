using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using ModelGenerator.Core.Entity;
using ModelVisualizer.Entity;
using ModelVisualizer.Interface;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ModelVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ConnectionString => this.txt_ConnectionString.Text;
        private string Namespace => this.txt_Namespace.Text;
        private DatabaseDefinition databaseDefinition { get; set; }
        private TreeView mainTreeView => this.tv_MainTreeView;
        private DataGrid mainGridView => this.gv_MainGridView;
        private TablePreview previewable;
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.CanResizeWithGrip;
            //var item1 = new TreeViewItem();
            //item1.Header = "Test1";
            //item1.ItemsSource = new[] { "A", "B", "C" };
            //mainTreeView.Items.Add(item1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                databaseDefinition = ModelGenerator.Core.Helper.SqlDefinition.GetDatabaseDefinition<SqlConnection, SqlParameter>(ConnectionString, x => $"[{x}]");
                mainTreeView.Items.Clear();
                var tableTree = new TreeViewItem();
                tableTree.Header = "Tables";
                tableTree.ItemsSource = databaseDefinition.Tables.Select(x => new TablePreview(x));
                mainTreeView.Items.Add(tableTree);
                if (databaseDefinition.StoredProcedures.Any())
                {
                    var storedProcedureTree = new TreeViewItem();
                    storedProcedureTree.Header = "Stored Procedures";
                    storedProcedureTree.ItemsSource = databaseDefinition.StoredProcedures.Select(x => new TablePreview(x));
                    mainTreeView.Items.Add(storedProcedureTree);
                }
                btn_GenerateAllModels.IsEnabled = true;
                btn_GenerateService.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tv_MainTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = e.NewValue;
            if (selectedItem is TablePreview instance)
            {
                txt_PreviewContent.Text = instance.Content;
                this.previewable = instance;
                this.btn_GenerateCurrentModel.IsEnabled = true;
                this.mainGridView.ItemsSource = instance.Structure.DefaultView;
            }
            else
            {
                txt_PreviewContent.Text = "Please select sub-item category (table, store procedure item) to preview code.";
                this.previewable = null;
                this.btn_GenerateCurrentModel.IsEnabled = false;
                this.mainGridView.ItemsSource = null;
            }
        }

        private void btn_GenerateCurrentModel_Click(object sender, RoutedEventArgs e)
        {
            if (FolderPick(out var directory))
            {
                var code = previewable.Content;
                if (!string.IsNullOrWhiteSpace(Namespace))
                {
                    code = code.Replace("CustomNameSpace", Namespace);
                }
                var file = Path.Combine(directory, $"{previewable.Description}.cs");
                File.WriteAllText(file, code);
                MessageBox.Show($"File saved to {file}");
            }
        }
        private bool FolderPick(out string directory)
        {
            using var ofd = new CommonOpenFileDialog();
            ofd.IsFolderPicker = true;
            var result = ofd.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                directory = ofd.FileName;
                if (string.IsNullOrWhiteSpace(Namespace))
                {
                    var hopefullyNamespace = directory.Split(@"\").Last();
                    var namespaceReplace = MessageBox.Show($"We detected that you didn't specified the namespace, do you want to use '{hopefullyNamespace}' as a namespace?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (namespaceReplace == MessageBoxResult.Yes)
                    {
                        txt_Namespace.Text = hopefullyNamespace;
                    }
                }
            }
            else
            {
                directory = null;
            }
            return (result == CommonFileDialogResult.Ok) && !string.IsNullOrWhiteSpace(ofd.FileName);
        }
        private void btn_GenerateAllModels_Click(object sender, RoutedEventArgs e)
        {
            if (FolderPick(out var directory))
            {
                var generator = new ModelGenerator.Core.Builder.ModelBuilder(directory, this.Namespace, databaseDefinition);
                var template = ModelGenerator.Core.Provider.ModelProvider.CSharpModelProvider.Context;
                generator.Generate(template);
            }
        }

        private void btn_GenerateService_Click(object sender, RoutedEventArgs e)
        {
            if (FolderPick(out var directory))
            {
                var generator = new ModelGenerator.Core.Builder.ServiceBuilder(directory, this.Namespace, databaseDefinition);
                var template = ModelGenerator.Core.Provider.ServiceProvider.CSharpServiceProvider.Context;
                generator.Generate(template);
            }
        }
    }
}
