using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Geometry;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCvSharpTest
{
    [Transaction(TransactionMode.Manual)]
    public class OpenCvTest : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            var regions = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_DetailComponents).OfClass(typeof(FilledRegion)).Cast<FilledRegion>().ToList();
            var pts = regions.Select(p => (p.GetBoundaries()[0].ToList()[0] as Arc).Center).ToList();

            //var vetexs = pts.Select(p => new Point(p.X, p.Y)).ToList();
            //var result = ConvexHull.GrahamScan(vetexs).Select(p=>new XYZ(p.X,p.Y,0)).ToList();

            var cvPts = pts.Select(p => new OpenCvSharp.Point(p.X, p.Y)).ToList();
            List<XYZ> result = new List<XYZ>();
            try
            {
                var cvResult = Cv2.ConvexHull(cvPts);
                result = cvResult.Select(p => new XYZ(p.X, p.Y, 0)).ToList();
            }
            catch (Exception)
            {

            }
            List<Line> lines = new List<Line>();
            for (int i = 0; i < result.Count; i++)
            {
                if (i == result.Count - 1)
                {
                    Line last = Line.CreateBound(result[result.Count - 1], result[0]);
                    lines.Add(last);
                    break;
                }
                Line ll = Line.CreateBound(result[i], result[i + 1]);
                lines.Add(ll);
            }
            doc.DrawDebugCurves(lines);


            return Result.Succeeded;
        }
    }
}
