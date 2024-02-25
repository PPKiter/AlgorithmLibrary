using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_NSGA2
{
    /// <summary>
    /// Pareto解集
    /// </summary>
    internal class Pareto
    {
        /// <summary>
        /// Pareto解
        /// </summary>
        public List<Individual> Individuals { get; set; }
        /// <summary>
        /// 根据目标函数1排序
        /// </summary>
        public List<Individual> SortFunc1 { get; set; }
        /// <summary>
        /// 根据目标函数2排序
        /// </summary>
        public List<Individual> SortFunc2 { get; set; }

        public Pareto(List<Individual> individuals)
        {
            Individuals = individuals;
        }

        /// <summary>
        /// 根据目标函数排序
        /// </summary>
        public void SortFunc()
        {
            SortFunc1=Individuals.OrderBy(p=>p.Function1).ToList();
            SortFunc2 = Individuals.OrderBy(p => p.Function2).ToList();
        }
    }
}
