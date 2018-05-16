using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Archilizer_Purge
{
    internal class RemoveFilter
    {
        private ExternalCommandData _commandData;
        private OverrideGraphicSettings _defaultOverrides;

        private static List<ElementId> all = new List<ElementId>();
        private static List<ElementId> used = new List<ElementId>();
        private static List<ElementId> unused = new List<ElementId>();
        private static List<ElementId> unassigned = new List<ElementId>();
        private int removed;

        private static UIApplication uiapp;
        private static UIDocument uidoc;
        private static Autodesk.Revit.ApplicationServices.Application app;
        private static Document doc;

        public bool removeFromView { get; set; }

        /// <summary>
        /// Constructor. Takes the external common data (of the Revit app)
        /// </summary>
        /// <param name="data"></param>
        public RemoveFilter(ExternalCommandData data)
        {
            _commandData = data;
            _defaultOverrides = new OverrideGraphicSettings();

            uiapp = _commandData.Application;
            uidoc = uiapp.ActiveUIDocument;
            app = uiapp.Application;
            doc = uidoc.Document;

            Initialize();
        }
        /// <summary>
        /// Get all the infroamtion that we need to procede
        /// All unused filters, remove all unused on views
        /// </summary>
        private void Initialize()
        {
            GetUsedFilters();
            GetUnusedFilters();
            GetAllFilters();
        }
        /// <summary>
        /// Delete unused filters main method
        /// </summary>
        public void DeleteUnusedFilters()
        {            
            using (Transaction t = new Transaction(doc, "Delete filters"))
            {
                t.Start();
                doc.Delete(unassigned);
                t.Commit();
            }
            TaskDialog.Show("Unused Filters.", "Unused Filters:" + Environment.NewLine + unassigned.Count.ToString() + " Filters were removed.");
                
        }
        /// <summary>
        /// Returns all the used filters in a project
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private void GetUsedFilters()
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> views = collector.OfClass(typeof(Autodesk.Revit.DB.View)).ToElements();

            foreach (Element el in views)
            {
                Autodesk.Revit.DB.View v = el as Autodesk.Revit.DB.View;
                ICollection<ElementId> filterIds;
                try
                {
                    filterIds = v.GetFilters();
                }
                catch (AmbiguousMatchException)
                {
                    continue;
                }
                catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                {
                    continue;
                }
                foreach (ElementId id in filterIds)
                {
                    OverrideGraphicSettings filterOverrides = v.GetFilterOverrides(id);
                    bool filterVisible = v.GetFilterVisibility(id);

                    if (CompareOverrides(filterOverrides, _defaultOverrides) && filterVisible)
                    {
                        if (removeFromView)
                        {
                            using (Transaction t = new Transaction(doc, "Remove filter"))
                            {
                                t.Start();
                                v.RemoveFilter(id);
                                t.Commit();
                            }
                            removed++;
                            continue;
                        }
                        else
                        {
                            unused.Add(id);
                        }
                    }
                    used.Add(id);
                }
            }
            if (removeFromView) TaskDialog.Show("Unused on Views Filters", "Unused filters on views" + Environment.NewLine + removed.ToString() + " filters removed.");
        }
        /// <summary>
        /// If false, the filter is different then a default one so it is in use
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        private bool CompareOverrides(OverrideGraphicSettings o1, OverrideGraphicSettings o2)
        {
            if (o1.CutFillColor.IsValid) return false;
            if (o1.CutFillPatternId != o2.CutFillPatternId) return false;
            if (o1.CutLineColor.IsValid) return false;
            if (o1.CutLinePatternId != o2.CutLinePatternId) return false;
            if (o1.DetailLevel != o2.DetailLevel) return false;
            if (o1.Halftone) return false;
            if (o1.IsCutFillPatternVisible != o2.IsCutFillPatternVisible) return false;
            if (o1.IsProjectionFillPatternVisible != o2.IsProjectionFillPatternVisible) return false;
            if (o1.ProjectionFillColor.IsValid) return false;
            if (o1.ProjectionFillPatternId != o2.ProjectionFillPatternId) return false;
            if (o1.ProjectionLineColor.IsValid) return false;
            if (o1.ProjectionLinePatternId != o2.ProjectionLinePatternId) return false;
            if (o1.ProjectionLineWeight != o2.ProjectionLineWeight) return false;
            if (o1.Transparency != o2.Transparency) return false;
            return true;
        }
        /// <summary>
        /// Returns the unused filters in a projects as all filters minus used filters
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private void GetUnusedFilters()
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            if(used.Count > 0)
            {
                unassigned = collector.OfClass(typeof(ParameterFilterElement)).Excluding(used).ToElementIds().ToList();
            }
            if (unused.Count > 0) unassigned = unassigned.Except(unused).ToList();
        }
        /// <summary>
        /// Returns all filters
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private void GetAllFilters()
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            all = collector.OfClass(typeof(ParameterFilterElement)).ToElementIds().ToList();
        }
    }
}
