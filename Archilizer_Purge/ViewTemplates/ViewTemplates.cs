using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archilizer_Purge
{
    public static class Utils
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
    /// <summary>
    /// Remove Unused View Templates
    /// </summary>
    internal class RemoveTemplates
    {
        private ExternalCommandData _commandData;
        /// <summary>
        /// Constructor. Takes the external common data (of the Revit app)
        /// </summary>
        /// <param name="data"></param>
        public RemoveTemplates(ExternalCommandData data)
        {
            _commandData = data;
        }

        public void removeViewTemplates()
        {
            Document doc = this._commandData.Application.ActiveUIDocument.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<View> views = collector.OfClass(typeof(View)).Cast<View>().Where(x => !x.IsTemplate).ToList();
            List<ElementId> usedTemplateIds = collector.OfClass(typeof(View)).Cast<View>().Where(x => !x.IsTemplate).Select(x => x.ViewTemplateId).ToList();
            List<ElementId> allTemplateIds = collector.OfClass(typeof(View)).Cast<View>().Where(x => x.IsTemplate).Select(x => x.Id).ToList();
            List<ElementId> unusedTemplateIds = allTemplateIds.Except(usedTemplateIds).ToList();
                        
            using (Transaction t = new Transaction(doc, "Delete filters"))
            {
                t.Start();
                doc.Delete(unusedTemplateIds);
                t.Commit();
            }
            TaskDialog.Show("Unused View Templates.", "Unused View Templates:" + Environment.NewLine + unusedTemplateIds.Count.ToString() + " View Templates were removed.");
               
        }
    }
}
