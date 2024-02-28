using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace _04_Backtrack
{
    [Transaction(TransactionMode.Manual)]
    public class BacktrackCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            var regions = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_DetailComponents).OfClass(typeof(FilledRegion)).Cast<FilledRegion>().ToList();

            var g1 = regions.Where(p => p.get_Parameter(BuiltInParameter.DOOR_NUMBER).AsString().Contains("1")).ToList();
            var g2 = regions.Where(p => p.get_Parameter(BuiltInParameter.DOOR_NUMBER).AsString().Contains("2")).ToList();
            var g3 = regions.Where(p => p.get_Parameter(BuiltInParameter.DOOR_NUMBER).AsString().Contains("3")).ToList();

            var pts1 = g1.Select(p => (p.GetBoundaries()[0].ToList()[0] as Arc).Center).ToList();
            var pts2 = g2.Select(p => (p.GetBoundaries()[0].ToList()[0] as Arc).Center).ToList();
            var pts3 = g3.Select(p => (p.GetBoundaries()[0].ToList()[0] as Arc).Center).ToList();


            List<List<Line>> groups = new List<List<Line>>();
            foreach (var p1 in pts1)
            {
                List<Line> group1 = new List<Line>();
                foreach (var p2 in pts2)
                {
                    Line ll = Line.CreateBound(p1, p2);
                    group1.Add(ll);
                }
                groups.Add(group1);
            }
            ////BacktrackAlgo<Line> algo = new BacktrackAlgo<Line>(groups, IsConflict);
            BacktrackAlgo<Line> algo = new BacktrackAlgo<Line>(groups, IsConflict);
            algo.Backtrack(0);
            doc.DrawDebugCurves(algo.Results[0]);


            return Result.Succeeded;
        }

        public bool IsConflict(Line l1, Line l2)
        {
            l1.Intersect(l2, out IntersectionResultArray results);
            //没有交集，不冲突
            if (results == null || results.IsEmpty)
            {
                return false;
            }
            ////有交集，但是交点为自身端点，不冲突
            //if (results.Size == 1)
            //{
            //    var insect = results.get_Item(0).XYZPoint;
            //    var isEnd = insect.IsAlmostEqualTo(l1.GetEndPoint(0), 1e-3) || insect.IsAlmostEqualTo(l1.GetEndPoint(1), 1e-3)
            //        || insect.IsAlmostEqualTo(l2.GetEndPoint(0), 1e-3) || insect.IsAlmostEqualTo(l2.GetEndPoint(1), 1e-3);
            //    if (isEnd)
            //    {
            //        return false;
            //    }
            //}
            return true;
        }
    }
}
