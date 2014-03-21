using System;
using FiniteStateMachines.Interfaces;
namespace FiniteStateMachines.Utility
{
    /// <remarks>
    /// Класс, инкапсулирующий в себе входящие параметры перехода .
    /// </remarks>
    /// <typeparam name="TIn">Тип входящего символа.</typeparam>
    public class StepQuery<TIn>
        where TIn:IComparable<TIn>,IEquatable<TIn>
    {
        ///<summary>
        /// Входящий символ.
        ///</summary>
        public ISymbol<TIn> Input { get; private set; }
        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="input">Входящий символ.</param>
        public StepQuery(ISymbol<TIn> input)
        {
            Input = input;
        }

        /// <summary>
        /// Конструктор для наследования.
        /// </summary>
        protected StepQuery(){}
    }
}
