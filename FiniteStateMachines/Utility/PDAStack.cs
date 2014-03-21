using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Магазинная память автомата.
    ///</summary>
    ///<typeparam name="T">Тип символов памяти.</typeparam>
    public class PDAStack<T>:IComparable<PDAStack<T>>,IEquatable<PDAStack<T>>
        where T:IComparable<T>,IEquatable<T>
    {
        private readonly List<T> _stack = new List<T>();
        ///<summary>
        /// Количество символов в памяти.
        ///</summary>
        public int Count
        {
            get { return _stack.Count; }
        }
        ///<summary>
        /// Конструктор.
        ///</summary>
        public PDAStack(){}
        ///<summary>
        /// Конструктор, принимающий начальное состояние памяти.
        ///</summary>
        ///<param name="stack"></param>
        public PDAStack(PDAStack<T> stack)
        {
            for(int i=0;i<stack.Count;++i)
                _stack.Add(stack._stack[i]);
        }
        ///<summary>
        /// Возвращает символ на вершине памяти.
        ///</summary>
        ///<returns>Символ на вершине памяти.</returns>
        ///<exception cref="ApplicationException">Возникает, если память пуста.</exception>
        public T Peek()
        {
            if(_stack.Count==0)
                throw new ApplicationException("Empty memory");
            return _stack[_stack.Count - 1];
        }
        ///<summary>
        /// Добавляет символ в память.
        ///</summary>
        ///<param name="element">Символ, который нужно добавить.</param>
        public void Push(T element)
        {
            _stack.Add(element);
        }
        ///<summary>
        /// Удаляет верхний символ из памяти.
        ///</summary>
        ///<returns>Удаленный из памяти символ.</returns>
        public T Pop()
        {
            var element = Peek();
            _stack.RemoveAt(_stack.Count-1);
            return element;
        }
        ///<summary>
        /// Метод, возвращающий нижний элемент памяти.
        ///</summary>
        ///<returns>Нижний элемент памяти.</returns>
        ///<exception cref="ApplicationException">Возникает, если память пуста.</exception>
        public T Botom()
        {
            if(_stack.Count==0)
                throw new ApplicationException("Empty memory");
            return _stack[0];
        }
        #region Implementation of IComparable<in PDAStack<T>>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(PDAStack<T> other)
        {
            int i = 0;
            int len = Math.Min(_stack.Count, other._stack.Count);
            while(i<len)
            {
                var cmp = _stack[i].CompareTo(other._stack[i]);
                if (cmp != 0)
                    return cmp;
                ++i;
            }
            return _stack.Count.CompareTo(other._stack.Count);
        }

        #endregion

        #region Implementation of IEquatable<PDAStack<T>>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(PDAStack<T> other)
        {
            return CompareTo(other) == 0;
        }

        #endregion
    }
}
