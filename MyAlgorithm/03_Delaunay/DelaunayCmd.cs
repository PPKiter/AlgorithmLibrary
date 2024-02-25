using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Delaunay
{
    [Transaction(TransactionMode.Manual)]
    internal class DelaunayCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            var regions = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_DetailComponents).OfClass(typeof(FilledRegion)).Cast<FilledRegion>().ToList();
            var pts = regions.Select(p => (p.GetBoundaries()[0].ToList()[0] as Arc).Center).ToList();

            var vetexs = pts.Select(p => new Point(p.X, p.Y)).ToList();
            DelaunayAlgo del = new DelaunayAlgo();
            var trians= del.Triangulate(vetexs);

            List<Line> results = new List<Line>();
            foreach (var tri in trians)
            {
                XYZ p1 = new XYZ(tri.Vertex1.X, tri.Vertex1.Y, 0);
                XYZ p2 = new XYZ(tri.Vertex2.X, tri.Vertex2.Y, 0);
                XYZ p3 = new XYZ(tri.Vertex3.X, tri.Vertex3.Y, 0);

                Line l1=Line.CreateBound(p1, p2);
                Line l2 =Line.CreateBound(p2, p3);
                Line l3=Line.CreateBound(p3, p1);
                results.Add(l1);
                results.Add(l2);
                results.Add(l3);
            }
            doc.DrawDebugCurves(results);

            //ConcaveHull ball = new ConcaveHull(vetexs);
            //double radis = 20000.0.MMToFeet();
            //var result = ball.Compute(radis).Select(p => new XYZ(p.X, p.Y, 0)).ToList();

            //List<Line> lines = new List<Line>();
            //for (int i = 0; i < result.Count - 1; i++)
            //{
            //    Line ll = Line.CreateBound(result[i], result[i + 1]);
            //    lines.Add(ll);
            //}

            //doc.DrawDebugCurves(lines);


            return Result.Succeeded;
        }
    }
}
