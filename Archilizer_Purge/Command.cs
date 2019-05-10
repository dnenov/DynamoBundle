#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Linq;
#endregion

namespace Archilizer_Purge
{
    /// <summary>
    /// Deletes those pesky *.0023.rvt files ..
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CommandDeleteBackups : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            DeleteBackups.Delete();
            DeleteBackups.DisposeInstance();
            return Result.Succeeded;
        }
    }
    /// <summary>
    /// Purge Unused Filters
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class PurgeFilters : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            RemoveFilter removeFilter = new RemoveFilter(commandData);
            // Invoke the removefilter method
            removeFilter.DeleteUnusedFilters();


            return Result.Succeeded;
        }
    }
    /// <summary>
    /// Purges Unused View Templates
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class PurgeUnusedTemplates : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            Archilizer_Purge.RemoveTemplates removeTempaltes = new Archilizer_Purge.RemoveTemplates(commandData);
            // Invoke the removeViewTemplates method
            removeTempaltes.removeViewTemplates();

            return Result.Succeeded;
        }
    }

    /// <summary>
    /// Purges Imported Line Patterns
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class PurgeImportedLinePatterns : IExternalCommand
    {
        public Result Execute(
        ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);

            List<ElementId> importedLinePatterns = collector
                .OfClass(typeof(LinePatternElement))
                .Cast<LinePatternElement>()
                .Where(x => x.Name.Contains("IMPORT"))
                .Select(x => x.Id)
                .ToList();

            using (Transaction t = new Transaction(doc, "Delete Imported Line Patterns"))
            {
                t.Start();
                string s = doc.Delete(importedLinePatterns).Count.ToString();
                t.Commit();

                TaskDialog.Show("Success", String.Format("{0} Imported Line Patterns deleted.", s));
            }

            return Result.Succeeded;
        }
    }
    /// <summary>
    /// Deletes views not on sheets
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CommandViewsNotOnSheets : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            bool permission = false;

            List<ViewSheet> sheets = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .WhereElementIsNotElementType()
                .Cast<ViewSheet>()
                .ToList();

            List<ElementId> usedViews = new List<ElementId>();

            foreach (var sheet in sheets)
            {
                usedViews.AddRange(sheet.GetAllPlacedViews().ToList());
            }

            usedViews = usedViews.Distinct().ToList();

            if (usedViews.Count == 0)
            {
                TaskDialog.Show("Error", "Cannot let that happen - at least one view needs to remain in the project. (No views are placed on sheets.)");
                return Result.Failed;
            }

            List<ElementId> delete = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Views)
                .OfClass(typeof(Autodesk.Revit.DB.View))
                .WhereElementIsNotElementType()
                .Cast<Autodesk.Revit.DB.View>()
                .Where(x => !x.IsTemplate)
                .Where(sc => (sc as ViewSchedule) == null)
                .Select(x => x.Id)
                .ToList();

            TaskDialog.Show("Warning", "Be careful with that though .. ");

            string msg = "You want to delete all those hard drawn views. You certain?";

            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(msg, "Are you really sure?", System.Windows.Forms.MessageBoxButtons.YesNo);

            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                permission = true;
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                permission = false;
            }

            if (!permission) return Result.Cancelled;

            using (Transaction t = new Transaction(doc, "Delete Views not on Sheets"))
            {
                t.Start();
                doc.Delete(delete.Except(usedViews).ToArray());
                t.Commit();
            }

            return Result.Succeeded;
        }
    }
    /// <summary>
    /// Purges imported DWGs
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class PurgeImportedDWG : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // Access current selection

            Selection sel = uidoc.Selection;

            // Retrieve elements from database

            FilteredElementCollector col
              = new FilteredElementCollector(doc);

            List<ImportInstance> imports = col.
                OfClass(typeof(ImportInstance))
                .Cast<ImportInstance>()
                .Where(x => !x.IsLinked)
                .ToList();

            if (imports.Count == 0)
            {
                TaskDialog.Show("Error", "No imported DWGs..");

                return Result.Failed;
            }

            List<ImportedDWG> dwg = new List<ImportedDWG>();

            foreach (ImportInstance instanace in imports)
            {
                dwg.Add(new ImportedDWG
                {
                    id = instanace.Id,
                    name = instanace.LookupParameter("Name").AsString(),
                    view = doc.GetElement(instanace.OwnerViewId) as View,
                    uniqueId = instanace.UniqueId.ToString()
                });
            }

            using (Archilizer_Purge.ImportedDWGForm myform = new Archilizer_Purge.ImportedDWGForm(uidoc, dwg))
            {
                System.Windows.Forms.DialogResult result = myform.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    return Result.Succeeded;
                }
                else
                {
                    return Result.Failed;
                }
            }
        }
    }

    internal class ImportedDWG
    {
        internal string uniqueId
        {
            get; set;
        }
        internal ElementId id
        {
            get; set;
        }
        internal string name
        {
            get; set;
        }
        internal View view
        {
            get; set;
        }
        internal ImportedDWG()
        {

        }
    }
}