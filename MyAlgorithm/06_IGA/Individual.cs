using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_IGA
{
    /// <summary>
    /// 染色体
    /// </summary>
    internal class Individual
    {

        /// <summary>
        /// 基因
        /// </summary>
        public Gene[] Genes { get; set; }
        /// <summary>
        /// 目标函数值
        /// </summary>
        public double Function { get; set; }
        /// <summary>
        /// 基因排成一行，方便调试时查看
        /// </summary>
        public string GeneRow { get; set; }
        /// <summary>
        /// 目标函数
        /// </summary>
        public Func<double[], double> Func { get; set; }
        public Individual(Gene[] genes, Func<double[], double> func)
        {
            Genes = genes;
            Func = func;
        }

        public void CalVirtualFitnes()
        {
            var xs= Genes.Select(p => p.Value).ToArray();
            Function = Func(xs);
        }
        /// <summary>
        /// 个体克隆
        /// </summary>
        /// <returns></returns>
        public Individual DeepCloneInd()
        {
            var cloneGen = Genes.Select(p => p.DeepCloneGene()).ToArray();
            Individual clone = new Individual(cloneGen,Func);
            clone.Function = Function;
            return clone;

        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <returns></returns>
        public double[] Decode()
        {
           return Genes.Select(p => p.Value).ToArray();
        }
    }
}
