using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DebugUtils
{
    public static class DebugUtils
    {

        public static void DrawDebugCurves(this Document doc, IEnumerable<Curve> curves)
        {
            Transaction ts = new Transaction(doc, "DrawDebugCurves");
            ts.Start();
            List<DetailCurve> detailCurves = new List<DetailCurve>();
            foreach (var curve in curves)
            {
                var detailCurve = doc.Create.NewDetailCurve(doc.ActiveView, curve);
                detailCurves.Add(detailCurve);
            }

            doc.CreateOrModifyLineStyle(detailCurves, "Debug线");
            ts.Commit();
        }

        /// <summary>
        /// 创建或修改线样式
        /// </summary>
        private static void CreateOrModifyLineStyle(this Document doc, List<DetailCurve> detailCurves, string name)
        {
            //获得项目里面的线
            var lineCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            //得到这个线下面所有的线样式
            var subcats = lineCategory.SubCategories;
            Category lineStyle = null;
            foreach (Category item in subcats)
            {
                //通过样式名称，查找需求样式
                if (item.Name == name)
                {
                    lineStyle = item;
                }
            }

            if (lineStyle == null)
            {
                //获取所有的线型图案，即实线、中心线、点划线等
                var linePatternElements = new FilteredElementCollector(doc).OfClass(typeof(LinePatternElement)).ToElements();
                var linePattern = linePatternElements.Where(x => x.Name == "实线").FirstOrDefault();
                //注意：实线比较特殊，不是linePattern，要用LinePatternElement.GetSolidPatternId()获取
                lineStyle = CreateNewLineType(doc, new Color(255, 0, 0), 6, LinePatternElement.GetSolidPatternId(), name);
            }

            //修改这条模型线的样式
            foreach (DetailCurve curve in detailCurves)
            {
                curve.LineStyle = lineStyle.GetGraphicsStyle(GraphicsStyleType.Projection);
            }

        }

        /// <summary>
        /// 创建新的线样式
        /// </summary>
        /// <param name="newColor"></param>
        /// <param name="weight"></param>
        /// <param name="linePatternId"></param>
        /// <returns></returns>
        private static Category CreateNewLineType(Document doc, Color newColor, int weight, ElementId linePatternId, string name)
        {
            Category newCategory = null;

            Category lineCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            newCategory = doc.Settings.Categories.NewSubcategory(lineCategory, name);
            //设置线的颜色
            newCategory.LineColor = newColor;
            //设置线的线型
            newCategory.SetLinePatternId(linePatternId, GraphicsStyleType.Projection);
            //设置线宽
            newCategory.SetLineWeight(weight, GraphicsStyleType.Projection);

            return newCategory;

        }

    }
}
