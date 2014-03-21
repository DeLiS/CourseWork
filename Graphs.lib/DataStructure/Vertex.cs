using System;
using System.Collections.Generic;

namespace Graphs.lib.DataStructure
{
    public class Vertex<T>:IComparable<T>,IComparable<Vertex<T>>,IEquatable<Vertex<T>>
        where T:IComparable<T>
    {
        public int Key { get; protected set; }
        public T Value { get; protected set; }
        protected readonly List<AdjacentVertex<T>> Adjacent = new List<AdjacentVertex<T>>();


        public Vertex(int key,T value)
        {
            Key = key;
            Value = value;
        }

        public virtual void ConnectTo(Vertex<T> finish,ConnectionArgs<T> args=null)
        {
            AdjacentVertex<T> adjv = new AdjacentVertex<T>(finish,args);
            Adjacent.Add(adjv);
        }

        public virtual void Disconnect(Vertex<T> vertex)
        {
            Adjacent.RemoveAll(x => x.Vertex.CompareTo(vertex) == 0);
        }

        public bool IsConnectedWith(Vertex<T> vertex,out ConnectionArgs<T> args)
        {
            args = null;
            foreach (var adjacentVertex in Adjacent)
            {
                if (adjacentVertex.Vertex.CompareTo(vertex) == 0)
                {
                    args = adjacentVertex.Args;
                    return true;
                }
            }
            return false;
        }

        public bool IsConnectedWith(T value,out ConnectionArgs<T> args)
        {
            args = null;
            foreach (var adjacentVertex in Adjacent)
            {
                if (adjacentVertex.Vertex.CompareTo(value) == 0)
                {
                    args = adjacentVertex.Args;
                    return true;
                }
            }
            return false;
        }
        
        public int CompareTo(T other)
        {
            return this.Value.CompareTo(other);
        }

        public int CompareTo(Vertex<T> other)
        {
            return this.Value.CompareTo(other.Value);
        }

        public bool Equals(Vertex<T> other)
        {
            return this.Key == other.Key;
        }
        public AdjacentVertex<T>[] AdjacentVertexes
        {
            get
            {
                AdjacentVertex<T>[] result = new AdjacentVertex<T>[Adjacent.Count];
                for (int i = 0; i < Adjacent.Count; ++i)
                    result[i] = Adjacent[i];
                return result;
            }
        }
    }
}
