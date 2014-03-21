using System;
using FiniteStateMachines.Interfaces;
using System.Diagnostics;
namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Сигнатура перехода конечного автомата.
    ///</summary>
    ///<typeparam name="TIn">Тип входного символа.</typeparam>
    ///<typeparam name="TOut">Тип выходного символа.</typeparam>
    ///<typeparam name="TId">Тип идентификаторов состояний.</typeparam>
    public class IdStepSignature<TIn, TOut, TId> : IComparable<IdStepSignature<TIn, TOut, TId>>, IEquatable<IdStepSignature<TIn, TOut, TId>>, ICloneable
        where TIn : IEquatable<TIn>, IComparable<TIn>
        where TOut : IEquatable<TOut>, IComparable<TOut>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="startState">Начальное состояние.</param>
        ///<param name="input">Входной символ.</param>
        ///<param name="output">Выходной символ.</param>
        ///<param name="endState">Конечное состояние.</param>
        public IdStepSignature(TId startState, ISymbol<TIn> input, ISymbol<TOut> output, TId endState)
        {
            StartState = startState;
            Input = input;
            Output = output;
            EndState = endState;
        }

        ///<summary>
        /// Конструктор, создающий сигнатуру перехода с номерами состояний вместо ссылок.
        ///</summary>
        ///<param name="refStepSignature">Ссылочная сигнатура перехода.</param>
        public IdStepSignature(RefStepSignature<TIn, TOut, TId> refStepSignature)
        {
            StartState = refStepSignature.StartState.Id;
            EndState = refStepSignature.TargetState.Id;
            Input = refStepSignature.InputSymbol;
            Output = refStepSignature.OutputSymbol;
        }

        protected IdStepSignature()
        {
            
        }

        ///<summary>
        /// Исходное состояние перехода.
        ///</summary>
        public TId StartState { get; set; }

        ///<summary>
        /// Входной символ перехода.
        ///</summary>
        public ISymbol<TIn> Input { get; set; }

        ///<summary>
        /// Выходной символ перехода.
        ///</summary>
        public ISymbol<TOut> Output { get; set; }

        ///<summary>
        /// Конечное состояние перехода.
        ///</summary>
        public TId EndState { get; set; }

        #region Implementation of IComparable<in IdStepSignature<TIn,TOut>>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual int CompareTo(IdStepSignature<TIn, TOut, TId> other)
        {
            int cmp = this.StartState.CompareTo(other.StartState);
            if (cmp != 0)
                return cmp;
            cmp = this.Input.CompareTo(other.Input);
            if (cmp != 0)
                return cmp;
            cmp = this.Output.CompareTo(other.Output);
            if (cmp != 0)
                return cmp;
            cmp = this.EndState.CompareTo(other.EndState);
            return cmp;

        }

        #endregion

        #region Implementation of IEquatable<IdStepSignature<TIn,TOut>>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual bool Equals(IdStepSignature<TIn, TOut,TId> other)
        {
            return CompareTo(other) == 0;
        }

        #endregion

        #region Implementation of ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public virtual object Clone()
        {
            return new IdStepSignature<TIn, TOut, TId>(StartState, Input, Output, EndState);
        }

        #endregion
    }
}