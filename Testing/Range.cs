using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Testing
{
    public class Range:Subset
    {
        public SinglePoint Start { get; private set; }
        public SinglePoint End { get; private set; }

        public Range(SinglePoint start, SinglePoint end)
        {
            Start = new SinglePoint(start);
            End = new SinglePoint(end);
        }
        public Range(Range range)
        {
            Start = new SinglePoint(range.Start);
            End = new SinglePoint(range.End);
        }

        public override Subset Add(Subset subset)
        {
            throw new NotImplementedException();
        }

        public override Subset Subtract(Subset subset)
        {
            throw new NotImplementedException();
        }

        public override Subset Multiply(Subset subset)
        {
            throw new NotImplementedException();
        }

        public override Subset Divide(Subset subset)
        {
            throw new NotImplementedException();
        }

        public override Subset Less(Subset subset)
        {
            throw new NotImplementedException();
        }

        public override Subset More(Subset subset)
        {
            throw new NotImplementedException();
        }

        public override Subset NotEquals(Subset subset)
        {
            throw new NotImplementedException();
        }

        public override Subset EqualSubset(Subset subset)
        {
            throw new NotImplementedException();
        }

        public override int CompareTo(Subset other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(Subset subset)
        {
            throw new NotImplementedException();
        }
    }
}
