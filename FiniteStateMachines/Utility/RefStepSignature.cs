using System;
using FiniteStateMachines.Interfaces;

namespace FiniteStateMachines.Utility
{
    /// <remarks>
    /// Ссылочная сигнатура перехода
    /// </remarks>
    /// <typeparam name="TIn">Тип входящего символа</typeparam>
    /// <typeparam name="TOut">Тип результирующего символа</typeparam>
    public class RefStepSignature<TIn, TOut, TId> : IComparable<RefStepSignature<TIn, TOut, TId>>, IEquatable<RefStepSignature<TIn, TOut, TId>>
        where TIn : IEquatable<TIn>, IComparable<TIn>
        where TOut : IEquatable<TOut>, IComparable<TOut>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        ///<summary>
        /// Исходное состояние перехода.
        ///</summary>
        public IState<TIn, TOut, TId> StartState { get; private set; }

        ///<summary>
        /// Входной символ перехода.
        ///</summary>
        public ISymbol<TIn> InputSymbol { get; private set; }

        ///<summary>
        /// Выходной символ перехода.
        ///</summary>
        public ISymbol<TOut> OutputSymbol { get; private set; }

        ///<summary>
        /// Конечное состояние перехода.
        ///</summary>
        public IState<TIn, TOut, TId> TargetState { get; private set; }

        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="startState">Начальное состояние.</param>
        ///<param name="inputSymbol">Входной символ.</param>
        ///<param name="outputSymbol">Выходной символ.</param>
        ///<param name="targetState">Конечное состояние.</param>
        public RefStepSignature(IState<TIn, TOut, TId> startState, ISymbol<TIn> inputSymbol, ISymbol<TOut> outputSymbol, IState<TIn, TOut, TId> targetState)
        {
            StartState = startState;
            TargetState = targetState;
            InputSymbol = inputSymbol;
            OutputSymbol = outputSymbol;
        }

        protected RefStepSignature()
        {}

        ///<summary>
        /// Конвертирует объект к типу IdStepSignature.
        ///</summary>
        ///<returns>Соответствующий объект типа IdStepSignature</returns>
        public IdStepSignature<TIn, TOut, TId> ToIdStepSignature()
        {
            return new IdStepSignature<TIn, TOut, TId>(StartState.Id, InputSymbol, OutputSymbol, TargetState.Id);
        }

        #region Implementation of IComparable<in RefStepSignature<TIn,TOut>>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(RefStepSignature<TIn, TOut, TId> other)
        {
            int s = this.StartState.CompareTo(other.StartState);
            if(s==0)
            {
                int i = this.InputSymbol.CompareTo(other.InputSymbol);
                if(i==0)
                {
                    int o = this.OutputSymbol.CompareTo(other.OutputSymbol);
                    if(o==0)
                    {
                        int e = this.TargetState.CompareTo(other.TargetState);
                        return e;
                    }
                    return o;

                }
                return i;
            }
            return s;
        }

        #endregion

        #region Implementation of IEquatable<RefStepSignature<TIn,TOut>>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(RefStepSignature<TIn, TOut, TId> other)
        {
            return this.StartState.Equals(other.StartState) &&
                   this.TargetState.Equals(other.TargetState) &&
                   this.InputSymbol.Equals(other.InputSymbol) &&
                   this.OutputSymbol.Equals(other.OutputSymbol);
        }

        #endregion
    }

}
