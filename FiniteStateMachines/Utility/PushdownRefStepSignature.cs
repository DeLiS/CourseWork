using System;
using FiniteStateMachines.Interfaces;

namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Сигнатура перехода в автомате с магазинной памятью с состояниями в виде ссылок.
    ///</summary>
    ///<typeparam name="TIn">Тип входных символов.</typeparam>
    ///<typeparam name="TOut">Тип результирующих символов.</typeparam>
    ///<typeparam name="TStack">Тип символов магазинной памяти.</typeparam>
    ///<typeparam name="TId">Тип идентификаторов состояний автомата.</typeparam>
    public class PushdownRefStepSignature<TIn, TOut, TStack, TId> : RefStepSignature<TIn, TOut, TId>, IComparable<PushdownRefStepSignature<TIn, TOut, TStack, TId>>
        where TOut : IEquatable<TOut>, IComparable<TOut>
        where TIn : IComparable<TIn>, IEquatable<TIn>
        where TStack:IComparable<TStack>,IEquatable<TStack>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        ///<summary>
        /// Действие, производимое с памятью при переходе.
        ///</summary>
        public StackActions StackAction { get; private set; }
        ///<summary>
        /// Символ, который необходимо добавить в стек, если такое действие предполагается.
        ///</summary>
        public ISymbol<TStack> ToPush { get; private set; }
        ///<summary>
        /// Необходимо ли проверять вершину стека на соотвествие символу StackTop.
        ///</summary>
        public bool CheckStack { get; private set; }
        ///<summary>
        /// Символ, с которым необходимо сравнить вершину стека, если переменная CheckStack истинна.
        ///</summary>
        public ISymbol<TStack> StackTop { get; private set; }
        ///<summary>
        /// Конструктор в случае, если при переходе необходимо проверять значение на вершине стека.
        ///</summary>
        ///<param name="startState">Исходное состояние.</param>
        ///<param name="inputSymbol">Входной символ.</param>
        ///<param name="stackTop">Вершина стека.</param>
        ///<param name="outputSymbol">Выходной символ.</param>
        ///<param name="targetState">Конечное состояние.</param>
        ///<param name="stackAction">Действие с памятью.</param>
        ///<param name="toPush">Символ, который необходимо добавить в стек (если необходимо).</param>
        public PushdownRefStepSignature(IState<TIn, TOut, TId> startState, 
                                        ISymbol<TIn> inputSymbol, 
                                        ISymbol<TOut> outputSymbol,
                                        IState<TIn, TOut, TId> targetState, 
                                        StackActions stackAction,
                                        ISymbol<TStack> toPush,
                                        ISymbol<TStack> stackTop):base(startState,inputSymbol,outputSymbol,targetState)
        {
            StackAction = stackAction;
            ToPush = toPush;
            StackTop = stackTop;
            CheckStack = true;
        }
        ///<summary>
        /// Конструктор в случае, если при переходе не нужно проверять значение на вершине стека.
        ///</summary>
        ///<param name="startState">Исходное состояние.</param>
        ///<param name="inputSymbol">Входной символ.</param>
        ///<param name="outputSymbol">Выходной символ.</param>
        ///<param name="targetState">Конечное состояние.</param>
        ///<param name="stackAction">Действие с памятью.</param>
        ///<param name="toPush">Символ, который необходимо добавить в стек (если необходимо).</param>
        public PushdownRefStepSignature(IState<TIn, TOut, TId> startState,
                                        ISymbol<TIn> inputSymbol,
                                        ISymbol<TOut> outputSymbol,
                                        IState<TIn, TOut, TId> targetState,
                                        StackActions stackAction,
                                        ISymbol<TStack> toPush
                                        )
            : base(startState, inputSymbol, outputSymbol, targetState)
        {
            StackAction = stackAction;
            ToPush = toPush;
            CheckStack = false;
        }

        #region Implementation of IComparable<in PushdownRefStepSignature<TIn,TOut,TStack>>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(PushdownRefStepSignature<TIn, TOut, TStack, TId> other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
