using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using ModelGenerator.Helpers;
using NuGet;
using NuGet.VisualStudio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ModelGenerator.NugetHelper
{
    public static class NugetHelper
    {
        private static readonly IComponentModel componentModel;
        static NugetHelper()
        {
            componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
        }
        public static bool IsNugetPackageInstalled(EnvDTE.Project project, string packageId)
        {
            IVsPackageInstallerServices installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            return installerServices.IsPackageInstalled(project, packageId);
        }
        public static bool InstallNugetPackage(EnvDTE.Project project, string packageId)
        {
            bool installedPkg = true;
            try
            {
                if (!IsNugetPackageInstalled(project, packageId))
                {
                    var installer = componentModel.GetService<IVsPackageInstaller>();
                    installer.InstallPackage("All", project, packageId, (System.Version)null, false);
                }
            }
            catch 
            {
                installedPkg = false;
            }
            return installedPkg;
        }
        //public static void InstallNugetPackage(string packageId, string packageRepositoryUri)
        //{
        //    var csprojFile = Directory.GetFiles(DevenvHelper.ProjectDirectory, "*.csproj").FirstOrDefault();
        //    if (csprojFile == null) return;
        //    var csprojContent = File.ReadAllText(csprojFile);
        //    var CsprojDoc = new XmlDocument();
        //    CsprojDoc.LoadXml(csprojFile);
        //    var Nsmgr = new XmlNamespaceManager(CsprojDoc.NameTable);
        //    Nsmgr.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

        //    IPackage packageInfos = GetNugetPackage(packageId, new Uri(packageRepositoryUri));

        //    XmlNode referenceNode = CsprojDoc.CreateNode(XmlNodeType.Element, "Reference", DevenvHelper.ProjectDefaultNamespace);
        //    XmlAttribute includeAttribute = CsprojDoc.CreateAttribute("Include");

        //    var targetFwProfile = CsprojDoc.SelectSingleNode("//x:TargetFrameworkProfile", Nsmgr);
        //    string targetFrameworkProfile = string.Empty;
        //    if (!string.IsNullOrEmpty(targetFwProfile?.InnerXml))
        //    {
        //        targetFrameworkProfile = targetFwProfile.InnerXml;
        //    }

        //    var targetFwAttribute = GetTargetFrameworkFromCsproj(CsprojDoc, Nsmgr);
        //    Regex p = new Regex(@"\d+(\.\d+)+");
        //    Match m = p.Match(targetFwAttribute.FrameworkName);
        //    Version targetFwVersion = Version.Parse(m.Value);

        //    // Get the package's assembly reference matching the target framework from the given '.csproj'.
        //    var assemblyReference =
        //        packageInfos.AssemblyReferences
        //            .Where(a => a.TargetFramework.Identifier.Equals(targetFwAttribute.FrameworkName.Split(',').First()))
        //            .Where(a => a.TargetFramework.Profile.Equals(targetFrameworkProfile))
        //            .Last(a => (a.TargetFramework.Version.Major.Equals(targetFwVersion.Major) && a.TargetFramework.Version.Minor.Equals(targetFwVersion.Minor)) ||
        //            a.TargetFramework.Version.Major.Equals(targetFwVersion.Major));

        //    DownloadNugetPackage(packageInfos.Id, new Uri(packageRepositoryUri), DevenvHelper.LocalPackagesDirectory, packageInfos.Version.ToFullString());

        //    string dllAbsolutePath = Path.GetFullPath($"{DevenvHelper.LocalPackagesDirectory}\\{packageInfos.GetFullName().Replace(' ', '.')}\\{assemblyReference.Path}");
        //    var assemblyInfos = Assembly.LoadFile(dllAbsolutePath);

        //    includeAttribute.Value = $"{assemblyInfos.FullName}, processorArchitecture=MSIL";

        //    referenceNode.Attributes.Append(includeAttribute);

        //    XmlNode hintPathNode = CsprojDoc.CreateNode(XmlNodeType.Element, "HintPath", DevenvHelper.ProjectDefaultNamespace);
        //    XmlNode privateNode = CsprojDoc.CreateNode(XmlNodeType.Element, "Private", DevenvHelper.ProjectDefaultNamespace);

        //    hintPathNode.InnerXml = $"$(SolutionDir)\\packages\\{assemblyReference.Path}";
        //    privateNode.InnerXml = "True";

        //    referenceNode.AppendChild(hintPathNode);
        //    referenceNode.AppendChild(privateNode);
        //    var itemGroupNode = CsprojDoc.SelectSingleNode("//x:Project/x:ItemGroup/x:Reference", Nsmgr).ParentNode;
        //    itemGroupNode.AppendChild(referenceNode);
        //}
        //public static IPackage GetNugetPackage(string packageId, Uri repoUri, string version = null)
        //{
        //    IPackageRepository packageRepository = PackageRepositoryFactory.Default.CreateRepository(repoUri.ToString());
        //    IPackage package;

        //    if (!string.IsNullOrEmpty(version))
        //    {
        //        package = packageRepository.FindPackagesById(packageId).SingleOrDefault(p => p.Version.ToFullString().Equals(version));
        //    }
        //    else
        //    {
        //        package = packageRepository.FindPackagesById(packageId).SingleOrDefault(p => p.IsLatestVersion);
        //    }

        //    return package;
        //}
        //public static TargetFrameworkAttribute GetTargetFrameworkFromCsproj(XmlDocument CsprojDoc, XmlNamespaceManager Nsmgr)
        //{
        //    XmlNode targetFrameworkNode = CsprojDoc.SelectSingleNode("//x:TargetFrameworkVersion", Nsmgr);
        //    return new TargetFrameworkAttribute($".NETFramework, Version={targetFrameworkNode.InnerXml}");
        //}
        //private static void DownloadNugetPackage(string packageId, Uri repoUri, string packagesFolderPath, string version)
        //{
        //    IPackageRepository packageRepository = PackageRepositoryFactory.Default.CreateRepository(repoUri.ToString());
        //    PackageManager packageManager = new PackageManager(packageRepository, packagesFolderPath);

        //    packageManager.InstallPackage(packageId, SemanticVersion.Parse(version));
        //}
    }
}
