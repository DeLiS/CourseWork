using System;

namespace Graphs.lib.DataStructure
{
    public class WeightedConnectionArgs<T>:ConnectionArgs<T>
        where T:IComparable<T>
    {
        public double Weight { get; protected set; }
        public WeightedConnectionArgs(bool directed, double weight):base(directed)
        {
            Weight = weight;
        }
        public override int CompareTo(ConnectionArgs<T> other)
        {
            var o = other as WeightedConnectionArgs<T>;
            if(o!=null)
            {
                var x = base.CompareTo(other);
                return x == 0 ? this.Weight.CompareTo(o.Weight) : x;
            }
            return base.CompareTo(other);
        }
    }
}
