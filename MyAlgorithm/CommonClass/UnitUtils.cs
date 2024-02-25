using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass
{
    /// <summary>
    /// 单位转换工具类
    /// </summary>
    public static class UnitUtils
    {
        public static double FeetToMM(this double feet)
        {
            return feet * 304.8;
        }

        public static double MMToFeet(this double mm)
        {
          return  mm / 304.8;
        }
    }
}
