using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_IGA
{
    internal class GAFunctions
    {
        /// <summary>
        /// Sphere Function（球函数）
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double SphereFunction(double[] x)
        {
            double sum = 0;
            foreach (double value in x)
            {
                sum += value * value;
            }
            return sum;
        }

        /// <summary>
        /// Rastrigin Function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double RastriginFunction(double[] x)
        {
            double sum = 0;
            foreach (double value in x)
            {
                sum += value * value - 10 * Math.Cos(2 * Math.PI * value);
            }
            return 10 * x.Length + sum;
        }

        /// <summary>
        /// Rosenbrock Function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double RosenbrockFunction(double[] x)
        {
            double sum = 0;
            for (int i = 0; i < x.Length - 1; i++)
            {
                double temp = 100 * Math.Pow(x[i + 1] - x[i] * x[i], 2) + Math.Pow(1 - x[i], 2);
                sum += temp;
            }
            return sum;
        }

        /// <summary>
        /// Ackley Function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double AckleyFunction(double[] x)
        {
            double sum1 = 0;
            double sum2 = 0;
            int n = x.Length;

            foreach (double value in x)
            {
                sum1 += value * value;
                sum2 += Math.Cos(2 * Math.PI * value);
            }

            return -20 * Math.Exp(-0.2 * Math.Sqrt(sum1 / n)) - Math.Exp(sum2 / n) + 20 + Math.E;
        }

        /// <summary>
        /// Griewank Function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GriewankFunction(double[] x)
        {
            double sum1 = 0;
            double product = 1;

            for (int i = 0; i < x.Length; i++)
            {
                sum1 += x[i] * x[i] / 4000;
                product *= Math.Cos(x[i] / Math.Sqrt(i + 1));
            }

            return sum1 - product + 1;
        }
    }
}
