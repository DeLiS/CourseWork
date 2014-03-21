using System;

namespace Graphs.lib.DataStructure
{
    public interface IEdge<T>
        where T:IComparable<T>
    {
        Vertex<T> Start { get;}
        Vertex<T> End { get; }
    }
}
