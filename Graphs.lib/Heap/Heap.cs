using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphs.lib.Heap
{
    public class Heap<Key,Value>:IHeap<Key,Value>
        where Key:IComparable<Key>
        where Value:IComparable<Value>
    {
        private readonly List<KeyValuePair<Key,Value>> _list = new List<KeyValuePair<Key, Value>>();
        private int HeapSize { get; set; }
        private readonly IComparer<Key> _comparer;
        private Key _infinity = default(Key);
        public Value Top { get { return _list[0].Value; }  }
        public Heap(IComparer<Key> comparer)
        {
            _comparer = comparer;
            HeapSize = 0;
        }
        public void Pop()
        {
            _list[0] = _list[HeapSize - 1];
            HeapSize--;
            _list.RemoveAt(HeapSize);
            MinHeapify(0);
        }

        public void Push(Value value,Key key)
        {
            if (_infinity.Equals(default(Key)))
                _infinity = key;
            if (_comparer.Compare(_infinity, key) < 0)
                _infinity = key;
            HeapSize++;
            _list.Add(new KeyValuePair<Key, Value>(_infinity,value));
            HeapIncreaseKey(HeapSize-1,key);

        }

        public void DecreaseKey(Value value, Key key)
        {
          for(int i=0;i<_list.Count;++i)
          {
              if(_list[i].Value.CompareTo(value)==0)
              {
                  HeapIncreaseKey(i,key);
                  return;
              }
          }
        }
        private void HeapIncreaseKey(int i,Key key)
        {
            if(_comparer.Compare(key,_list[i].Key)>0)
                throw new Exception("Wrong key");
            _list[i] = new KeyValuePair<Key, Value>(key, _list[i].Value);
            while(i>0&&_comparer.Compare(_list[i].Key,_list[Parent(i)].Key)<0)
            {
                var tmp = _list[i];
                _list[i] = _list[Parent(i)];
                _list[Parent(i)] = tmp;
                i = Parent(i);
            }
        }
        public void Delete(Value value)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return HeapSize; }
        }

        private static int Parent(int i)
        {
            return (i-1)/2;
        }
        private static int Left(int i)
        {
            return 2*i + 1;
        }
        private static int Right(int i)
        {
            return 2*i + 2;
        }
        private void MinHeapify(int i)
        {
            int largest;
            int l = Left(i);
            int r = Right(i);
            if(l<_list.Count&&_comparer.Compare(_list[l].Key,_list[i].Key)<0)
            {
                largest = l;
            }
            else
            {
                largest = i;
            }
            if(r<_list.Count&&_comparer.Compare(_list[r].Key,_list[largest].Key)<0)
            {
                largest = r;
            }
            if(largest!=i)
            {
                var tmp = _list[largest];
                _list[largest] = _list[i];
                _list[i] = tmp;
                MinHeapify(largest);
            }
        }
        private void BuildMinHeap()
        {
            int size = HeapSize;
            for(int i=size/2;i>=0;--i)
            {
                MinHeapify(i);
            }
        }
        private void HeapSort()
        {
            BuildMinHeap();
            for(int i=HeapSize-1;i>0;--i)
            {
                var tmp = _list[0];
                _list[0] = _list[i];
                _list[i] = tmp;
                HeapSize--;
                MinHeapify(0);
            }
        }
        private static void Swap(ref object a,ref object b)
        {
            object tmp = a;
            a = b;
            b = tmp;
        }
        private Value ExtractMax()
        {
            Value max = _list[0].Value;
            _list[0] = _list[HeapSize - 1];
            HeapSize--;
            _list.RemoveAt(HeapSize);
            MinHeapify(0);
            return max;
        }
        public void DecreaseOrAdd(Value value, Key key)
        {
            for (int i = 0; i < _list.Count; ++i)
            {
                if (_list[i].Value.CompareTo(value) == 0)
                {
                    HeapIncreaseKey(i, key);
                    return;
                }
            }
            this.Push(value,key);
        }
    }
}
