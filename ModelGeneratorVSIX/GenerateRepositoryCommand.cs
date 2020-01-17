using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ModelGenerator.Core.Refined.Entity;
using ModelGenerator.Core.Refined.Enum;
using ModelGenerator.Core.Refined.Helper;
using ModelGenerator.Helpers;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.ComponentModel.Design;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

namespace ModelGenerator
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GenerateRepositoryCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;
        public const int CommandId2 = 0x0101;
        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("afdef772-1399-4bab-801b-f635202e043e");
        public static readonly Guid CommandSet2 = new Guid("e9cef5d6-fbe3-4435-bf4f-84b6a4534fe4");
        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateRepositoryCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private GenerateRepositoryCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static GenerateRepositoryCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in GenerateRepository's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new GenerateRepositoryCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!NugetHelper.NugetHelper.IsNugetPackageInstalled(DevenvHelper.SelectedProject, "Deszolate.Utilities.Lite"))
            {
                var acceptLibraryInstaller = MessageBox.Show(
                        $"{DevenvHelper.ProjectName} currently not install 'Deszolate.Utilities.Lite'. This library is required in order to make the generated code works, do you want to install now?",
                        "ModelGenerator",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                if (acceptLibraryInstaller == DialogResult.Yes)
                {
                    NugetHelper.NugetHelper.InstallNugetPackage(DevenvHelper.SelectedProject, "Deszolate.Utilities.Lite");
                }
            }
            var form = new GeneratorForm(DevenvHelper.ProjectDefaultNamespace, DevenvHelper.ProjectDirectory);
            //form.OnSubmitGenerate += delegate
            //{
            //    var genType = form.GeneratorType;
            //    var targetLang = form.TargetLanguage;
            //    var targetDb = form.TargetDatabaseConnector;
            //    var dbCon = form.ConnectionString;
            //    var autoReload = form.AutomaticReload;

            //    form.Dispose();
            //    if (!NugetHelper.NugetHelper.IsNugetPackageInstalled(DevenvHelper.SelectedProject, "Deszolate.Utilities.Lite"))
            //    {
            //        var acceptLibraryInstaller = MessageBox.Show(
            //                $"{DevenvHelper.ProjectName} currently not install 'Deszolate.Utilities.Lite'. This library is required in order to make the generated code works, do you want to install now?",
            //                "ModelGenerator",
            //                MessageBoxButtons.YesNo,
            //                MessageBoxIcon.Warning
            //            );
            //        if (acceptLibraryInstaller == DialogResult.Yes)
            //        {
            //            NugetHelper.NugetHelper.InstallNugetPackage(DevenvHelper.SelectedProject, "Deszolate.Utilities.Lite");
            //        }
            //    }
            //    var thread = new System.Threading.Thread(
            //    new System.Threading.ThreadStart(() => this.GenerateService(genType, targetLang, targetDb, dbCon, autoReload)));
            //    thread.Start();
            //    var messageResult = MessageBox.Show("Generator now working on background thread, your files will soon generated.", "ModelGenerator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //};
            form.Show();
        }
        private void GenerateService(SupportMode mode, SupportLanguage language, SupportDatabase database, string connectionString, bool reloadProject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    VsShellUtilities.ShowMessageBox(
                        this.package,
                        "Connection string must not be null.",
                        "ModelGenerator",
                        OLEMSGICON.OLEMSGICON_INFO,
                        OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                    return;
                }
                var dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;

                var directory = DevenvHelper.ProjectDirectory;
                var solutionName = dte2.GetSolutionName();
                var projectName = DevenvHelper.ProjectName;
                var defaultNamespace = DevenvHelper.ProjectDefaultNamespace;

                dte2.Windows.Item(EnvDTE.Constants.vsWindowKindSolutionExplorer).Activate();
                dte2.ToolWindows.SolutionExplorer.GetItem($@"{solutionName}\{projectName}").Select(vsUISelectionType.vsUISelectionTypeSelect);

                //if (reloadProject) dte2.ExecuteCommand("Project.ReloadProject");
                MessageBox.Show($"Successfully generate services file for {projectName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
