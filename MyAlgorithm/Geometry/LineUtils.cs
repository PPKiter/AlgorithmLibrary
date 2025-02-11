using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public static class LineUtils
    {
        #region Line

        /// <summary>
        /// 将线段投影到XY平面
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line NewZ(this Line line)
        {
            return Line.CreateBound(line.GetEndPoint(0).NewZ(), line.GetEndPoint(1).NewZ());
        }

        /// <summary>
        /// 将线段投影到XZ平面
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line NewY(this Line line)
        {
            return Line.CreateBound(line.GetEndPoint(0).NewY(), line.GetEndPoint(1).NewY());
        }

        /// <summary>
        /// 将线段投影到YZ平面
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line NewX(this Line line)
        {
            return Line.CreateBound(line.GetEndPoint(0).NewX(), line.GetEndPoint(1).NewX());
        }

        /// <summary>
        /// 获取线段中点
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static XYZ Middle(this Line line)
        {
            return 0.5 * line.GetEndPoint(0) + 0.5 * line.GetEndPoint(1);
        }

        /// <summary>
        /// 判断两条之间是否完全相同，忽略方向
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="tol"></param>
        /// <returns></returns>
        public static bool IsEqualWithIngoreDir(this Line line1, Line line2, double tol)
        {
            return Math.Abs(line1.Length - line2.Length) < tol &&
                line1.Middle().IsAlmostEqualTo(line2.Middle(), tol)&&
                (line1.GetEndPoint(0).IsAlmostEqualTo(line2.GetEndPoint(0),tol)||
                line1.GetEndPoint(0).IsAlmostEqualTo(line2.GetEndPoint(1), tol));


        }

        /// <summary>
        /// 判断两条之间是否完全相同，考虑方向
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="tol"></param>
        /// <returns></returns>
        public static bool IsEqualWithDir(this Line line1, Line line2, double tol)
        {
            return line1.GetEndPoint(0).IsAlmostEqualTo(line2.GetEndPoint(0), 1e-3) &&
                line1.GetEndPoint(1).IsAlmostEqualTo(line2.GetEndPoint(1), 1e-3);
        }

        #endregion


        #region List<Line>
        /// <summary>
        /// 将线段集投影到XY平面
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static List<Line> NewZ(this List<Line> lines)
        {
            return lines.Select(p => p.NewZ()).ToList();
        }

        /// <summary>
        /// 线段集去重
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<Line> Distinct(this List<Line> lines,double tol=0)
        {
            List<Line> sames = new List<Line>();
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = i + 1; j < lines.Count; j++)
                {
                    if (lines[i].IsEqualWithIngoreDir(lines[j], tol))
                    {
                        sames.Add(lines[i]);
                        break;
                    }
                }
            }
            sames.ForEach(p => lines.Remove(p));
            return lines;
        }

        #endregion
    }
}
