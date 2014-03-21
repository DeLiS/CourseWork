using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Testing
{
    public class SinglePoint:Subset
    {
        public double Point { get; private set; }

        public SinglePoint()
        {
            Empty = true;
        }
        
        public SinglePoint(double point)
        {
            Point = point;
            Empty = false;
        }
        
        public SinglePoint(SinglePoint point)
        {
            Point = point.Point;
            Empty = point.Empty;
        }

        public override Subset Add(Subset subset)
        {
            if(subset is SinglePoint)
            {
                SinglePoint singlePoint = subset as SinglePoint;
                if (!(singlePoint.Empty || this.Empty))
                {
                    SinglePoint result = new SinglePoint(this.Point + singlePoint.Point);
                    return result;
                }
            }
            throw new NotImplementedException();
        }

        public override Subset Subtract(Subset subset)
        {
            if (subset is SinglePoint)
            {
                SinglePoint singlePoint = subset as SinglePoint;
                if (!(singlePoint.Empty || this.Empty))
                {
                    SinglePoint result = new SinglePoint(this.Point - singlePoint.Point);
                    return result;
                }
            }
            throw new NotImplementedException();
        }

        public override Subset Multiply(Subset subset)
        {
            if (subset is SinglePoint)
            {
                SinglePoint singlePoint = subset as SinglePoint;
                if (!(singlePoint.Empty || this.Empty))
                {
                    SinglePoint result = new SinglePoint(this.Point * singlePoint.Point);
                    return result;
                }
            }
            throw new NotImplementedException();
        }

        public override Subset Divide(Subset subset)
        {
            if (subset is SinglePoint)
            {
                SinglePoint singlePoint = subset as SinglePoint;
                if (!(singlePoint.Empty || this.Empty))
                {
                    SinglePoint result = new SinglePoint(this.Point / singlePoint.Point);
                    return result;
                }
            }
            throw new NotImplementedException();
        }

        public override Subset Less(Subset subset)
        {
            if (subset is SinglePoint)
            {
                SinglePoint singlePoint = subset as SinglePoint;
                if (!(singlePoint.Empty || this.Empty))
                {
                    SinglePoint result;
                    if(this.Point < singlePoint.Point)
                    {
                        result = new SinglePoint(this);
                    }
                    else
                    {
                        result = new SinglePoint();
                    }
                    return result;
                }
            }
            throw new NotImplementedException();
        }

        public override Subset More(Subset subset)
        {
            if (subset is SinglePoint)
            {
                SinglePoint singlePoint = subset as SinglePoint;
                if (!(singlePoint.Empty || this.Empty))
                {
                    SinglePoint result;
                    if (this.Point > singlePoint.Point)
                    {
                        result = new SinglePoint(this);
                    }
                    else
                    {
                        result = new SinglePoint();
                    }
                    return result;
                }
            }
            throw new NotImplementedException();
        }

        public override Subset NotEquals(Subset subset)
        {
            if (subset is SinglePoint)
            {
                SinglePoint singlePoint = subset as SinglePoint;
                if (!(singlePoint.Empty || this.Empty))
                {
                    SinglePoint result;
                    if (this.Point != singlePoint.Point)
                    {
                        result = new SinglePoint(this);
                    }
                    else
                    {
                        result = new SinglePoint();
                    }
                    return result;
                }
            }
            throw new NotImplementedException();
        }

        public override Subset EqualSubset(Subset subset)
        {
            if (subset is SinglePoint)
            {
                SinglePoint singlePoint = subset as SinglePoint;
                if (!(singlePoint.Empty || this.Empty))
                {
                    SinglePoint result;
                    if (this.Point == singlePoint.Point)
                    {
                        result = new SinglePoint(this);
                    }
                    else
                    {
                        result = new SinglePoint();
                    }
                    return result;
                }
                else
                {
                    if (this.Empty == singlePoint.Empty)
                        return new SinglePoint();
                }
            }
            throw new NotImplementedException();
        }

        public override int CompareTo(Subset other)
        {
            if(other is SinglePoint)
            {
                SinglePoint singlePoint = other as SinglePoint;
                if (!(singlePoint.Empty || this.Empty))
                {
                    return this.Point.CompareTo(singlePoint.Point);
                }
            }
            throw new NotImplementedException();
        }

        public override bool Equals(Subset subset)
        {
            if (subset is SinglePoint)
            {
                SinglePoint singlePoint = subset as SinglePoint;
                if (!(singlePoint.Empty || this.Empty))
                {
                    return this.Point == singlePoint.Point;
                }
                else
                {
                    return this.Empty == singlePoint.Empty;
                }
            }
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            return new SinglePoint(this);
        }
    }
}
