using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public static class XYZUtils
    {
        /// <summary>
        /// 将点投影到XY平面
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static XYZ NewZ(this XYZ pt,double z=0)
        {
            return new XYZ(pt.X, pt.Y, z);
        }

        /// <summary>
        /// 将点投影到YZ平面
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static XYZ NewX(this XYZ pt,double x=0)
        {
            return new XYZ(x, pt.Y, pt.Z);
        }

        /// <summary>
        /// 将点投影到XZ平面
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static XYZ NewY(this XYZ pt,double y=0)
        {
            return new XYZ(pt.X, y, pt.Z);
        }

        /// <summary>
        /// 点的偏移（二维）
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="dir">偏移方向</param>
        /// <param name="offset">偏移距离</param>
        /// <returns></returns>
        public static XYZ Offset(this XYZ pt, XYZ dir, double offset)
        {
            double dx2 = offset * offset / ((dir.Y * dir.Y / dir.X * dir.X) + 1);
            double dx=Math.Sqrt(dx2);
            double dy = (dir.Y / dir.X) * dx;
            if (dir.X < 0)
            {
                dx = -dx;
            }
            if (dir.Y < 0)
            {
                dy = -dy;
            }
            return new XYZ(pt.X + dx*offset, pt.Y + dy*offset, 0);
        }
    }
}
