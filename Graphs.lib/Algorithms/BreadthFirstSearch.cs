using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphs.lib.DataStructure;

namespace Graphs.lib.Algorithms
{
    public class BreadthFirstSearch<T>:IAlgorithm<T>
        where T:IComparable<T>
    {
        public Graph<T> Graph{get; set; }
        public T Start { get; private set; }
        public T Value { get; private set; }
        public Vertex<T> End { get; private set; }
        protected Queue<T> queue;
        protected Dictionary<T, bool> Used;
        protected Dictionary<T,int> Distances;
        protected Dictionary<T, T> Parents;
        protected bool Found { get; set; }
        public BreadthFirstSearch(Graph<T> g,T start, T searchFor)
        {
            Graph = g;
            Value = searchFor;
            Start = start;
        }
        public virtual void Run()
        {
            Found = false;
            Distances = new Dictionary<T, int>(Graph.VertexesCount);
            Distances[Start] = 0;
            Used = new Dictionary<T, bool>(Graph.VertexesCount);
            Used[Start] = true;
            Parents = new Dictionary<T, T>(Graph.VertexesCount);
            queue = new Queue<T>(Graph.VertexesCount);
            queue.Enqueue(Start);
            while(queue.Count>0)
            {
                var cur = queue.Dequeue();
                var vertexes = Graph.AdjacentVertexes(cur);
                foreach (var adjacentVertex in vertexes)
                {
                    bool used;
                    if (!Used.TryGetValue(adjacentVertex.Vertex.Value, out used))
                        used = false;
                    if(used==false)
                    {
                        Used[adjacentVertex.Vertex.Value] = true;
                        int distance;
                        if (!Distances.TryGetValue(adjacentVertex.Vertex.Value, out distance))
                            distance = Distances[cur] + 1;
                        if (distance >= Distances[cur] + 1)
                        {
                            Distances[adjacentVertex.Vertex.Value] = Distances[cur] + 1;
                            Parents[adjacentVertex.Vertex.Value] = cur;
                        }
                        queue.Enqueue(adjacentVertex.Vertex.Value);
                        if (adjacentVertex.Vertex.Value.CompareTo(Value) == 0)
                        {
                            Found = true;
                            End = adjacentVertex.Vertex;
                        }
                    }
                
                }
            }
        }
        public int GetDistance(T value)
        {
            int distance;
            if (!Distances.TryGetValue(value, out distance))
                distance = -1;
            return distance;
        }
    }
}
