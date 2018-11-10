#region Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace Archilizer_Purge
{
    class App : IExternalApplication
    {
        static void AddRibbonPanel(UIControlledApplication application)
        {
            // Create a custom ribbon panel
            String tabName = "Archilizer";
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch (Exception)
            {

            }
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Purge +");

            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            PushButtonData delBackupsBtn = CreatePushButton("Delete Backups", "1. Delete Backups", thisAssemblyPath, "Archilizer_Purge.CommandDeleteBackups",
                "Deletes those pesky .00??.rvt files.", "purge.png", "purge_small.png");

            PushButtonData dwgBtn = CreatePushButton("Imported DWG Files", "2. Imported DWG Files", thisAssemblyPath, "Archilizer_Purge.PurgeImportedDWG", 
                "Purges imported (but not linked) DWG files in the current project.", "purge.png", "purge_small.png");

            PushButtonData dwgLinesBtn = CreatePushButton("DWG Line Patterns", "3. DWG Line Patterns", thisAssemblyPath, "Archilizer_Purge.PurgeImportedLinePatterns",
                "Purges line patterns brought by imported DWG files.", "purge.png", "purge_small.png");

            PushButtonData viewsBtn = CreatePushButton("Views not on Sheets", "4. Views not on Sheets", thisAssemblyPath, "Archilizer_Purge.CommandViewsNotOnSheets",
                "Purges Views not placed on Sheets. Use at your own risk!", "purge.png", "purge_small.png");

            PushButtonData vtemplateBtn = CreatePushButton("Unused View Templates", "5. Unused View Templates", thisAssemblyPath, "Archilizer_Purge.PurgeUnusedTemplates",
                "Purges Unused View Templates.", "purge.png", "purge_small.png");

            PushButtonData filtersBtn = CreatePushButton("Unused Filters", "6. Unused Filters", thisAssemblyPath, "Archilizer_Purge.PurgeFilters",
                "Purges Unused Filters (Filters that have never been assigned to a View or a View Template).", "purge.png", "purge_small.png");

            PulldownButtonData pd1 = new PulldownButtonData("purgeButton", "Purge");
            BitmapImage pdImage = new BitmapImage(new Uri(String.Format("pack://application:,,,/Archilizer_Purge;component/Resources/{0}", "purge.png")));
            pd1.LargeImage = pdImage;
            pd1.Image = pdImage;
            PulldownButton pd = ribbonPanel.AddItem(pd1) as PulldownButton;

            pd.AddPushButton(delBackupsBtn);
            pd.AddPushButton(dwgBtn);
            pd.AddPushButton(dwgLinesBtn);
            pd.AddPushButton(viewsBtn);
            pd.AddPushButton(vtemplateBtn);
            pd.AddPushButton(filtersBtn);
        }
        
        private static PushButtonData CreatePushButton(string text, string name, string path, string command, string tooltip, string icon, string iconSmall)
        {
            PushButtonData pbData = new PushButtonData(
                text,
                name,
                path,
                command);

            pbData.ToolTip = tooltip;

            return pbData;
        }
        
        public Result OnStartup(UIControlledApplication a)
        {
            // Make sure you have to update the plugin
            string version = a.ControlledApplication.VersionNumber;
            if(Int32.Parse(version) < 2019)
            {
                AddRibbonPanel(a);
            }
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
