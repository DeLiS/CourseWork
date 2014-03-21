using System;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;
using System.Diagnostics;
namespace FiniteStateMachines.Core
{
    /// <summary>
    /// Смежное состояние
    /// </summary>
    /// <typeparam name="TIn">Входящий символ</typeparam>
    /// <typeparam name="TOut">Результирующий символ</typeparam>
    public class AdjacentState<TIn, TOut, TId> : IComparable<AdjacentState<TIn, TOut, TId>>, IEquatable<AdjacentState<TIn, TOut, TId>>
        where TIn : IEquatable<TIn>,IComparable<TIn>
        where TOut : IEquatable<TOut>,IComparable<TOut>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        
        ///<summary>
        /// Результирующее состояние
        ///</summary>
        public IState<TIn, TOut, TId> TargetState { get; protected set; }

        ///<summary>
        /// Входной символ
        ///</summary>
        public ISymbol<TIn> Input { get; protected set; }

        ///<summary>
        /// Выходной символ
        ///</summary>
        public ISymbol<TOut> Output { get; protected set; }

        ///<summary>
        /// Конструктор
        ///</summary>
        ///<param name="input">Входной символ</param>
        ///<param name="output">Выходной символ</param>
        ///<param name="targetState">Результирующее состояние</param>
        public AdjacentState(RefStepSignature<TIn, TOut, TId> signature)
        {
            TargetState = signature.TargetState;
            Input = signature.InputSymbol;
            Output = signature.OutputSymbol;
        }

        /// <summary>
        /// Для наследования.
        /// </summary>
        protected AdjacentState(){}

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual int CompareTo(AdjacentState<TIn, TOut, TId> other)
        {
            int a = this.Input.CompareTo(other.Input);
            if (a != 0)
                return a;
            a = this.Output.CompareTo(other.Output);
            if (a != 0)
                return a;
            a = this.TargetState.CompareTo(other.TargetState);
            return a;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual bool Equals(AdjacentState<TIn, TOut, TId> other)
        {
            return this.CompareTo(other) == 0;
        }
    }
}
