using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_IGA
{
    /// <summary>
    /// 基因
    /// </summary>
    internal class Gene
    {
        /// <summary>
        /// 变量范围
        /// </summary>
        public int[] Range { get;set; }
        /// <summary>
        /// 变量值
        /// </summary>
        public int Value { get;set; }

        public Gene()
        {
             
        }

        /// <summary>
        /// 解码函数
        /// </summary>
        private void Decode()
        {
            
        }

        /// <summary>
        /// 基因拷贝
        /// </summary>
        /// <returns></returns>
        public Gene DeepCloneGene()
        {
            Gene clone= new Gene();
            clone.Range = Range;
            clone.Value = Value;
            return clone;
        }
    }
}
