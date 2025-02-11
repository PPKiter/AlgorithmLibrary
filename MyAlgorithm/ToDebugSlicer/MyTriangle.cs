using Autodesk.Revit.DB;
using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDebugSlicer
{
    internal class MyTriangle
    {
        public MyPoint Vertex1 { get; set; }
        public MyPoint Vertex2 { get; set; }
        public MyPoint Vertex3 { get; set; }

        /// <summary>
        /// 法向量
        /// </summary>
        public XYZ Normal { get; set; }
        /// <summary>
        /// 顶点索引
        /// </summary>
        public List<int> Indexes { get; set; }
        /// <summary>
        /// 顶点
        /// </summary>
        public List<MyPoint> Vertexs { get; set; }

        private List<MyEdge> _edges = null;
        /// <summary>
        /// 边
        /// </summary>
        public List<MyEdge> Edges
        {
            get
            {
                if (_edges == null)
                {
                    _edges = new List<MyEdge>()
                    {
                            new MyEdge(Indexes[0], Indexes[1]),
                            new MyEdge(Indexes[1], Indexes[2]),
                            new MyEdge(Indexes[2], Indexes[0])
                    };

                }
                return _edges;
            }
        }


        public MyTriangle(MyPoint v1, MyPoint v2, MyPoint v3)
        {
            Vertex1 = v1;
            Vertex2 = v2;
            Vertex3 = v3;
            Vertexs = new List<MyPoint>() { Vertex1, Vertex2, Vertex3 };
            Normal = (Vertex2.ToXYZ() - Vertex1.ToXYZ()).CrossProduct(Vertex3.ToXYZ() - Vertex2.ToXYZ()).Normalize();
        }

        /// <summary>
        /// 将三角形转换为revit线段
        /// </summary>
        /// <returns></returns>
        public List<Line> ToLines()
        {
            List<Line> lines = new List<Line>();
            if (!Vertex1.IsEqual(Vertex2, 1e-3))
            {
                Line l1 = Line.CreateBound(Vertex1.ToXYZ(), Vertex2.ToXYZ());
                lines.Add(l1);
            }
            if (!Vertex2.IsEqual(Vertex3, 1e-3))
            {
                Line l2 = Line.CreateBound(Vertex2.ToXYZ(), Vertex3.ToXYZ());
                lines.Add(l2);
            }
            if (!Vertex3.IsEqual(Vertex1, 1e-3))
            {
                Line l3 = Line.CreateBound(Vertex3.ToXYZ(), Vertex1.ToXYZ());
                lines.Add(l3);
            }

            return lines;
        }

        public List<Line> ToLines(XYZ pt1, XYZ pt2, XYZ pt3)
        {
            List<Line> lines = new List<Line>();
            if (!pt1.IsAlmostEqualTo(pt2, 1e-3))
            {
                Line l1 = Line.CreateBound(pt1, pt2);
                lines.Add(l1);
            }
            if (!pt2.IsAlmostEqualTo(pt3, 1e-3))
            {
                Line l2 = Line.CreateBound(pt2, pt3);
                lines.Add(l2);
            }
            if (!pt3.IsAlmostEqualTo(pt1, 1e-3))
            {
                Line l3 = Line.CreateBound(pt1, pt3);
                lines.Add(l3);
            }

            return lines;
        }

        /// <summary>
        /// 三角形在XY平面上投影的线段
        /// </summary>
        /// <returns></returns>
        public List<Line> ToXYLines()
        {
            var pt1 = Vertex1.ToXYZ().NewZ();
            var pt2 = Vertex2.ToXYZ().NewZ();
            var pt3 = Vertex3.ToXYZ().NewZ();

            return ToLines(pt1, pt2, pt3);
        }

        /// <summary>
        /// 三角形在XY平面上投影的线段
        /// </summary>
        /// <returns></returns>
        public List<Line> ToXZLines()
        {
            //由于Revit只能在XY平面绘制详图线，所以这里需要将Z的值转到Y中
            var pt1 = Vertex1.ToXYZ().NewZ().NewY(Vertex1.Z);
            var pt2 = Vertex2.ToXYZ().NewZ().NewY(Vertex2.Z);
            var pt3 = Vertex3.ToXYZ().NewZ().NewY(Vertex3.Z);

            return ToLines(pt1, pt2, pt3);
        }

        /// <summary>
        /// 三角形在XY平面上投影的线段
        /// </summary>
        /// <returns></returns>
        public List<Line> ToYZLines()
        {
            //由于Revit只能在XY平面绘制详图线，所以这里需要将Z的值转到X中
            var pt1 = Vertex1.ToXYZ().NewZ().NewX(Vertex1.Y).NewY(Vertex1.Z);
            var pt2 = Vertex2.ToXYZ().NewZ().NewX(Vertex2.Y).NewY(Vertex2.Z);
            var pt3 = Vertex3.ToXYZ().NewZ().NewX(Vertex3.Y).NewY(Vertex3.Z);

            return ToLines(pt1, pt2, pt3);
        }
    }
}
