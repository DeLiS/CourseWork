using System;

namespace Graphs.lib.DataStructure
{
    public class ConnectionArgs<T>:IComparable<ConnectionArgs<T>>
        where T : IComparable<T>
    {
        public bool Directed { get; protected set; }
        public ConnectionArgs(bool directed)
        {
            Directed = directed;
        }
        public static ConnectionArgs<T> DirectedEdge
        {
            get { return new ConnectionArgs<T>(true);}
        }
        public static ConnectionArgs<T> UndirectedEdge
        {
            get {return new ConnectionArgs<T>(false);}
        }

        public virtual int CompareTo(ConnectionArgs<T> other)
        {
            return this.Directed.CompareTo(other.Directed);
        }
    }
}
