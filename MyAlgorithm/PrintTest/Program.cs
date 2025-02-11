using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrintTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //List<int> nums = new List<int>();
            //for (int i = 0; i < 1000; i++)
            //{
            //    nums.Add(i);
            //}

            //List<List<int>> groups = new List<List<int>>();
            //for (int i = 0; i < 10; i++)
            //{
            //    var group = nums.GetRange(i * 100, 100);
            //    groups.Add(group);
            //}

            //var last = nums.GetRange(900, nums.Count - 900);
            //bool has = nums.Contains(5);

            //Console.WriteLine();

            //string layerPath = @"C:\D\prusaSlicerTest01\PrusaSlicer\build\src\data.txt";
            //string ss = "0\r\n2e+07\r\n2e+07\r\n3.5e+07\r\n3.5e+07\r\n5e+07\r\n5e+07\r\n6.5e+07\r\n6.5e+07\r\n8e+07\r\n8e+07\r\n9.5e+07\r\n9.5e+07\r\n1.1e+08\r\n1.1e+08\r\n1.25e+08\r\n1.25e+08\r\n1.4e+08\r\n1.4e+08\r\n1.55e+08\r\n1.55e+08\r\n1.7e+08\r\n1.7e+08\r\n1.85e+08\r\n1.85e+08\r\n2e+08\r\n2e+08\r\n2.15e+08\r\n2.15e+08\r\n2.3e+08\r\n2.3e+08\r\n2.45e+08\r\n2.45e+08\r\n2.6e+08\r\n2.6e+08\r\n2.75e+08\r\n2.75e+08\r\n2.9e+08\r\n2.9e+08\r\n3.05e+08\r\n3.05e+08\r\n3.2e+08\r\n3.2e+08\r\n3.35e+08\r\n3.35e+08\r\n3.5e+08\r\n3.5e+08\r\n3.65e+08\r\n3.65e+08\r\n3.8e+08\r\n3.8e+08\r\n3.95e+08\r\n3.95e+08\r\n4.1e+08\r\n4.1e+08\r\n4.25e+08\r\n4.25e+08\r\n4.4e+08\r\n4.4e+08\r\n4.55e+08\r\n4.55e+08\r\n4.7e+08\r\n4.7e+08\r\n4.85e+08\r\n4.85e+08\r\n5e+08\r\n5e+08\r\n5.15e+08\r\n5.15e+08\r\n5.3e+08\r\n5.3e+08\r\n5.45e+08\r\n5.45e+08\r\n5.6e+08\r\n5.6e+08\r\n5.75e+08\r\n5.75e+08\r\n5.9e+08\r\n5.9e+08\r\n6.05e+08\r\n6.05e+08\r\n6.2e+08\r\n6.2e+08\r\n6.35e+08\r\n6.35e+08\r\n6.5e+08\r\n6.5e+08\r\n6.65e+08\r\n6.65e+08\r\n6.8e+08\r\n6.8e+08\r\n6.95e+08\r\n6.95e+08\r\n7.1e+08\r\n7.1e+08\r\n7.25e+08\r\n7.25e+08\r\n7.4e+08\r\n7.4e+08\r\n7.55e+08\r\n7.55e+08\r\n7.7e+08\r\n7.7e+08\r\n7.85e+08\r\n7.85e+08\r\n8e+08\r\n8e+08\r\n8.15e+08\r\n8.15e+08\r\n8.3e+08\r\n8.3e+08\r\n8.45e+08\r\n8.45e+08\r\n8.6e+08\r\n8.6e+08\r\n8.75e+08\r\n8.75e+08\r\n8.9e+08\r\n8.9e+08\r\n9.05e+08\r\n9.05e+08\r\n9.2e+08\r\n9.2e+08\r\n9.35e+08\r\n9.35e+08\r\n9.5e+08\r\n9.5e+08\r\n9.65e+08\r\n9.65e+08\r\n9.8e+08\r\n9.8e+08\r\n9.95e+08\r\n9.95e+08\r\n1.01e+09\r\n1.01e+09\r\n1.025e+09\r\n1.025e+09\r\n1.04e+09\r\n1.04e+09\r\n1.055e+09\r\n1.055e+09\r\n1.07e+09\r\n1.07e+09\r\n1.085e+09\r\n1.085e+09\r\n1.1e+09\r\n1.1e+09\r\n1.115e+09\r\n1.115e+09\r\n1.13e+09\r\n1.13e+09\r\n1.145e+09\r\n1.145e+09\r\n1.16e+09\r\n1.16e+09\r\n1.175e+09\r\n1.175e+09\r\n1.19e+09\r\n1.19e+09\r\n1.205e+09\r\n1.205e+09\r\n1.22e+09\r\n1.22e+09\r\n1.235e+09\r\n1.235e+09\r\n1.25e+09\r\n1.25e+09\r\n1.265e+09\r\n1.265e+09\r\n1.28e+09\r\n1.28e+09\r\n1.295e+09\r\n1.295e+09\r\n1.31e+09\r\n1.31e+09\r\n1.325e+09\r\n1.325e+09\r\n1.34e+09\r\n1.34e+09\r\n1.355e+09\r\n1.355e+09\r\n1.37e+09\r\n1.37e+09\r\n1.385e+09\r\n1.385e+09\r\n1.4e+09\r\n1.4e+09\r\n1.415e+09\r\n1.415e+09\r\n1.43e+09\r\n1.43e+09\r\n1.445e+09\r\n1.445e+09\r\n1.46e+09\r\n1.46e+09\r\n1.475e+09\r\n1.475e+09\r\n1.49e+09\r\n1.49e+09\r\n1.505e+09\r\n";
            //List<int> nums = new List<int>();
            //using (StreamReader reader = new StreamReader(layerPath))
            //{
            //    string line;
            //    while ((line = reader.ReadLine()) != null)
            //    {
            //        // 尝试将字符串转换为整数
            //        if (int.TryParse(line, out int number))
            //        {
            //            nums.Add(number);
            //        }
            //    }
            //}

            //// 定义正则表达式来匹配数字（包括科学记数法）
            //string pattern = @"[-+]?\d*\.?\d+([eE][-+]?\d+)?";

            //// 使用Regex.Matches找到所有匹配的数字
            //MatchCollection matches = Regex.Matches(ss, pattern);
            //List<double> numdoubles = new List<double>();
            //foreach (Match match in matches)
            //{
            //    string numberStr = match.Value;
            //    double number;

            //    // 尝试将字符串转换为双精度浮点数
            //    if (double.TryParse(numberStr, out number))
            //    {
            //        numdoubles.Add(number);
            //    }
            //}


            //读取.txt文件测试
            string filePath = "C:\\Users\\Arthur.Zhuang\\Desktop\\non_debug\\lin4\\process\\entities.txt";
            List<PointF> points = new List<PointF>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        if (values.Length == 2 &&
                            float.TryParse(values[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
                            float.TryParse(values[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
                        {
                            points.Add(new PointF(x, y));
                        }
                        else
                        {
                            Console.WriteLine($"无法解析行: {line}");
                        }
                    }
                }

                Console.WriteLine("解析结果：");
                foreach (var point in points)
                {
                    Console.WriteLine(point);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取文件时出错: {ex.Message}");
            }
        }
    }

    class PointF
    {
        public float X { get; set; }
        public float Y { get; set; }

        public PointF(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
