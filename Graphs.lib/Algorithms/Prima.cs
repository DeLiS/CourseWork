using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphs.lib.DataStructure;
using Graphs.lib.Heap;
namespace Graphs.lib.Algorithms
{
    public class Prima<T>:IAlgorithm<T>
        where T:IComparable<T>
    {
        class MyComparer:IComparer<double>
        {
            public int Compare(double x, double y)
            {
                return x.CompareTo(y);
            }
        }
        public Graph<T> Graph { get; set; }
        protected T Start { get; set; }
        protected Heap<double, T> heap = new Heap<double, T>(new MyComparer());
        protected Dictionary<T,T> VertexParent = new Dictionary<T, T>();
        protected int Count = 1;
        protected Dictionary<T, bool> Used = new Dictionary<T, bool>();
        protected T Prev { get; set; }
        public double Weight { get; private set; }
        public Prima(Graph<T> g, T start)
        {
            Graph = g;
            Weight = 0;
            double infinity = 0;
            foreach (var value in Graph)
            {
                Used[value] = false;
            }
            foreach (var edge in Graph.Edges)
            {
                var args = edge.ConnectionInfo as WeightedConnectionArgs<T>;
                if(args!=null)
                {
                    infinity += Math.Abs(args.Weight);
                }
            }
            
            Start = start;
            Used[start] = true;
            VertexParent.Add(start,start);
            foreach (var value in Graph)
            {
                heap.Push(value,infinity);
            }
            foreach (var adjacentVertex in Graph.AdjacentVertexes(start))
            {
                var weightedConnectionArgs = adjacentVertex.Args as WeightedConnectionArgs<T>;
                if(weightedConnectionArgs!=null)
                {
                    VertexParent[adjacentVertex.Vertex.Value] = Start;
                    heap.DecreaseKey(adjacentVertex.Vertex.Value, weightedConnectionArgs.Weight);
                }
                else
                {
                    throw new Exception();
                }
            }
            Prev = Start;
        }
        public void Run()
        {
            while(Count<Graph.VertexesCount)
            {
                while(heap.Count>0&&Used[heap.Top])
                    heap.Pop();
                if (heap.Count == 0)
                    break;
                var current = heap.Top;
                heap.Pop();
                var edge = Graph.GetEdge(VertexParent[current], current);
                var edgeargs = edge.ConnectionInfo as WeightedConnectionArgs<T>;
                if (edgeargs != null)
                {
                    Weight += edgeargs.Weight;
                }
                Used[current] = true;
                foreach (var value in Graph.AdjacentVertexes(current))
                {
                   if(!Used[value.Vertex.Value])
                   {
                       WeightedConnectionArgs<T> args = value.Args as WeightedConnectionArgs<T>;
                       if(args!=null)
                       {
                           bool noException = true;
                           try
                           {
                               heap.DecreaseKey(value.Vertex.Value, args.Weight);
                           }
                           catch(Exception e)
                           {
                               if (e.Message != "Wrong key")
                                   throw e;
                               noException = false;
                           }
                           if(noException)
                           {
                               VertexParent[value.Vertex.Value] = current;
                           }
                       } 
                   }
                }
            }
        }
    }
}
