using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDebugSlicer
{
    internal static class SlicerUtils
    {
        /// <summary>
        /// 从JsonJson文件中读取三角形信息
        /// </summary>
        /// <param name="vertexs"></param>
        /// <returns></returns>
        public static List<MyTriangle> ReadTriangleMeshJson(List<MyPoint> vertexs, string trianglePath)
        {
            //这里点记录的是每个三角形的索引
            List<MyPoint> pts = JsonConvert.DeserializeObject<List<MyPoint>>(trianglePath);
            List<MyTriangle> triangles = new List<MyTriangle>();
            foreach (var pt in pts)
            {
                int index1 = pt.ID;
                int index2 = (int)pt.X;
                int index3 = (int)pt.Y;

                MyTriangle triangle = new MyTriangle(vertexs[index1], vertexs[index2], vertexs[index3])
                { Indexes = new List<int>() { index1, index2, index3 } };

                triangles.Add(triangle);

            }

            return triangles;
        }

        /// <summary>
        /// 查找模型特征位置
        /// </summary>
        /// <param name="points"></param>
        /// <param name="triangles"></param>
        /// <returns></returns>
        public static List<double> FindSpectialLocations(List<MyPoint> points, List<MyTriangle> triangles)
        {
            List<double> specials = new List<double>();
            //1.查找特征面
            var specialFaces = FindSpectialFaces(triangles, 0.01);
            List<double> faceValues = specialFaces.Select(p => p.Vertexs[0].Z).ToList();
            //2.查找特征边
            var specialEdges = FindSpectialEdges(triangles,Math.PI/4);
            List<double> edgeValues = specialEdges.Select(p => p.Vertex1.Z).ToList();
            edgeValues.AddRange(specialEdges.Select(p => p.Vertex1.Z).ToList());
            //3.查找特征点
            var specialPts= FindSpectialPoints(points, triangles);

            specials.AddRange(faceValues);
            specials.AddRange(edgeValues);
            specials.AddRange(specialPts);

            return specials;
        }

        /// <summary>
        /// 查找特征面
        /// </summary>
        /// <param name="triangles"></param>
        /// <param name="tol">浮点数相等判断误差</param>
        public static List<MyTriangle> FindSpectialFaces(List<MyTriangle> triangles, double tol)
        {
            //查找三点共z面的三角形
            List<MyTriangle> specialFaces = new List<MyTriangle>();
            foreach (var triangle in triangles)
            {
                if (Math.Abs(triangle.Vertex1.Z - triangle.Vertex2.Z) < tol &&
                    Math.Abs(triangle.Vertex2.Z - triangle.Vertex3.Z) < tol &&
                    Math.Abs(triangle.Vertex3.Z - triangle.Vertex1.Z) < tol)
                {
                    specialFaces.Add(triangle);
                }
            }
            return specialFaces;
        }

        /// <summary>
        /// 查找特征边
        /// </summary>
        /// <param name="triangles"></param>
        /// <param name="angle">相邻三角面片法向角度</param>
        /// <returns></returns>
        public static List<MyEdge> FindSpectialEdges(List<MyTriangle> triangles, double angle)
        {
            List<MyEdge> specialEdges = new List<MyEdge>();
            var edges = triangles.SelectMany(p => p.Edges).ToList();
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges.Count == 2)
                {
                    if (edges[i].Triangles[0].Normal.AngleTo(edges[i].Triangles[1].Normal) > angle)
                    {
                        specialEdges.Add(edges[i]);
                    }
                }
            }
            return specialEdges;
        }

        /// <summary>
        /// 查找特征点
        /// </summary>
        /// <param name="points"></param>
        /// <param name="triangles"></param>
        public static List<double> FindSpectialPoints(List<MyPoint> points, List<MyTriangle> triangles)
        {
            List<double> specialPoints = new List<double>();
            for (int i = 0; i < points.Count; i++)
            {
                var hasPtMeshs = triangles.FindAll(p => p.Indexes.Contains(i)).ToList();
                var allPts = hasPtMeshs.SelectMany(p => p.Vertexs).ToList();
                var maxZ = allPts.Max(p => p.Z);
                var minZ = allPts.Min(p => p.Z);
                specialPoints.Add(maxZ);
                specialPoints.Add(minZ);

            }
            return specialPoints;
        }
    }
}
