using System;

namespace Graphs.lib.DataStructure
{
    public class Edge<T>:IEdge<T>,IComparable<Edge<T>>
        where T : IComparable<T>
    {
        public Vertex<T> Start { get; protected set; }
        public Vertex<T> End { get; protected set; }
        public ConnectionArgs<T> ConnectionInfo { get; protected set; }
        public Edge(){}
        public Edge(Vertex<T> start, Vertex<T> end,ConnectionArgs<T> info)
        {
            Start = start;
            End = end;
            ConnectionInfo = info;
        }

        public int CompareTo(Edge<T> other)
        {
            var x = Start.CompareTo(other.Start);
            var y = End.CompareTo(other.End);
            var z = ConnectionInfo.CompareTo(other.ConnectionInfo);
            if (x != 0)
                return x;
            if (y != 0)
                return y;
            if (z != 0)
                return z;
            return 0;
        }
    }
}
