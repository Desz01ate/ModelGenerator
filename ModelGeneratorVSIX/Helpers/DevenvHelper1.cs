using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ModelGenerator.Helpers
{
    public static class DevenvHelper
    {
        public static Project SelectedProject { get; private set; }
        public static string ProjectDirectory => Path.GetDirectoryName(SelectedProject?.FullName);
        public static string LocalPackagesDirectory => Path.Combine(ProjectDirectory, "TempPackages");
        public static string ProjectName => SelectedProject?.Name;
        public static string ProjectDefaultNamespace => SelectedProject?.Properties?.Item("DefaultNamespace")?.Value?.ToString();
        static DevenvHelper()
        {
            SelectedProject = GetSelectedProject();
        }
        private static Project GetSelectedProject()
        {
            var monitorSelection = Package.GetGlobalService(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;

            monitorSelection.GetCurrentSelection(out IntPtr hierarchyPointer,
                                                 out uint projectItemId,
                                                 out IVsMultiItemSelect multiItemSelect,
                                                 out IntPtr selectionContainerPointer);

            IVsHierarchy selectedHierarchy = Marshal.GetTypedObjectForIUnknown(
                                                 hierarchyPointer,
                                                 typeof(IVsHierarchy)) as IVsHierarchy;
            object selectedObject = null;
            if (selectedHierarchy != null)
            {
                ErrorHandler.ThrowOnFailure(selectedHierarchy.GetProperty(
                                                  projectItemId,
                                                  (int)__VSHPROPID.VSHPROPID_ExtObject,
                                                  out selectedObject));
            }
            Project selectedProject = selectedObject as Project;
            return selectedProject;
        }

        public static string GetSolutionName(this DTE2 dte)
        {
            return Path.GetFileNameWithoutExtension(dte.Solution.FullName);
        }
        public static void UnloadProject(this DTE2 dte)
        {
            dte.ExecuteCommand("Project.UnloadProject");
        }
        public static void ReloadProject(this DTE2 dte)
        {
            dte.ExecuteCommand("Project.ReloadProject");
        }
    }
}
