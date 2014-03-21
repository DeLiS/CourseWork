using System;

namespace Graphs.lib.DataStructure
{
    interface IWeightedEdge<T>:IEdge<T>
        where T:IComparable<T>
    {
        double Weight { get; }
    }
}
