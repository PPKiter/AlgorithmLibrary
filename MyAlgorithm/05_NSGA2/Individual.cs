using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_NSGA2
{
    /// <summary>
    /// 染色体
    /// </summary>
    internal class Individual
    {
        /// <summary>
        /// 当前个体被支配的数量
        /// </summary>
        public int DomintedCount { get; set; }
        /// <summary>
        /// 当前个体支配了哪些个体 
        /// </summary>
        public List<Individual> DominateInds { get; set; } = new List<Individual> { };
        /// <summary>
        /// Pareto等级
        /// </summary>
        public int ParetoRank { get; set; } = 10000;
        /// <summary>
        /// 拥挤度
        /// </summary>
        public double CrowdingRate { get; set; }
        /// <summary>
        /// 用来计算拥挤度而定义的集合
        /// </summary>
        public List<double> Crowdings { get; set; } = new List<double>();
        /// <summary>
        /// 基因
        /// </summary>
        public Gene[] Genes { get; set; }
        /// <summary>
        /// 目标函数1
        /// </summary>
        public double Function1 { get; set; }
        /// <summary>
        /// 目标函数2
        /// </summary>
        public double Function2 { get; set; }
        /// <summary>
        /// 基因排成一行，方便调试时查看
        /// </summary>
        public string GeneRow { get; set; }
        public Individual(Gene[] genes)
        {
            Genes = genes;
        }

        public void CalVirtualFitnes()
        {
            
        }
        /// <summary>
        /// 个体克隆
        /// </summary>
        /// <returns></returns>
        public Individual DeepCloneInd()
        {
            var cloneGen=Genes.Select(p=>p.DeepCloneGene()).ToArray();
            Individual clone = new Individual(cloneGen);
            clone.ParetoRank = ParetoRank;
            clone.CrowdingRate = CrowdingRate;
            clone.Crowdings = Crowdings;
            clone.DominateInds = DominateInds;
            clone.DomintedCount = DomintedCount;
            clone.Function1 = Function1;
            clone.Function2 = Function2;

            return clone;

        }
    }
}
