using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_Sort
{
    internal class Sort
    {
        public int[] nums;
        public Sort(int[] arr)
        {
            nums = arr;
        }

        /// <summary>
        /// 简单排序（直接插入排序），稳定
        /// 思路：与前面的有序数组比较
        /// 第二个for循环代表前面的有序数组
        /// 时间复杂度：n²
        /// </summary>
        public void InsertSort()
        {
            int i;int j;int temp;
            for ( i = 1; i < nums.Length; i++)
            {
                //与前面的一个数比较
                if (nums[i] < nums[i - 1])
                {
                    temp = nums[i];
                    nums[i] = nums[i - 1];
                    //与前面的有序数组比较
                    for (j = i - 1; j >= 0&&nums[j]>temp; j--)
                    {
                        //当前j大于temp，j朝后移动一位
                        nums[j + 1] = nums[j];
                        nums[j ] = temp;
                    }
                }

            }
        }

        /// <summary>
        /// 冒泡排序，稳定
        /// 思路：两两之间比较
        /// 第二个for循环代表比较的趟数
        /// 时间复杂度：n²
        /// </summary>
        public void BubbleSort()
        {
            for (int i = 0; i < nums.Length-1; i++)
            {
                for (int j = 0; j < nums.Length - 1-i; j++)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        int temp = nums[j];
                        nums[j] = nums[j + 1];
                        nums[j + 1] = temp;                        
                    }
                }
            }
        }

        /// <summary>
        /// 选择排序，不稳定
        /// 思路：每次找到最小的元素放桶里
        /// 第二个for循环代表当前元素后面需要比较的元素
        /// 时间复杂度：n²
        /// </summary>
        public void SelectSort()
        {
            int i, j, temp;
            int k;//k作为最小元素下表的指针
            for ( i = 0; i < nums.Length - 1; i++)
            {
                k = i;//起始时，让k指向i
                for ( j = i+1; j < nums.Length; j++)
                {
                    //后一个元素比前一个元素小的话
                    if (nums[j] < nums[k])
                    {
                        k = j;//k指向更小的元素的下标
                        if (k != i)
                        {
                            //交换元素位置
                            temp = nums[i];
                            nums[i] = nums[k];
                            nums[k] = temp;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 希尔排序，不稳定
        /// 思路：根据步长分组，每组进行插入排序，直到步长递减到1
        /// 时间复杂度：n1.3
        /// </summary>
        public void ShellSort()
        {

        }



    }
}
