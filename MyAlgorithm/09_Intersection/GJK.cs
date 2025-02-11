using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_Intersection
{
    struct Pt2
    {
        public double x, y;

        public Pt2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Pt2 operator -(Pt2 a, Pt2 b) => new Pt2(a.x - b.x, a.y - b.y);
        public double Dot(Pt2 other) => x * other.x + y * other.y;
        public Pt2 Normalize()
        {
            double length = (double)Math.Sqrt(x * x + y * y);
            return new Pt2(x / length, y / length);
        }

        // 计算垂直向量
        public Pt2 Perpendicular() => new Pt2(-y, x);
    }

    class GJK
    {
        private static Pt2 Support(List<Pt2> shape, Pt2 direction)
        {
            double maxDot = float.NegativeInfinity;
            Pt2 bestVertex = new Pt2();

            foreach (var vertex in shape)
            {
                double dotProduct = vertex.Dot(direction);
                if (dotProduct > maxDot)
                {
                    maxDot = dotProduct;
                    bestVertex = vertex;
                }
            }
            return bestVertex;
        }

        private static bool HandleSimplex(List<Pt2> simplex, ref Pt2 direction)
        {
            if (simplex.Count == 2)
            {
                Pt2 A = simplex[1];
                Pt2 B = simplex[0];
                Pt2 AB = B - A;
                Pt2 AO = new Pt2(-A.x, -A.y);
                direction = AB.Perpendicular().Normalize();
                return false;
            }

            if (simplex.Count == 3)
            {
                Pt2 A = simplex[2];
                Pt2 B = simplex[1];
                Pt2 C = simplex[0];
                Pt2 AB = B - A;
                Pt2 AC = C - A;
                Pt2 AO = new Pt2(-A.x, -A.y);

                if (AB.Perpendicular().Dot(AO) > 0)
                {
                    direction = AB.Perpendicular().Normalize();
                    simplex.Remove(C);
                }
                else
                {
                    direction = AC.Perpendicular().Normalize();
                    simplex.Remove(B);
                }

                return false;
            }

            return true; // 如果简单形状包含原点
        }

        /// <summary>
        /// 相交检测
        /// </summary>
        /// <param name="shapeA"></param>
        /// <param name="shapeB"></param>
        /// <returns></returns>
        public static bool CheckCollision(List<Pt2> shapeA, List<Pt2> shapeB)
        {
            Pt2 d = new Pt2(1, 0);
            Pt2 point = Support(shapeA, d) - Support(shapeB, new Pt2(-d.x, -d.y));
            List<Pt2> simplex = new List<Pt2> { point };

            for (int iteration = 0; iteration < 100; iteration++)
            {
                if (point.Dot(point) < 1e-6) // 原点在简单形状内
                    return true;

                d = point.Normalize();
                point = Support(shapeA, d) - Support(shapeB, new Pt2(-d.x, -d.y));

                if (point.Dot(d) < 0) // 如果点在支持方向的反方向，退出
                    return false;

                simplex.Add(point);

                if (HandleSimplex(simplex, ref d)) // 处理简单形状
                    return true;
            }

            return false; // 未找到交点
        }
    }
}
