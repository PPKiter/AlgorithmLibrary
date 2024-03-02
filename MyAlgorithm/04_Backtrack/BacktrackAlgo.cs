using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_Backtrack
{
    internal class BacktrackAlgo<T> where T : class
    {
        /// <summary>
        /// 冲突检查：用于剪枝函数中的剪枝操作
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="existing">个体1，前序</param>
        /// <param name="candidate">个体2，当前</param>
        /// <returns></returns>
        public delegate bool ConflictCheckerDelegate<S>(S existing, S candidate);

        private int _max;
        //分组列表
        private List<List<T>> _groups;

        /// <summary>
        /// 当前状态下的一条路径结果
        /// </summary>
        private Stack<T> Result { get; set; }

        public List<List<T>> Results { get; set; } = new List<List<T>>();

        /// <summary>
        /// 冲突函数
        /// </summary>
        private ConflictCheckerDelegate<T> ConflictChecker { get; set; }

        Func<T, T, bool> Conflict2 { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="groups">待回溯列表，第一层list是所有分组，第二层list是每个分组有多少个体</param>
        /// <param name="conflictChecker"></param>
        public BacktrackAlgo(List<List<T>> groups, ConflictCheckerDelegate<T> conflictChecker)
        {
            _max = groups.Count - 1;
            _groups = groups;
            ConflictChecker = conflictChecker;
        }


        /// <summary>
        /// 算法入口
        /// </summary>
        /// <param name="col"></param>
        public void Backtrack(int col)
        {
            //达到最后一列分组，返回
            if (col == _max)
            {
                return;
            }
            foreach (var group in _groups[col])
            {
                Result = new Stack<T>();
                col = 0;
                Result.Push(group);
                Check(ref col);
            }
        }

        /// <summary>
        /// 检查每一项
        /// </summary>
        /// <param name="start"></param>
        private void Check(ref int start)
        {
            if (start == _max)
            {
                //按顺序输出
                var result = Result.Reverse().Cast<T>().ToList();
                Results.Add(result);
                Result.Pop();
                start--;
                return;
            }
            var nextGroup = _groups[start + 1];
            for (int i = 0; i < nextGroup.Count; i++)
            {
                if (Judge(nextGroup[i]))
                {
                    Result.Push(nextGroup[i]);
                    start++;
                    Check(ref start);
                }
                //一层遍历完毕后，弹出栈顶元素
                Result.Pop();
                start--;
            }
        }

        /// <summary>
        /// 剪枝函数：判断当前元素是否与路径前序结果冲突
        /// true:检查通过；false：不通过
        /// </summary>
        /// <param name="cur"></param>
        /// <returns></returns>
        private bool Judge(T cur)
        {
            var exists = Result.ToList();
            foreach (var exist in exists)
            {
                //冲突检查
                if (ConflictChecker(exist, cur))
                {
                    return false;
                }

            }
            return true;
        }
    }
}
