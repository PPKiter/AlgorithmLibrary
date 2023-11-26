using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_ConvexHull
{

    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class ConvexHull
    {
        // 求极角，返回角度的弧度值
        private static double Angle(Point p1, Point p2)
        {
            return Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }

        // 用于排序的比较器，按照极角排序
        private class PolarAngleComparer : IComparer<Point>
        {
            private Point referencePoint;

            public PolarAngleComparer(Point referencePoint)
            {
                this.referencePoint = referencePoint;
            }

            public int Compare(Point p1, Point p2)
            {
                double angle1 = Angle(referencePoint, p1);
                double angle2 = Angle(referencePoint, p2);

                if (angle1 < angle2)
                    return -1;
                else if (angle1 > angle2)
                    return 1;
                else
                {
                    // 如果两点的极角相同，距离较近的排在前面
                    double distance1 = Math.Pow(p1.X - referencePoint.X, 2) + Math.Pow(p1.Y - referencePoint.Y, 2);
                    double distance2 = Math.Pow(p2.X - referencePoint.X, 2) + Math.Pow(p2.Y - referencePoint.Y, 2);

                    if (distance1 < distance2)
                        return -1;
                    else if (distance1 > distance2)
                        return 1;
                    else
                        return 0;
                }
            }
        }

        // 判断三个点的走向，用于确定是否需要进行栈的出栈操作
        private static int Orientation(Point p, Point q, Point r)
        {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0)
                return 0;  // 三点共线
            return (val > 0) ? 1 : 2; // 顺时针或逆时针
        }

        // Graham 扫描算法
        public static List<Point> GrahamScan(List<Point> points)
        {
            int n = points.Count;
            if (n < 3)
                throw new ArgumentException("凸包需要至少三个点");

            // 寻找最下方且最左边的点
            Point referencePoint = points.OrderBy(p => p.Y).ThenBy(p => p.X).First();

            // 根据极角排序其他点
            List<Point> sortedPoints = points.Where(p => p != referencePoint).ToList();
            sortedPoints.Sort(new PolarAngleComparer(referencePoint));

            // 压入参考点和前两个点
            Stack<Point> hull = new Stack<Point>();
            hull.Push(referencePoint);
            hull.Push(sortedPoints[0]);
            hull.Push(sortedPoints[1]);

            // 处理剩余的点
            for (int i = 2; i < sortedPoints.Count; i++)
            {
                while (hull.Count > 1 && Orientation(hull.ElementAt(1), hull.Peek(), sortedPoints[i]) != 2)
                    hull.Pop();
                hull.Push(sortedPoints[i]);
            }

            return hull.ToList();
        }



    }
}
