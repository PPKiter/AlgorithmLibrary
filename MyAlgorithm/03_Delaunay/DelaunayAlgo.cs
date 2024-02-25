using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Delaunay
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
    }
    internal class DelaunayAlgo
    {

        public class Triangle
        {
            public Point Vertex1 { get; }
            public Point Vertex2 { get; }
            public Point Vertex3 { get; }

            public Triangle(Point v1, Point v2, Point v3)
            {
                Vertex1 = v1;
                Vertex2 = v2;
                Vertex3 = v3;
            }

            public bool Contains(Point point)
            {
                double denominator = ((Vertex2.Y - Vertex3.Y) * (Vertex1.X - Vertex3.X) + (Vertex3.X - Vertex2.X) * (Vertex1.Y - Vertex3.Y));
                double alpha = ((Vertex2.Y - Vertex3.Y) * (point.X - Vertex3.X) + (Vertex3.X - Vertex2.X) * (point.Y - Vertex3.Y)) / denominator;
                double beta = ((Vertex3.Y - Vertex1.Y) * (point.X - Vertex3.X) + (Vertex1.X - Vertex3.X) * (point.Y - Vertex3.Y)) / denominator;
                double gamma = 1 - alpha - beta;

                return alpha >= 0 && beta >= 0 && gamma >= 0;
            }
        }

        private List<Triangle> triangles;

        public List<Triangle> Triangulate(List<Point> points)
        {
            triangles = new List<Triangle>();

            // 添加一个超级三角形，包含所有点
            double maxX = points.Max(p => p.X);
            double maxY = points.Max(p => p.Y);
            double minX = points.Min(p => p.X);
            double minY = points.Min(p => p.Y);

            double deltaMax = Math.Max(maxX - minX, maxY - minY);
            double midX = (maxX + minX) * 0.5;
            double midY = (maxY + minY) * 0.5;

            Point p1 = new Point(midX - 20 * deltaMax, midY - deltaMax);
            Point p2 = new Point(midX, midY + 20 * deltaMax);
            Point p3 = new Point(midX + 20 * deltaMax, midY - deltaMax);

            triangles.Add(new Triangle(p1, p2, p3));

            // 逐点加入三角形
            foreach (var point in points)
            {
                List<Triangle> badTriangles = new List<Triangle>();

                foreach (var triangle in triangles)
                {
                    if (triangle.Contains(point))
                    {
                        badTriangles.Add(triangle);
                    }
                }

                List<Tuple<Point, Point>> polygonEdges = new List<Tuple<Point, Point>>();

                foreach (var triangle in badTriangles)
                {
                    polygonEdges.Add(new Tuple<Point, Point>(triangle.Vertex1, triangle.Vertex2));
                    polygonEdges.Add(new Tuple<Point, Point>(triangle.Vertex2, triangle.Vertex3));
                    polygonEdges.Add(new Tuple<Point, Point>(triangle.Vertex3, triangle.Vertex1));
                }

                triangles.RemoveAll(t => badTriangles.Contains(t));

                foreach (var edge in polygonEdges)
                {
                    if (polygonEdges.Count(e => e.Equals(new Tuple<Point, Point>(edge.Item2, edge.Item1))) == 0)
                    {
                        triangles.Add(new Triangle(edge.Item1, edge.Item2, point));
                    }
                }
            }

            // 移除超级三角形中包含的点
            triangles.RemoveAll(t =>
                t.Vertex1 == p1 || t.Vertex1 == p2 || t.Vertex1 == p3 ||
                t.Vertex2 == p1 || t.Vertex2 == p2 || t.Vertex2 == p3 ||
                t.Vertex3 == p1 || t.Vertex3 == p2 || t.Vertex3 == p3);

            return triangles;
        }
    }
}

