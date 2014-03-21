using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphs.lib.Algorithms;
using Graphs.lib.DataStructure;
using Graphs.lib.Heap;
namespace Graphs.lib.Algorithms
{
    public class TopologicalSort<T>:DepthFirstSearch<T>
        where T:IComparable<T>
    {
        
        class TimeComparer:IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return y.CompareTo(x);
            }
        }

        IHeap<int,T> Heap = new Heap<int, T>(new TimeComparer());
        protected List<T> Order;
        public TopologicalSort(Graph<T> graph,T anyValue)
        {
            Graph = graph;
            Start = anyValue;
            Value = anyValue;
        }
        public override void Run()
        {
            base.Run();
            Graph<T> graph = Graph.Transpose();
            foreach (var vertex in graph)
            {
                Heap.Push(vertex,TimeOut[vertex]);
            }
            while(Heap.Count>0)
            {
                var currentVertex = Heap.Top;
                Heap.Pop();
                if(VertexColour[currentVertex]==Colours.White)
                    DfsVisit(currentVertex);
            }
        }
        public T[] Result()
        {
            var x = new SortedList<int, T>();
            foreach (var value in Graph)
            {
                x.Add(-TimeOut[value],value);
            }
            return x.Values.ToArray();
        }
    }
    
}
