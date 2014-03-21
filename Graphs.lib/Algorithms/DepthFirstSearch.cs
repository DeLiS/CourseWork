using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphs.lib.DataStructure;
using Graphs.lib.Algorithms;
namespace Graphs.lib.Algorithms
{
    public class DepthFirstSearch<T>:IAlgorithm<T>
        where T:IComparable<T>
    {
        public Graph<T> Graph{get;set ;}

        protected Dictionary<T, int> TimeIn = new Dictionary<T, int>();
        protected Dictionary<T,int> TimeOut = new Dictionary<T, int>();
        protected Dictionary<T, Colours> VertexColour = new Dictionary<T, Colours>();
        protected Dictionary<T,T> Parent = new Dictionary<T, T>();
        protected int Timer = 0;
        public T Start { get; set; }
        public T Value { get; set; }
        protected Vertex<T> End { get; set; }
        public DepthFirstSearch()
        {
            End = null;
        }
        public DepthFirstSearch(Graph<T> graph,T start, T searchFor)
        {
            Start = start;
            Parent[Start] = Start;
            Value = searchFor;
            Graph = graph;
            End = null;
        }
        public List<T> Path(T value)
        {
            List<T> list = new List<T>();
            T cur = value;
            while(cur.CompareTo(Start)!=0)
            {
                list.Add(cur);
                cur = Parent[cur];
            }
            list.Add(cur);
            list.Reverse();
            return list;
        }
        public virtual void Run()
        {
            foreach (var vertex in Graph)
            {
                Colours colour;
                if(!VertexColour.TryGetValue(vertex,out colour))
                    DfsVisit(vertex);
            }

        }
        protected virtual void DfsVisit(T vertex)
        {
            Timer = Timer + 1;
            TimeIn[vertex] = Timer;
            VertexColour[vertex] = Colours.Gray;
            var vertexes = Graph.AdjacentVertexes(vertex);
            foreach (var adjacentVertex in vertexes)
            {
                if (adjacentVertex.Vertex.Value.CompareTo(Value) == 0)
                    End = adjacentVertex.Vertex;
                Colours colour;
                if(!VertexColour.TryGetValue(adjacentVertex.Vertex.Value,out colour))
                {
                    colour = Colours.White;
                }
                if(colour == Colours.White)
                {
                    Parent[adjacentVertex.Vertex.Value] = vertex;
                    DfsVisit(adjacentVertex.Vertex.Value);
                }
            }
            Timer = Timer + 1;
            TimeOut[vertex] = Timer;
            VertexColour[vertex] = Colours.Black;
        }
    }
}
