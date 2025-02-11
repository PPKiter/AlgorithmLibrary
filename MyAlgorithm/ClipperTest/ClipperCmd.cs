using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ClipperLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace ClipperTest
{
    [Transaction(TransactionMode.Manual)]
    internal class ClipperCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            var l1 = (doc.GetElement(new ElementId(2500)) as DetailLine).GeometryCurve;
            var l2 = (doc.GetElement(new ElementId(2538)) as DetailLine).GeometryCurve;
            var l3 = (doc.GetElement(new ElementId(2575)) as DetailLine).GeometryCurve;
            //var l4 = (doc.GetElement(new ElementId(3075)) as DetailLine).GeometryCurve;

            List<Curve> crvs1 = new List<Curve>() { l1, l2, l3 };
            var l4 = Line.CreateBound(l3.GetEndPoint(1), l1.GetEndPoint(0));
            crvs1.Add(l4);
            var pts = crvs1.Select(p => p.GetEndPoint(0)).ToList();

            var polygon = pts.Select(p => new IntPoint(p.X * 100000000, p.Y * 100000000)).ToList();


            // 创建 ClipperOffset 对象
            ClipperOffset clipperOffset = new ClipperOffset();

            // 添加多边形
            clipperOffset.AddPath(polygon, ClipperLib.JoinType.jtMiter, EndType.etClosedPolygon);

            // 创建用于存储偏移结果的多边形集合
            List<List<IntPoint>> solution = new List<List<IntPoint>>();

            // 进行偏移，参数为偏移量
            clipperOffset.Execute(ref solution, 0.5 * 100000000);
            // 输出偏移后的多边形

            // 输出偏移后的多边形
            foreach (var path in solution)
            {
                var ss = path.Select(p => new XYZ((double)p.X / 100000000, (double)p.Y / 100000000, 0)).ToList();

                List<Line> lls = new List<Line>();
                for (int i = 0; i < ss.Count; i++)
                {
                    if (i == ss.Count - 1)
                    {
                        //Line lle = Line.CreateBound(ss[i], ss[0]);
                        //lls.Add(lle);
                        break;
                    }
                    Line ll = Line.CreateBound(ss[i], ss[i + 1]);
                    lls.Add(ll);


                }
                lls.DrawDebugCurves(doc);

            }
            return Result.Succeeded;
        }
    }
}
