using System;

namespace Graphs.lib.DataStructure
{
    public class WeightedGraphFacotry<T>:GraphPartsFactory<T>
        where T:IComparable<T>
    {
        public override Edge<T> GetEdge(Vertex<T> start, Vertex<T> end)
        {
            ConnectionArgs<T> args;
            if (start.IsConnectedWith(end, out args))
            {
                WeightedConnectionArgs<T> wargs = args as WeightedConnectionArgs<T>;
                if(wargs!=null)
                {
                   var edge = 
                        new Edge<T>(start,end,wargs);
                    return edge;
                }
                throw new Exception("Wrong args type");
            }
            return null;
            
        }
    }
}
