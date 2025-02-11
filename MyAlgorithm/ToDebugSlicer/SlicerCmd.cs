using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CommonClass;
using Geometry;
using System.Text.RegularExpressions;

namespace ToDebugSlicer
{
    [Transaction(TransactionMode.Manual)]
    internal class SlicerCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            //string pointPath = File.ReadAllText("C:\\Users\\Arthur.Zhuang\\Desktop\\my_prusa\\JsonCompare\\mesh_point.json");
            //List<MyPoint> pts = JsonConvert.DeserializeObject<List<MyPoint>>(pointPath);
            ////单位变换
            //pts.ForEach(p => p.TransUnit(100000000));
            ////坐标变换，最小的z值设为0
            //var minZ = pts.Min(p => p.Z);
            //double offset = minZ - 0;
            //pts.ForEach(p => p.Z -= offset);

            //string trianglePath = File.ReadAllText("C:\\Users\\Arthur.Zhuang\\Desktop\\my_prusa\\JsonCompare\\mesh_index.json");
            //var triangles = SlicerUtils.ReadTriangleMeshJson(pts, trianglePath);
            //var triLines = triangles.Select(p => p.ToXZLines()).Flatten().ToList();
            //triLines = triLines.Distinct(1e-3);
            ////var triLines1 = triLines.GetRange(3000, triLines.Count - 3000);
            //triLines.DrawDebugCurves(doc);


            //查找特征点
            #region 打印帮助方法
            ////打印
            //var triLines1 = triLines.GetRange(500000, triLines.Count- 500000);
            //for (int i = 0; i < 38; i++)
            //{
            //    if (i * 1000 + 1000 < triLines1.Count)
            //    {
            //        var group = triLines1.GetRange(i * 1000, 1000);
            //        group = group.Distinct(1e-3);
            //        group.DrawDebugCurves(doc);
            //    }
            //    else
            //    {
            //        var sur = triLines1.Count % 1000;
            //        var group = triLines1.GetRange(i * 1000, sur);
            //        group.DrawDebugCurves(doc);

            //    }

            //}

            #endregion

            //string s_layer00 = "0\r\n2e+07\r\n4e+07\r\n6e+07\r\n8e+07\r\n1e+08\r\n1.2e+08\r\n1.4e+08\r\n1.6e+08\r\n1.8e+08\r\n2e+08\r\n2.2e+08\r\n2.4e+08\r\n2.6e+08\r\n2.8e+08\r\n3e+08\r\n3.2e+08\r\n3.4e+08\r\n3.6e+08\r\n3.8e+08\r\n4e+08\r\n4.2e+08\r\n4.4e+08\r\n4.6e+08\r\n4.8e+08\r\n5e+08\r\n5.2e+08\r\n5.4e+08\r\n5.6e+08\r\n5.8e+08\r\n6e+08\r\n6.2e+08\r\n6.4e+08\r\n6.6e+08\r\n6.8e+08\r\n7e+08\r\n7.2e+08\r\n7.4e+08\r\n7.6e+08\r\n7.8e+08\r\n8e+08\r\n8.2e+08\r\n8.4e+08\r\n8.6e+08\r\n8.8e+08\r\n9e+08\r\n9.2e+08\r\n9.4e+08\r\n9.6e+08\r\n9.8e+08\r\n1e+09\r\n1.02e+09\r\n1.04e+09\r\n1.06e+09\r\n1.08e+09\r\n1.1e+09\r\n1.12e+09\r\n1.14e+09\r\n1.16e+09\r\n1.18e+09\r\n1.2e+09\r\n1.22e+09\r\n1.24e+09\r\n1.26e+09\r\n1.28e+09\r\n1.3e+09\r\n1.32e+09\r\n1.34e+09\r\n1.36e+09\r\n1.38e+09\r\n1.4e+09\r\n";
            //string s_layer01 = "0\r\n2e+07\r\n4.98e+07\r\n7.8e+07\r\n9.8e+07\r\n1.18e+08\r\n1.38e+08\r\n1.583e+08\r\n1.783e+08\r\n1.983e+08\r\n2.183e+08\r\n2.383e+08\r\n2.583e+08\r\n2.786e+08\r\n2.998e+08\r\n3.198e+08\r\n3.398e+08\r\n3.598e+08\r\n3.798e+08\r\n4.002e+08\r\n4.219e+08\r\n4.486e+08\r\n4.644e+08\r\n4.794e+08\r\n4.998e+08\r\n5.198e+08\r\n5.401e+08\r\n5.601e+08\r\n5.801e+08\r\n6.001e+08\r\n6.201e+08\r\n6.401e+08\r\n6.601e+08\r\n6.798e+08\r\n6.998e+08\r\n7.198e+08\r\n7.398e+08\r\n7.598e+08\r\n7.798e+08\r\n7.998e+08\r\n8.198e+08\r\n8.376e+08\r\n8.621e+08\r\n8.804e+08\r\n9.001e+08\r\n9.201e+08\r\n9.401e+08\r\n9.641e+08\r\n9.841e+08\r\n1.0041e+09\r\n1.0303e+09\r\n1.0503e+09\r\n1.0703e+09\r\n1.1001e+09\r\n1.12e+09\r\n1.1485e+09\r\n1.1685e+09\r\n1.1885e+09\r\n1.2085e+09\r\n1.2285e+09\r\n1.2485e+09\r\n1.2685e+09\r\n1.2885e+09\r\n1.3085e+09\r\n1.3376e+09\r\n1.3576e+09\r\n1.3845e+09\r\n1.4001e+09\r\n";

            string ss = "2e+07\r\n4e+07\r\n6e+07\r\n8e+07\r\n1e+08\r\n1.2e+08\r\n2.6e+08\r\n2.8e+08\r\n3e+08\r\n3.2e+08\r\n3.4e+08\r\n1.32e+09\r\n1.34e+09\r\n1.6e+09\r\n1.62e+09\r\n2.28e+09\r\n2.3e+09\r\n2.32e+09\r\n2.34e+09\r\n2.36e+09\r\n2.38e+09\r\n2.48e+09\r\n2.5e+09\r\n2.52e+09\r\n2.54e+09\r\n2.56e+09\r\n2.58e+09\r\n2.6e+09\r\n2.62e+09\r\n2.64e+09\r\n2.66e+09\r\n2.68e+09\r\n2.7e+09\r\n2.72e+09\r\n2.82e+09\r\n2.84e+09\r\n2.86e+09\r\n2.88e+09\r\n";
            // 定义正则表达式来匹配数字（包括科学记数法）
            string pattern = @"[-+]?\d*\.?\d+([eE][-+]?\d+)?";
            // 使用Regex.Matches找到所有匹配的数字
            MatchCollection matches = Regex.Matches(ss, pattern);
            List<double> numdoubles = new List<double>();
            foreach (Match match in matches)
            {
                string numberStr = match.Value;
                double number;
                // 尝试将字符串转换为双精度浮点数
                if (double.TryParse(numberStr, out number))
                {
                    numdoubles.Add(number);
                }
            }
            var zLayers = numdoubles.Select(p => p / 100000000).ToList();
            zLayers = zLayers.Distinct().ToList();
            var zPts = zLayers.Select(p => new XYZ(0, p, 0)).ToList();
            var zLines = zPts.Select(p => Line.CreateBound(p.Offset(XYZ.BasisX.Negate(), 10), p.Offset(XYZ.BasisX, 10)));
            zLines.DrawDebugCurves(doc);

            return Result.Succeeded;

        }
    }
}
