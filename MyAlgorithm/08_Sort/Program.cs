using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_Sort
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] nums = { 2, 3, 4, 1, 5, 8, 7, 6, 9 };
            Sort sort = new Sort(nums);
            ////1.冒泡
            //sort.BubbleSort();
            ////2.插入排序
            //sort.InsertSort();
            //3.选择排序
            sort.SelectSort();

            foreach (int i in sort.nums)
            {
                Console.WriteLine(i);
            }

            Console.ReadKey();
        }
    }
}
