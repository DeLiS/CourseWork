using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Testing
{
    public class Variable
    {
        public string Name { get; private set; }
        private SortedSet<Subset> _subsets = new SortedSet<Subset>();

        public IEnumerable<Subset> Subsets
        {
            get { return _subsets; }
        }
        public Variable(string name)
        {
            Name = name;
        }

        public Variable(string name, SortedSet<Subset> subsets)
        {
            Name = name;
            _subsets = subsets;
        }

        public Variable(){}

        public SortedSet<Subset> Add(Variable variable)
        {
            SortedSet<Subset> result = new SortedSet<Subset>();
            foreach (var subset in Subsets)
            {
                foreach (var otherSubset in variable.Subsets)
                {
                    result.Add(subset.Add(otherSubset));
                }
            }
            return result;
        }
        public SortedSet<Subset> Subtract(Variable variable)
        {
            SortedSet<Subset> result = new SortedSet<Subset>();
            foreach (var subset in Subsets)
            {
                foreach (var otherSubset in variable.Subsets)
                {
                    result.Add(subset.Subtract(otherSubset));
                }
            }
            return result;
        }
        public SortedSet<Subset> Multiply(Variable variable)
        {
            SortedSet<Subset> result = new SortedSet<Subset>();
            foreach (var subset in Subsets)
            {
                foreach (var otherSubset in variable.Subsets)
                {
                    result.Add(subset.Multiply(otherSubset));
                }
            }
            return result;
        }
        public SortedSet<Subset> Divide(Variable variable)
        {
            SortedSet<Subset> result = new SortedSet<Subset>();
            foreach (var subset in Subsets)
            {
                foreach (var otherSubset in variable.Subsets)
                {
                    result.Add(subset.Divide(otherSubset));
                }
            }
            return result;
        }
        public SortedSet<Subset> Less(Variable variable)
        {
            SortedSet<Subset> result = new SortedSet<Subset>();
            foreach (var subset in Subsets)
            {
                Subset tmp = subset.Clone() as Subset;
                foreach (var otherSubset in variable.Subsets)
                {
                    tmp = tmp.Less(otherSubset);
                }
                if(!tmp.Empty)
                {
                    result.Add(tmp);
                }
            }
            return result;
        }
        public SortedSet<Subset> More(Variable variable);
        public SortedSet<Subset> NotEquals(Variable variable);
        public SortedSet<Subset> EqualSubset(Variable variable);
    }
}
