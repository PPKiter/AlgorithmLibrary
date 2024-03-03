using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_PSO
{
    /// <summary>
    /// 粒子群算法
    /// 参考文献：https://zhuanlan.zhihu.com/p/346355572
    /// </summary>
    internal class PSO
    {
        private int numParticles; // 粒子数量
        private int numDimensions; // 搜索空间维度
        public double[] globalBestPosition; // 全局最优位置
        public double globalBestValue; // 全局最优值
        private Particle[] particles; // 粒子数组

        public PSO(int numParticles, int numDimensions)
        {
            this.numParticles = numParticles;
            this.numDimensions = numDimensions;
            this.globalBestPosition = new double[numDimensions];
            this.globalBestValue = double.MaxValue;
            this.particles = new Particle[numParticles];
        }

        /// <summary>
        /// 粒子群算法主程序
        /// </summary>
        /// <param name="maxIterations">最大迭代数</param>
        /// <returns></returns>
        public double[] Main(int maxIterations)
        {
            //初始化
            Initialize();
            for (int i = 0; i < maxIterations; i++)
            {
                Update();
            }
            return globalBestPosition;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            Random rand = new Random();
            for (int i = 0; i < numParticles; i++)
            {
                particles[i] = new Particle
                {
                    Position = new double[numDimensions],
                    Velocity = new double[numDimensions],
                    PersonalBestPosition = new double[numDimensions],
                    PersonalBestValue = double.MaxValue
                };

                // 初始化粒子位置和速度
                for (int j = 0; j < numDimensions; j++)
                {
                    particles[i].Position[j] = rand.NextDouble() * 10; // 初始化粒子位置在[0, 10]范围内
                    particles[i].Velocity[j] = rand.NextDouble() * 0.1; // 初始化粒子速度
                    particles[i].PersonalBestPosition[j] = particles[i].Position[j];
                }

                // 计算粒子的适应度，更新个体最优位置和全局最优位置
                double value = Evaluate(particles[i].Position);
                if (value < particles[i].PersonalBestValue)
                {
                    particles[i].PersonalBestValue = value;
                    Array.Copy(particles[i].Position, particles[i].PersonalBestPosition, numDimensions);
                }

                if (value < globalBestValue)
                {
                    globalBestValue = value;
                    Array.Copy(particles[i].Position, globalBestPosition, numDimensions);
                }
            }
        }

        /// <summary>
        /// 适应度函数（选用Sphere Function进行测试）
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public double Evaluate(double[] position)
        {
            // Sphere Function（球函数）的计算
            double sum = 0;
            foreach (double value in position)
            {
                sum += value * value;
            }
            return sum;
        }

        /// <summary>
        /// 个体更新
        /// </summary>
        public void Update()
        {
            Random rand = new Random();
            double w = 0.5; // 惯性权重
            double c1 = 1.5; // 个体学习因子
            double c2 = 1.5; // 社会学习因子

            for (int i = 0; i < numParticles; i++)
            {
                for (int j = 0; j < numDimensions; j++)
                {
                    // 更新粒子速度
                    double r1 = rand.NextDouble();
                    double r2 = rand.NextDouble();
                    particles[i].Velocity[j] = w * particles[i].Velocity[j] +
                                               c1 * r1 * (particles[i].PersonalBestPosition[j] - particles[i].Position[j]) +
                                               c2 * r2 * (globalBestPosition[j] - particles[i].Position[j]);
                    // 更新粒子位置
                    particles[i].Position[j] += particles[i].Velocity[j];
                }

                // 计算粒子适应度，更新个体最优位置和全局最优位置
                double value = Evaluate(particles[i].Position);
                if (value < particles[i].PersonalBestValue)
                {
                    particles[i].PersonalBestValue = value;
                    Array.Copy(particles[i].Position, particles[i].PersonalBestPosition, numDimensions);
                }

                if (value < globalBestValue)
                {
                    globalBestValue = value;
                    Array.Copy(particles[i].Position, globalBestPosition, numDimensions);
                }
            }
        }


    }
}
