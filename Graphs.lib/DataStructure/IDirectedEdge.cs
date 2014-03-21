using System;

namespace Graphs.lib.DataStructure
{
    public interface IDirectedEdge<T>:IEdge<T>
        where T:IComparable<T>
    {
        bool Directed { get; }
    }
}
