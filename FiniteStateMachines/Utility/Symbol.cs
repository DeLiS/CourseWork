using System;
using FiniteStateMachines.Interfaces;

namespace FiniteStateMachines.Utility
{
    ///<remarks>
    /// Символ.
    ///</remarks>
    ///<typeparam name="T">Носитель информации.</typeparam>
    public class Symbol<T>:ISymbol<T>,IEquatable<Symbol<T>>,IComparable<Symbol<T>>
        where T:IComparable<T>,IEquatable<T>
    {
        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual int CompareTo(ISymbol<T> other)
        {
            var otherSymbol = other as Symbol<T>;
            if (otherSymbol != null )
            {

                if (Type == SymbolType.Empty && otherSymbol.Type == SymbolType.Empty)
                    return 0;
                if (Type == SymbolType.Empty)
                    return -1;
                if (otherSymbol.Type == SymbolType.Empty)
                    return 1;
                return Value.CompareTo(otherSymbol.Value);
            }
            return -1; // throw new exception?
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual bool Equals(ISymbol<T> other)
        {
            return CompareTo(other) == 0;
        }

        ///<summary>
        /// Значение символа.
        ///</summary>
        public T Value{get; private set;}

        ///<summary>
        /// Тип символа.
        ///</summary>
        public SymbolType Type { get; private set; }

        ///<summary>
        /// Конструктор по умолчанию, создает символ типа "пустой".
        ///</summary>
        public Symbol()
        {
            Type = SymbolType.Empty;
        }

        ///<summary>
        /// Конструктор с параметрами.
        ///</summary>
        ///<param name="value">Значение символа.</param>
        ///<param name="type">Тип символа.</param>
        public Symbol(T value, SymbolType type)
        {
            Value = value;
            Type = type;
        }

        public bool Equals(Symbol<T> other)
        {
            return CompareTo(other) == 0;
        }

        public int CompareTo(Symbol<T> other)
        {
            var cmp = Type.CompareTo(other.Type);
            if (cmp != 0)
                return cmp;
            if (Type == SymbolType.Empty)
                return 0;
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        private static readonly Symbol<T> _empty = new Symbol<T>();
        ///<summary>
        /// Пустой символ типа Т.
        ///</summary>
        public static Symbol<T> Empty
        {
            get { return _empty; }
        }
    }
    
}
