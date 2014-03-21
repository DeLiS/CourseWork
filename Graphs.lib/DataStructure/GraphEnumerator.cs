using System;
using System.Collections;
using System.Collections.Generic;

namespace Graphs.lib.DataStructure
{
    class GraphEnumerator<T>:IEnumerator<T>
        where T:IComparable<T>
    {
        private Vertex<T>[] Vertexes;
        private int i = -1;
        public GraphEnumerator(Vertex<T>[] vertarray)
        {
            Vertexes = vertarray;
        }
        public void Dispose()
        {}

        public bool MoveNext()
        {
            i = i + 1;
            return i < Vertexes.Length;
        }

        public void Reset()
        {
            i = -1;
        }

        public T Current
        {
            get { return Vertexes[i].Value; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
