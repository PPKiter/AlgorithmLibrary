using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// 点是否在多边形内
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public bool IsPtInCircle(List<Curve> circle)
        {
            XYZ pt = new XYZ(X, Y, 0);
            XYZ pt1 = new XYZ(X + int.MaxValue * 0.5, Y, 0);
            Line ll = Line.CreateBound(pt, pt1);
            //射线法
            int p = 0;
            foreach (Curve c in circle)
            {
                IntersectionResultArray resultArray;
                c.Intersect(ll, out resultArray);
                if (!resultArray.IsEmpty)
                {
                    p++;
                }
            }
            return p % 2 == 0 ? false : true;
        }
    }

    class Rectangle
    {
        public List<Point> Points { get; set; }
        public Rectangle(Point p1, Point p2, Point p3, Point p4)
        {
            Points = new List<Point>() { p1, p2, p3, p4 };
        }

        /// <summary>
        /// 矩形是否在多边形内
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public bool IsRecInCircle(List<Curve> circle)
        {
            var insides = Points.Where(p => p.IsPtInCircle(circle));
            if (!insides.Any())
            {
                return false;
            }
            return insides.Count()==4?true:false;
        }
    }
}
