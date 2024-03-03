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
        /// 变量范围(下限和上限)
        /// </summary>
        public double[] Range { get; set; }
        /// <summary>
        /// 变量值
        /// </summary>
        public double Value { get; set; }

        public Gene()
        {
            Range = new double[] { -10.0, 10.0 };
        }

        /// <summary>
        /// 给变量赋值
        /// </summary>
        public void GetValue()
        {
            //生成随机种子
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int seed = BitConverter.ToInt32(buffer, 0);
            Random random = new Random(seed);
            Value =Math.Round( Range[0] + (Range[1] - Range[0]) * random.NextDouble(),2);
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
            Gene clone = new Gene();
            clone.Range = Range;
            clone.Value = Value;
            return clone;
        }
    }
}
