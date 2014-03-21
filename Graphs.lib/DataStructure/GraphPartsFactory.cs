using System;

namespace Graphs.lib.DataStructure
{
    public class GraphPartsFactory<T>
        where T:IComparable<T>
    {
        public virtual Vertex<T> CreateVertex(int key, T value)
        {
            Vertex<T> vertex = new Vertex<T>(key,value);
            return vertex;
        }
        public virtual Edge<T> GetEdge(Vertex<T> start,Vertex<T> end)
        {
            ConnectionArgs<T> args=null;
            if(start.IsConnectedWith(end,out args))
            {
                Edge<T> edge = new Edge<T>(start,end,args);
            }
            return null;
        }
        public virtual AdjacentVertex<T> CreateAdjacentVertex(Vertex<T> vertex, ConnectionArgs<T> args)
        {
            AdjacentVertex<T> adjvert = new AdjacentVertex<T>(vertex,args);
            return adjvert;
        }
    }
}
