using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDebugSlicer
{
    [Transaction(TransactionMode.Manual)]
    internal class DebugPolyonCmd : IExternalCommand
    {


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            string pointPath = File.ReadAllText("C:\\Users\\Arthur.Zhuang\\Desktop\\my_prusa\\export_json\\expoints42.json");
            //string pointPath = File.ReadAllText("C:\\Users\\Arthur.Zhuang\\Desktop\\my_prusa\\export_json\\points.json");
            List<List<MyPoint>> v_pts = JsonConvert.DeserializeObject<List<List<MyPoint>>>(pointPath);
            //单位变换

            List<List<MyPoint>> trans= new List<List<MyPoint>>();
            for (int i = 0; i < v_pts.Count; i++)
            {
                //var pts = v_pts[i].Select(p => new MyPoint() { X = p.ID, Y = p.X }).ToList();
                var pts = v_pts[i];
                pts.ForEach(p => p.TransUnit(10));
                trans.Add(pts);
            }

            //连接成多边形
            List<List<Line>> polygons = new List<List<Line>>();
            for (int i = 0; i < trans.Count; i++)
            {
                var pts = trans[i].Select(p => new XYZ(p.X, p.Y, 0)).ToList();
                List<Line> polygon = new List<Line>();
                for (int j = 0; j < pts.Count; j++)
                {
                    if (j == pts.Count - 1)
                    {
                        Line last = Line.CreateBound(pts[j], pts[0]);
                        polygon.Add(last);
                        break;

                    }
                    Line ll = Line.CreateBound(pts[j], pts[j + 1]);
                    polygon.Add(ll);
                }
                polygons.Add(polygon);
            }

            polygons[7].DrawDebugCurves(doc);
            //polygons[9].DrawDebugCurves(doc);
            //foreach (var polygon in polygons)
            //{
            //    polygon.DrawDebugCurves(doc);
            //}
            return Result.Succeeded;
        }
    }
}
