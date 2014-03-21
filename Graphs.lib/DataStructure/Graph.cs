using System;
using System.Collections;
using System.Collections.Generic;

namespace Graphs.lib.DataStructure
{
    public class Graph<T>:IEnumerable<T>
        where T:IComparable<T>
    {
        protected GraphPartsFactory<T> _factory = new GraphPartsFactory<T>();
        protected List<Vertex<T>> Vertexes = new List<Vertex<T>>();
        protected int MaxIndex = -1;
        public Graph()
        {
            
        }
        public Graph(GraphPartsFactory<T> factory)
        {
            this._factory = factory;
        }
        protected Edge<T> GetEdge(Vertex<T> start, Vertex<T> end)
        {
            Edge<T> edge = _factory.GetEdge(start, end);
            return edge;
        }
        public Edge<T> GetEdge(T start, T end)
        {
            Vertex<T> vstart = GetVertex(start);
            Vertex<T> vend = GetVertex(end);
            return GetEdge(vstart, vend);
        }
        public virtual int AddVertex(T value)
        {
            MaxIndex = MaxIndex + 1;
            Vertex<T> vertex = _factory.CreateVertex(MaxIndex, value);//new Vertex<T>(MaxIndex,value);
            Vertexes.Add(vertex);
            return MaxIndex;
        }
        protected virtual bool AreConnected(Vertex<T> start, Vertex<T> end,out ConnectionArgs<T> args)
        {

            return start.IsConnectedWith(end,out args);
        }
        protected virtual void Connect(Vertex<T> start, Vertex<T> end, ConnectionArgs<T> args)
        {
           
            start.ConnectTo(end,args);
            if(!args.Directed)
            {
                end.ConnectTo(start,args);
            }
        }
        public virtual int GetKey(T value)
        {
            foreach (Vertex<T> t in Vertexes)
                if (t.Value.CompareTo(value) == 0)
                    return t.Key;
            return -1;
        }
        public virtual T GetValue(int Key)
        {
            foreach (var vertex in Vertexes)
            {
                if (vertex.Key == Key)
                    return vertex.Value;
            }
            throw new Exception("No element with such key");
        }
        protected Vertex<T> GetVertex(T value)
        {
            foreach (var vertex in Vertexes)
            {
                if (vertex.Value.CompareTo(value) == 0)
                    return vertex;
            }
            throw new Exception("No such vertex");
        }
        public virtual void Connect(T start, T end, ConnectionArgs<T> args)
        {
            var vstart = this.GetVertex(start);
            var vend = this.GetVertex(end);
            this.Connect(vstart,vend,args);
        }
        public virtual void Disconnect(T start, T end)
        {
            ConnectionArgs<T> args;
           if(AreConnected(start,end,out args))
           {
               var vstart = GetVertex(start);
               var vend = GetVertex(end);
               vstart.Disconnect(vend);
               if(!args.Directed)
               {
                   vend.Disconnect(vstart);
               }
           }
        }
        public virtual bool AreConnected(T start, T end,out ConnectionArgs<T> args)
        {
            args = null;
            foreach (Vertex<T> t in Vertexes)
            {
                if(t.Value.CompareTo(start)==0)
                {
                    
                    return t.IsConnectedWith(end,out args);
                }
            }
            return false;
        }
        public virtual bool Contains(T value)
        {
            foreach (var vertex in Vertexes)
            {
                if (vertex.Value.CompareTo(value) == 0)
                    return true;
            }
            return false;
        }
        public virtual AdjacentVertex<T>[] AdjacentVertexes(T value)
        {
            Vertex<T> vertex = GetVertex(value);
            if(vertex!=null)
            {
                return vertex.AdjacentVertexes;
            }
            throw new Exception("No such vertex in graph");
        }
        public IEnumerator<T> GetEnumerator()
        {
            return new GraphEnumerator<T>(Vertexes.ToArray());
        }
        public Graph<T> Transpose()
        {
            Graph<T> graph = new Graph<T>();
            for (int i = 0; i < Vertexes.Count; ++i)
                graph.AddVertex(Vertexes[i].Value);
            for(int i = 0;i<Vertexes.Count;++i)
            {
                var vertexes = Vertexes[i].AdjacentVertexes;
                foreach (var adjacentVertex in vertexes)
                {
                    graph.Connect(adjacentVertex.Vertex.Value,Vertexes[i].Value,adjacentVertex.Args);
                }
            }
            return graph;
        }
        public int VertexesCount { get { return Vertexes.Count; } }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerable<Edge<T>> Edges
        {
            get
            {
                foreach (var vertex in Vertexes)
                {
                    foreach (var vertex1 in this.AdjacentVertexes(vertex.Value))
                    {
                        if(vertex1.Args.Directed==true || vertex1.Args.Directed==false&&vertex.CompareTo(vertex1.Vertex)<0)
                        yield return new Edge<T>(vertex,vertex1.Vertex,vertex1.Args);
                    }
                }
            }
        }
        public IEnumerable<T> Values
        {
            get
            {
                foreach (var vertex in Vertexes)
                {
                    yield return vertex.Value;
                }
            }
        }
    }
}
