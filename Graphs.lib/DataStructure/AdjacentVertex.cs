using System;

namespace Graphs.lib.DataStructure
{
    public class AdjacentVertex<T> 
        where T : IComparable<T>
    {
        public Vertex<T> Vertex { get; protected set; }
        public ConnectionArgs<T> Args { get; protected set; }
        public AdjacentVertex(Vertex<T> vertex,ConnectionArgs<T> args = null)
        {
            Vertex = vertex;
            Args = args;
        }
    }
}
