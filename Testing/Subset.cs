using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Testing
{
    public abstract class Subset:IComparable<Subset>,IEquatable<Subset>
    {
        public abstract Subset Add(Subset subset);
        public abstract Subset Subtract(Subset subset);
        public abstract Subset Multiply(Subset subset);
        public abstract Subset Divide(Subset subset);
        public abstract Subset Less(Subset subset);
        public abstract Subset More(Subset subset);
        public abstract Subset NotEquals(Subset subset);
        public abstract Subset EqualSubset(Subset subset);
        public bool Empty { get; protected set; }
        public abstract int CompareTo(Subset other);

        public abstract bool Equals(Subset subset);
    }
}
