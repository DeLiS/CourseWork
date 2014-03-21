using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Пара.
    ///</summary>
    ///<typeparam name="T1">Тип первого элемента.</typeparam>
    ///<typeparam name="T2">Тип второго элемента.</typeparam>
    //[DebuggerDisplay("({Key},{Value})")]
    public class Pair<T1,T2>:IComparable<Pair<T1, T2>>,IEquatable<Pair<T1, T2>>
        where T1:IComparable<T1>,IEquatable<T1>
        where T2:IComparable<T2>,IEquatable<T2>
    {
        ///<summary>
        /// Первый элемент.
        ///</summary>
        public T1 Key { get; private set; }
        ///<summary>
        /// Второй элемент.
        ///</summary>
        public T2 Value { get; private set; }
        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="key">Первый элемент.</param>
        ///<param name="value">Второй элемент.</param>
        public Pair(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }
        #region Implementation of IComparable<in Pair<T,U>>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(Pair<T1, T2> other)
        {
            int keycmp = this.Key.CompareTo(other.Key);
            if(keycmp!=0)
                return keycmp;
            return this.Value.CompareTo(other.Value);
        }

        #endregion

        #region Implementation of IEquatable<Pair<T,U>>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Pair<T1, T2> other)
        {
            return CompareTo(other) == 0;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("({0},{1})", Key, Value);
        }
    }
}
