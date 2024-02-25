using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_NSGA2
{
    /// <summary>
    /// NSGA2辅助工具类
    /// </summary>
    internal static class NSGA2Utils
    {
        /// <summary>
        /// 染色体集合拷贝
        /// </summary>
        /// <param name="individuals"></param>
        /// <returns></returns>
        public static List<Individual> DeepCloneInds(this IEnumerable<Individual> individuals)
        {
            List<Individual> clones = new List<Individual>();
            foreach (var ind in individuals)
            {
                var clone = ind.DeepCloneInd();
                clones.Add(clone);
            }
            return clones;
        }
    }
}
