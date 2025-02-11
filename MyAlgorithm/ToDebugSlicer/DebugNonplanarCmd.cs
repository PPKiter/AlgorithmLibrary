using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Geometry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDebugSlicer
{
    [Transaction(TransactionMode.Manual)]
    internal class DebugNonplanarCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            //string filePath = "C:\\Users\\Arthur.Zhuang\\Desktop\\non_debug\\lin4\\process\\74loop2.txt";

            string folderPath = "C:\\Users\\Arthur.Zhuang\\Desktop\\non_debug\\lin4\\process";
            var txtPaths = GetDirAllTxts(folderPath).ToList();



            List<Line> allPolys=new List<Line>();
            foreach (var txtPath in txtPaths)
            {
                var points = GetMyPoints(txtPath);
                points.ForEach(p => p.TransUnit(1e-1));
                var poly = GetPolygon(points);
                allPolys.AddRange(poly);
            }
            allPolys.DrawDebugCurves(doc);


            return Result.Succeeded;

        }

        /// <summary>
        /// 从.txt文件中读取点的坐标
        /// </summary>
        /// <param name="filePath"></param>
        public List<MyPoint> GetMyPoints(string filePath)
        {
            List<MyPoint> points = new List<MyPoint>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        if (values.Length == 2 &&
                            float.TryParse(values[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
                            float.TryParse(values[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
                        {
                            points.Add(new MyPoint(x, y, 0));
                        }
                        else
                        {
                            TaskDialog.Show("提示", $"无法解析行: {line}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("提示", $"读取文件时出错: {ex.Message}");
            }
            return points;
        }

        /// <summary>
        /// 根据点集连接成多边形
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public List<Line> GetPolygon(List<MyPoint> points)
        {
            //降点连接成多边形
            List<Line> polygon = new List<Line>();
            var pts = points.Select(p => p.ToXYZ()).ToList();

            for (int j = 0; j < pts.Count; j++)
            {
                if (j == pts.Count - 1)
                {
                    //Line last = Line.CreateBound(pts[j], pts[0]);
                    //polygon.Add(last);
                    break;

                }
                try
                {
                    Line ll = Line.CreateBound(pts[j], pts[j + 1]);
                    polygon.Add(ll);
                }
                catch (Exception)
                {
                    continue;
                }

            }
            return polygon;
        }

        /// <summary>
        /// 获取文件夹下所有.txt文件
        /// </summary>
        /// <param name="folderPath"></param>
        public string[] GetDirAllTxts(string folderPath)
        {
            try
            {
                // 检查文件夹是否存在
                if (Directory.Exists(folderPath))
                {
                    // 获取所有 .txt 文件
                    string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");
                    return txtFiles;
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("提示", $"发生错误: {ex.Message}");
            }

            return new string[0];
        }
    }
}
