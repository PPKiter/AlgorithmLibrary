using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoUtils
{
    /// <summary>
    /// 数组工具类
    /// </summary>
    internal static class ArraryUtils
    {
        /// <summary>
        /// 将一个数组根据两点分割成三段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArrary">目标数组</param>
        /// <param name="startIndex">起始分割点</param>
        /// <param name="endIndex">终止分割点</param>
        /// <returns></returns>
        public static List<List<T>> SegArraryToThreeParts<T>(this T[] sourceArrary,int startIndex,int endIndex)
        {
            List<T> seg1=sourceArrary.Take(startIndex).ToList();
            List<T> seg2 = sourceArrary.Skip(startIndex).Take(endIndex-startIndex+1).ToList();
            List<T> seg3 = sourceArrary.Skip(endIndex+1).Take(sourceArrary.Length-endIndex).ToList();

            return new List<List<T>>() { seg1 , seg2 , seg3 };

        }
    }
}
