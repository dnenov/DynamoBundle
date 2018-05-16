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

            PushButtonData dwgBtn = CreatePushButton("Purge Imported DWG Files", String.Format("Purge Imported" + Environment.NewLine + "DWG Files"), thisAssemblyPath, "Archilizer_Purge.PurgeImportedDWG", 
                "Purges imported (but not linked) DWG files in the current project.", "purge.png", "purge_small.png");

            PushButtonData dwgLinesBtn = CreatePushButton("Purge DWG Line Patterns", String.Format("Purge DWG" + Environment.NewLine + "Line Patterns"), thisAssemblyPath, "Archilizer_Purge.PurgeImportedLinePatterns",
                "Purges line patterns brought by imported DWG files.", "purge.png", "purge_small.png");

            PushButtonData viewsBtn = CreatePushButton("Purge Views not on Sheets", String.Format("Purge Views" + Environment.NewLine + "not on Sheets"), thisAssemblyPath, "Archilizer_Purge.CommandViewsNotOnSheets",
                "Purges Views not placed on Sheets. Use at your own risk!", "purge.png", "purge_small.png");


            SplitButtonData sb1 = new SplitButtonData("purgeButton", "Purge");
            SplitButton sb = ribbonPanel.AddItem(sb1) as SplitButton;
            sb.IsSynchronizedWithCurrentItem = true;

            sb.AddPushButton(dwgBtn);
            sb.AddPushButton(dwgLinesBtn);
            sb.AddPushButton(viewsBtn);
        }
        
        private static PushButtonData CreatePushButton(string text, string name, string path, string command, string tooltip, string icon, string iconSmall)
        {
            PushButtonData pbData = new PushButtonData(
                text,
                name,
                path,
                command);

            pbData.ToolTip = tooltip;
            BitmapImage pbLImage = new BitmapImage(new Uri(String.Format("pack://application:,,,/Archilizer_Purge;component/Resources/{0}", icon)));
            pbData.LargeImage = pbLImage;
            BitmapImage pbSImage = new BitmapImage(new Uri(String.Format("pack://application:,,,/Archilizer_Purge;component/Resources/{0}", iconSmall)));
            pbData.Image = pbSImage;

            return pbData;
        }
        
        public Result OnStartup(UIControlledApplication a)
        {
            AddRibbonPanel(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
