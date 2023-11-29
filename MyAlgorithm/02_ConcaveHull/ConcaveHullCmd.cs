using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonClass;
using DebugUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_ConcaveHull
{
    /// <summary>
    /// 凹包
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class ConcaveHullCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            var regions = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_DetailComponents).OfClass(typeof(FilledRegion)).Cast<FilledRegion>().ToList();
            var pts = regions.Select(p => (p.GetBoundaries()[0].ToList()[0] as Arc).Center).ToList();

            var vetexs = pts.Select(p => new Point(p.X, p.Y)).ToList();
            ConcaveHull ball = new ConcaveHull(vetexs);
            double radis = 20000.0.MMToFeet();
            var result = ball.Compute(radis).Select(p => new XYZ(p.X, p.Y, 0)).ToList();

            List<Line> lines = new List<Line>();
            for (int i = 0; i < result.Count-1; i++)
            {
                Line ll = Line.CreateBound(result[i], result[i + 1]);
                lines.Add(ll);
            }

            doc.DrawDebugCurves(lines);


            return Result.Succeeded;
        }
    }
}
