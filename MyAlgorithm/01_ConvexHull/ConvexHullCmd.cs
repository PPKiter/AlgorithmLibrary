using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DebugUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_ConvexHull
{
    [Transaction(TransactionMode.Manual)]
    internal class ConvexHullCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            var regions = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_DetailComponents).OfClass(typeof(FilledRegion)).Cast<FilledRegion>().ToList();
            var pts = regions.Select(p => (p.GetBoundaries()[0].ToList()[0] as Arc).Center).ToList();

            var vetexs = pts.Select(p => new Point(p.X, p.Y)).ToList();
            var result = ConvexHull.GrahamScan(vetexs).Select(p=>new XYZ(p.X,p.Y,0)).ToList();

            List<Line> lines = new List<Line>();
            for (int i = 0; i < result.Count; i++)
            {
                if (i == result.Count - 1)
                {
                    Line last = Line.CreateBound(result[result.Count - 1], result[0]);
                    lines.Add(last);
                    break;
                }
                Line ll= Line.CreateBound(result[i], result[i + 1]);
                lines.Add(ll);
            }

            doc.DrawDebugCurves(lines);


            return Result.Succeeded;
        }

    }
}
