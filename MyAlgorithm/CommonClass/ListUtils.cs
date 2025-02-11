using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass
{
    public static class ListUtils
    {
        /// <summary>
        /// 拍平集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> Flatten<T>(this IEnumerable<IEnumerable<T>> list)
        {
            return list.SelectMany(x => x).ToList();
        }

        /// <summary>
        /// 递归分组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<List<T>> ChainGroupBy<T>(this List<T> list, Func<T, T, bool> predicate)
        {
            List<List<T>> groups = new List<List<T>>();
            while (list.Count > 0)
            {
                List<T> group = new List<T>() { list[0] };
                for (int i = 1; i < list.Count; i++)
                {
                    if (predicate(list[0], list[i]))
                    {
                        group.Add(list[i]);
                    }
                }
                groups.Add(group);
                group.ForEach(p => list.Remove(p));
            }
            return groups;
        }

    }
}
