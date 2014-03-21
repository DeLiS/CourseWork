using System;
using System.Collections.Generic;
using FiniteStateMachines.Interfaces;

namespace FiniteStateMachines.Utility
{
    /// <remarks>
    /// Символ, соберающий в себе множество символов.
    /// </remarks>
    public class MultiSymbol<T>:ISymbol<T>
        where T:IComparable<T>,IEquatable<T>
    {
        ///<summary>
        /// Множество символов.
        ///</summary>
        public SortedSet<ISymbol<T>> SymbolSet { get; private set; }
        
        /// <summary>
        /// Конструктор без параметров.
        /// Выставляет значение поля Type равным SymbolType.Complex.
        /// </summary>
        public MultiSymbol()
        {
            SymbolSet = new SortedSet<ISymbol<T>>();
            this.Type = SymbolType.Complex;
        }

        /// <summary>
        /// Добавление символа во множество.
        /// </summary>
        /// <param name="symbol">Символ, который нужно добавить.</param>
        public void AddSymbol(ISymbol<T> symbol)
        {
            if (!SymbolSet.Contains(symbol))
                SymbolSet.Add(symbol);
        }

        /// <summary>
        /// Метод, удаляющий символ из множества символов.
        /// </summary>
        /// <param name="symbol">Символ, который нужно удалить.</param>
        public void RemoveSymbol(ISymbol<T> symbol)
        {
            SymbolSet.Remove(symbol);
        }

        #region Implementation of IComparable<in ISymbol<T>>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(ISymbol<T> other)
        {
            var symbol = other as MultiSymbol<T>;
            if(symbol!=null)
            {
                var enumerator1 = this.SymbolSet.GetEnumerator();
                var enumerator2 = symbol.SymbolSet.GetEnumerator();
                while(true)
                {
                    bool canMove1 = enumerator1.MoveNext();
                    bool canMove2 = enumerator2.MoveNext();
                    if(canMove1&&!canMove2)
                        return 1;
                    if(!canMove1&&canMove2)
                        return -1;
                    if(!canMove1)// && ! move2 - expression is always true
                        return 0;
                    if (enumerator1.Current != null)
                    {
                        int curCmp = enumerator1.Current.CompareTo(enumerator2.Current);
                        if(curCmp!=0)
                            return curCmp;
                    }
                    else
                    {
                        throw new ApplicationException("Comparing error");
                    }
                }
            }
            return -1;//throw?
        }

        #endregion

        #region Implementation of IEquatable<ISymbol<T>>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ISymbol<T> other)
        {
            var symbol = other as MultiSymbol<T>;
            if (symbol != null)
            {
                return this.SymbolSet.Count == symbol.SymbolSet.Count && this.SymbolSet.IsSubsetOf(symbol.SymbolSet);
                //return this.SymbolSet.Equals(symbol.SymbolSet);
            }
            return false; //throw?
        }

        #endregion

        #region Implementation of ISymbol<T>
        /// <summary>
        /// Тип символа
        /// </summary>
        public SymbolType Type { get; private set; }

        #endregion
    }
}
