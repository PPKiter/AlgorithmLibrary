using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDebugSlicer
{
    internal class MyEdge
    {
        public MyPoint Vertex1 { get; set; }
        public MyPoint Vertex2 { get; set; }

        public List<int> Indexes { get; set; }
        /// <summary>
        /// 邻接的三角形
        /// </summary>
        public List<MyTriangle> Triangles { get; set; }

        public MyEdge(MyPoint p1, MyPoint p2, int index1, int index2)
        {
            Vertex1 = p1;
            Vertex2 = p2;
            Indexes = new List<int>() { index1, index2 };
        }
        public MyEdge(int index1, int index2)
        {
            Indexes = new List<int>() { index1, index2 };
        }

        /// <summary>
        /// 计算邻接的三角形
        /// </summary>
        /// <param name="triangles"></param>
        public void CalNeighborTriangles(List<MyTriangle> triangles)
        {
            Triangles = triangles.FindAll(p => p.Indexes.Contains(Indexes[0]) && p.Indexes.Contains(Indexes[1]));
        }
    }
}
