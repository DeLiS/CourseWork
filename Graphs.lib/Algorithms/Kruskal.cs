using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphs.lib.Heap;
using Graphs.lib.DisjointSet;
using Graphs.lib.DataStructure;
namespace Graphs.lib.Algorithms
{
    public class Kruskal<T>:IAlgorithm<T>
        where T:IComparable<T>
    {
        class MyCmp:IComparer<double>
        {
            public int Compare(double x, double y)
            {
                return x.CompareTo(y);
            }
        }
        public Graph<T> Graph { get; set; }
        protected DisjointSet<T> disjointSet;
        protected Heap<double, Edge<T>> heap;
        protected Dictionary<Edge<T>,double> Edges = new Dictionary<Edge<T>, double>();
        public double Weight { get; private set; }
        public Kruskal(Graph<T> graph)
        {
            Graph = graph;
            disjointSet = new DisjointSet<T>(graph.Values);
            heap = new Heap<double, Edge<T>>(new MyCmp());
            foreach (var edge in Graph.Edges)
            {
                var args = edge.ConnectionInfo as WeightedConnectionArgs<T> ;
                if (args != null)
                    heap.Push(edge,args.Weight);
            }
            Weight = 0;
        }
        public void Run()
        {
            int n = Graph.VertexesCount - 1;
            while(n!=0)
            {
                var cur = heap.Top;
                heap.Pop();
                if(!disjointSet.AreInOneSubset(cur.Start.Value,cur.End.Value))
                {
                   
                    var args = cur.ConnectionInfo as WeightedConnectionArgs<T>;
                    if(args!=null)
                    {
                         --n;
                        Edges[cur] = args.Weight;
                        disjointSet.Join(cur.Start.Value, cur.End.Value);
                        Weight += args.Weight;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
        }
    }
}
