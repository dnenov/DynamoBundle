#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Linq;
#endregion

namespace DynamoBundle
{
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

            if(imports.Count == 0)
            {
                TaskDialog.Show("Error", "No imported DWGs..");

                return Result.Failed;
            }

            List<ImportedDWG> dwg = new List<ImportedDWG>();

            foreach(ImportInstance instanace in imports)
            {
                dwg.Add(new ImportedDWG {
                    id = instanace.Id,
                    name = instanace.LookupParameter("Name").AsString(),
                    view = doc.GetElement(instanace.OwnerViewId) as View,
                    uniqueId = instanace.UniqueId.ToString()
                });
            }

            using (ImportedDWGForm myform = new ImportedDWGForm(uidoc, dwg))
            {
                System.Windows.Forms.DialogResult result = myform.ShowDialog();

                if(result == System.Windows.Forms.DialogResult.OK)
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
}
