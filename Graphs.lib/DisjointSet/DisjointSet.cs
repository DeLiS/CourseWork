using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphs.lib.DisjointSet
{
    public class DisjointSet<T>
        where T:IComparable<T>
    {
        private Dictionary<T, T> tree = new Dictionary<T, T>();
        public DisjointSet(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                tree[value] = value;
            }
        }
        public T Id(T value)
        {
            T current = value;
            if(tree[current].CompareTo(current)!=0)
            {
                var answer = Id(tree[current]);
                tree[current] = answer;
                return answer;
            }
            return current;
        }
        public void Reset()
        {
            foreach (var value in tree.Keys)
            {
                tree[value] = value;
            }
        }
        public void Join(T a, T b)
        {
            tree[Id(b)] = a;
        }
        public bool AreInOneSubset(T a, T b)
        {
            return Id(a).CompareTo(Id(b)) == 0;
        }

    }
}
