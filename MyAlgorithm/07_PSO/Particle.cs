using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_PSO
{
    /// <summary>
    /// 粒子
    /// </summary>
    internal class Particle
    {
        // 粒子的位置、速度和个体最优位置
        public double[] Position { get; set; }
        public double[] Velocity { get; set; }
        public double[] PersonalBestPosition { get; set; }
        public double PersonalBestValue { get; set; }
    }
}
