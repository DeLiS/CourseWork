using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphs.lib.Heap;
using Graphs.lib.DataStructure;
namespace Graphs.lib.Algorithms
{
    public class Dijkstra<T>:IAlgorithm<T>
        where T:IComparable<T>
    {
        class VertexCmp:IComparer<double>
        {
            public int Compare(double x, double y)
            {
                return x.CompareTo(y);
            }
        }

        public Graph<T> Graph
        {
            get;
            set;
        }
        private Dictionary<T,double> Distance = new Dictionary<T, double>();
        private Dictionary<T,T> Parent = new Dictionary<T, T>();
        private Dictionary<T,bool> Used = new Dictionary<T, bool>();
        private Heap<double, T> heap = new Heap<double, T>(new VertexCmp());
        public T Start { get; set; }

        public Dijkstra(Graph<T> g, T start)
        {
            Graph = g;
            Start = start;
        }
        public void Run()
        {
            Used[Start] = true;
            Distance[Start] = 0;
            heap.Push(Start,Distance[Start]);
            while(heap.Count>0)
            {
                var current = heap.Top;
                heap.Pop();
                Used[current] = true;
                foreach (var adjacentVertex in Graph.AdjacentVertexes(current))
                {
                    var args = adjacentVertex.Args as WeightedConnectionArgs<T>;
                    if (args != null)
                    {
                        var next = adjacentVertex.Vertex.Value;
                        Relax(current, next, args.Weight);
                    }
                }
            }
        }
        public void Relax(T start, T end, double w)
        {
            double startDistance;
            double endDistance;
            if (!Distance.TryGetValue(start, out startDistance))
                return;// false;
            if (!Distance.TryGetValue(end, out endDistance))
            {
                Distance[end] = startDistance + w;
                Parent[end] = start;
                bool used;
                if (!Used.TryGetValue(end, out used))
                {
                    heap.Push(end, startDistance + w);
                    Used[end] = true;
                }
                else
                {
                    heap.DecreaseKey(end, startDistance + w);
                }
                return;// true;
            }
            if(endDistance >= startDistance + w)
            {
                Distance[end] = startDistance + w;
                Parent[end] = start;
                bool used;
                if (!Used.TryGetValue(end, out used))
                {
                    heap.Push(end, startDistance + w);
                    Used[end] = true;
                }
                else
                {
                    heap.DecreaseKey(end, startDistance + w);
                }
                return;// true;
            }
            return;// false;
        }
        public double GetDistance(T dest)
        {
            return Distance[dest];
        }
    }
}
