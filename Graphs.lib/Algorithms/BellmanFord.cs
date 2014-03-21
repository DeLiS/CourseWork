using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphs.lib.DataStructure;

namespace Graphs.lib.Algorithms
{
    public class BellmanFord<T>:IAlgorithm<T>
        where T:IComparable<T>
    {
        private Graph<T> _graph;
        private Dictionary<T, double> Distance = new Dictionary<T, double>();
        private Dictionary<T, T> Parent = new Dictionary<T, T>();
        public bool HasNegativeCycles { get; private set; }
        public Graph<T> Graph
        {
            get { return _graph; }
            set { _graph = value; }
        }
        public BellmanFord(Graph<T> graph,T start)
        {
            HasNegativeCycles = false;
            Graph = graph;
            Distance[start] = 0;
        }
        public void Run()
        {
            int vertexCount = Graph.VertexesCount;
            for(int i=0;i<vertexCount-1;++i)
            {
                foreach (var vertex in Graph)
                {
                    foreach (var adjacentVertex in Graph.AdjacentVertexes(vertex))
                    {
                        var args = adjacentVertex.Args as WeightedConnectionArgs<T>;
                        if(args==null)
                            throw new Exception();
                        Relax(vertex, adjacentVertex.Vertex.Value,args.Weight);
                    }
                }
            }
            foreach (var vertex in Graph)
            {
                foreach (var adjacentVertex in Graph.AdjacentVertexes(vertex))
                {
                    var args = adjacentVertex.Args as WeightedConnectionArgs<T>;
                    if (args == null)
                        throw new Exception();
                    if (Relax(vertex, adjacentVertex.Vertex.Value, args.Weight))
                    {
                        HasNegativeCycles = true;
                        return;
                    }
                }
            }
            return;
        }
        public bool Relax(T start, T end, double w)
        {
            double startDistance;
            double endDistance;
            if (!Distance.TryGetValue(start, out startDistance))
                return false;
            if (!Distance.TryGetValue(end, out endDistance))
            {
                Distance[end] = startDistance + w;
                Parent[end] = start;
                return true;
            }
            if (endDistance > startDistance + w)
            {
                Distance[end] = startDistance + w;
                Parent[end] = start;
                return true;
            }
            return false;
        }
        public double GetDistance(T i)
        {
            return Distance[i];
        }
    }
}
