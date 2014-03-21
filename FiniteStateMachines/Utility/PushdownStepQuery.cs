using System;
using FiniteStateMachines.Interfaces;
namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Запрос к путешественнику с памятью.
    ///</summary>
    ///<typeparam name="TIn">Тип входного символа.</typeparam>
    ///<typeparam name="TStack">Тип символа магазинной памяти.</typeparam>
    public class PushdownStepQuery<TIn,TStack>:StepQuery<TIn>
        where TIn:IComparable<TIn>,IEquatable<TIn>
        where TStack:IComparable<TStack>,IEquatable<TStack>
    {
        /// <summary>
        /// Значение вершины памяти.
        /// </summary>
        public ISymbol<TStack> StackTop { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="input">Входной символ.</param>
        /// <param name="stackTop">Вершина памяти.</param>
        public PushdownStepQuery(ISymbol<TIn> input, ISymbol<TStack> stackTop):base(input)
        {
            StackTop = stackTop;
        }
    }
}
