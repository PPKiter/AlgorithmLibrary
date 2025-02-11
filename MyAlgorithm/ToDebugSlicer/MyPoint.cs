using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDebugSlicer
{
    internal class MyPoint
    {
        public int ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public MyPoint()
        {
                
        }
        public MyPoint(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        /// <summary>
        /// 相等判断
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tol"></param>
        /// <returns></returns>
        public bool IsEqual(MyPoint other,double tol)
        {
            return Math.Abs(X-other.X)<=tol && Math.Abs(Y-other.Y)<=tol && Math.Abs(Z-other.Z)<=tol;
        }

        /// <summary>
        /// 转换成Revit中的XYZ
        /// </summary>
        /// <returns></returns>
        public XYZ ToXYZ()
        {
            return new XYZ(X, Y, Z);
        }

        /// <summary>
        /// 单位变换(除以单位)
        /// </summary>
        /// <param name="unit"></param>
        public void TransUnit(double unit)
        {
            X = X / unit;
            Y = Y / unit;
            Z = Z / unit;
            
        }
    }

}
