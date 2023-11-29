using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_ConcaveHull
{
    /// <summary>
    /// 二维平面上的一个点
    /// </summary>
    /// </summary>
    public class Point
    {
        /// <summary>
        /// x坐标
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// y坐标
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 到其他点的距离
        /// </summary>
        public double Distance { get; set; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
    /// <summary>
    /// 凹包类
    /// </summary>
    public class ConcaveHull
    {
     
        //点点之间距离列表
        private double[,] _distances;
        //邻居列表
        private List<int>[] _neigbours;
        private bool[] _signs;
        private List<Point> _points;

        public ConcaveHull(List<Point> list)
        {
            this._points = list;
            _points = _points.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
            _signs = new bool[_points.Count];
            for (int i = 0; i < _signs.Length; i++)
            {
                _signs[i] = false;
            }
            InitDistance();
            InitNeighbours();
        }


        /// <summary>
        /// 计算默认的半径
        /// </summary>
        /// <returns></returns>
        public double CalDefaultRadius()
        {
            double r = double.MinValue;
            for (int i = 0; i < _points.Count; i++)
            {
                if (_distances[i, _neigbours[i][1]] > r)
                {
                    r = _distances[i, _neigbours[i][1]];
                }
            }
            return r;
        }
        /// <summary>
        /// 使用滚球法获取凹包
        /// 不输入半径时，用默认计算半径代替
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public List<Point> Compute(double radius=-1)
        {
            //计算默认半径
            if (radius == -1)
            {
                radius = CalDefaultRadius();
            }
            List<Point> results = new List<Point>();
            List<int>[] neighs = GetNeighbourList(2 * radius);
            results.Add(_points[0]);
            int i = 0, j = -1, pre = -1;
            while (true)
            {
                j = GetNextPoint(pre, i, neighs[i], radius);
                if (j == -1)
                {
                    break;
                }
                Point p = CalCenterByPtsAndRadius(_points[i], _points[j], radius);
                results.Add(_points[j]);
                _signs[j] = true;
                pre = i;
                i = j;
            }
            return results;
        }


        /// <summary>
        /// 初始化每个点的按距离排序的邻居列表
        /// </summary>
        private void InitNeighbours()
        {
            _neigbours = new List<int>[_points.Count];
            for (int i = 0; i < _neigbours.Length; i++)
            {
                _neigbours[i] = SortNeighboursByDis(i);
            }
        }
        /// <summary>
        /// 初始化点之间的距离
        /// </summary>
        private void InitDistance()
        {
            _distances = new double[_points.Count, _points.Count];
            for (int i = 0; i < _points.Count; i++)
            {
                for (int j = 0; j < _points.Count; j++)
                {
                    _distances[i, j] = CalDistanceToPts(_points[i], _points[j]);
                }
            }
        }
        /// <summary>
        /// 获取下一个凹包点
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="cur"></param>
        /// <param name="list"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private int GetNextPoint(int pre, int cur, List<int> list, double radius)
        {
            SortNeighbours(list, pre, cur);
            for (int j = 0; j < list.Count; j++)
            {
                if (_signs[list[j]])
                {
                    continue;

                }
                int adjIndex = list[j];
                Point xianp = _points[adjIndex];
                Point rightCirleCenter = CalCenterByPtsAndRadius(_points[cur], xianp, radius);
                if (!IsPointsInCircle(list, rightCirleCenter, radius, adjIndex))
                {
                    return list[j];
                }
            }
            return -1;
        }
        /// <summary>
        /// 根据角度对邻居列表进行排序
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pre"></param>
        /// <param name="cur"></param>
        private void SortNeighbours(List<int> list, int pre, int cur)
        {
            Point origin = _points[cur];
            Point df;
            if (pre != -1)
            {
                df = new Point(_points[pre].X - origin.X, _points[pre].Y - origin.Y);
            }
            else
            {
                df = new Point(1, 0);
            }
            int temp = 0;
            for (int i = list.Count; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    if (CompareAngle(_points[list[j]], _points[list[j + 1]], origin, df))
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }
        /// <summary>
        /// 检查圆内是否存在其他点
        /// </summary>
        /// <param name="roundPts"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        private bool IsPointsInCircle(List<int> roundPts, Point center, double radius, int roundId)
        {
            for (int k = 0; k < roundPts.Count; k++)
            {
                if (roundPts[k] != roundId)
                {
                    int index2 = roundPts[k];
                    if (IsPtInCircle(_points[index2], center, radius))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 根据两点及半径计算圆心
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private static Point CalCenterByPtsAndRadius(Point a, Point b, double r)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double cx = 0.5 * (b.X + a.X);
            double cy = 0.5 * (b.Y + a.Y);
            if (r * r / (dx * dx + dy * dy) - 0.25 < 0)
            {
                return new Point(-1, -1);
            }
            double sqrt = Math.Sqrt(r * r / (dx * dx + dy * dy) - 0.25);
            return new Point(cx - dy * sqrt, cy + dx * sqrt);
        }
        /// <summary>
        /// 检查点是否在圆内
        /// </summary>
        /// <param name="p"></param>
        /// <param name="center"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private static bool IsPtInCircle(Point p, Point center, double r)
        {
            double dis2 = (p.X - center.X) * (p.X - center.X) + (p.Y - center.Y) * (p.Y - center.Y);
            return dis2 < r * r;
        }
        /// <summary>
        /// 获取半径范围内的邻居列表
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        private List<int>[] GetNeighbourList(double radius)
        {
            List<int>[] adjs = new List<int>[_points.Count];
            for (int i = 0; i < _points.Count; i++)
            {
                adjs[i] = new List<int>();
            }
            for (int i = 0; i < _points.Count; i++)
            {

                for (int j = 0; j < _points.Count; j++)
                {
                    if (i < j && _distances[i, j] < radius)
                    {
                        adjs[i].Add(j);
                        adjs[j].Add(i);
                    }
                }
            }
            return adjs;
        }
        /// <summary>
        /// 按距离排序的邻居列表
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private List<int> SortNeighboursByDis(int index)
        {
            List<Point> pts = new List<Point>();

            for (int i = 0; i < _points.Count; i++)
            {
                Point pt = _points[i];
                pt.Id = i;
                pt.Distance = _distances[index,i];
                pts.Add(pt);
            }
            pts= pts.OrderBy(p=>p.Distance).ToList();

            List<int> adj = new List<int>();
            for (int i = 1; i < pts.Count; i++)
            {
                adj.Add(pts[i].Id);
            }
            return adj;
        }
        /// <summary>
        /// 比较两个点相对于参考点的角度
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="origin"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        private bool CompareAngle(Point a, Point b, Point origin, Point reference)
        {

            Point da = new Point(a.X - origin.X, a.Y - origin.Y);
            Point db = new Point(b.X - origin.X, b.Y - origin.Y);
            //b相对于参考向量的叉积
            double detb = CalCrossProduct(reference, db);
            // 如果 b 的叉积为零且 b 与参考向量的夹角大于等于零度，则返回 false
            if (detb == 0 && db.X * reference.X + db.Y * reference.Y >= 0)
            {
                return false;
            }
            //a 相对于参考向量的叉积
            double deta = CalCrossProduct(reference, da);
            //如果 a 的叉积为零且 a 与参考向量的夹角大于等于零度，则返回 true
            if (deta == 0 && da.X * reference.X + da.Y * reference.Y >= 0)
            {
                return true;
            }
            // 如果 a 和 b 在参考向量的同一侧，则比较它们之间的叉积大小
            if (deta * detb >= 0)
            {
                //如果叉积大于零，返回 true；否则返回 false
                return CalCrossProduct(da, db) > 0;
            }

            //向量小于零度实际上是很大的，接近 2pi
            return deta > 0;
        }

        /// <summary>
        /// 计算两点之间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static double CalDistanceToPts(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        /// <summary>
        /// 计算两个向量的叉积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static double CalCrossProduct(Point a, Point b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}
