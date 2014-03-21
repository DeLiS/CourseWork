using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphs.lib.Heap
{
    interface IHeap<Key,Value>
        where Value:IComparable<Value>
        where Key:IComparable<Key>
    {
        Value Top { get; }
        void Pop();
        void Push(Value value,Key key);
        void DecreaseKey(Value value, Key key);
        void Delete(Value value);
        int Count { get; }
    }
}
