using System;
using FiniteStateMachines.Interfaces;

namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Сигнатура перехода для автомата с магазинной памятью.
    ///</summary>
    ///<typeparam name="TIn">Тип входящих символов.</typeparam>
    ///<typeparam name="TOut">Тип результирующих символов.</typeparam>
    ///<typeparam name="TStack">Тип символов магазинной памяти.</typeparam>
    ///<typeparam name="TId">Тип уникальных идентификаторов состояний автомата.</typeparam>
    public class IdPushDownStepSignature<TIn, TOut, TStack, TId> :
        IdStepSignature<TIn, TOut, TId>,
        IComparable<IdPushDownStepSignature<TIn, TOut, TStack, TId>>,
        IEquatable<IdPushDownStepSignature<TIn, TOut, TStack, TId>>
        where TIn : IComparable<TIn>, IEquatable<TIn>
        where TOut : IComparable<TOut>, IEquatable<TOut>
        where TStack : IComparable<TStack>, IEquatable<TStack>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        ///<summary>
        /// Действие, производимое с памятью при переходе.
        ///</summary>
        public StackActions StackAction { get; private set; }
        ///<summary>
        /// Символ, с которым необходимо сравнить вершину стека, если переменная CheckStack истинна.
        ///</summary>
        public ISymbol<TStack> StackTop { get; private set; }
        ///<summary>
        /// Символ, который необходимо добавить в стек, если такое действие предполагается.
        ///</summary>
        public ISymbol<TStack> ToPush { get; private set; }
        ///<summary>
        /// Необходимо ли проверять вершину стека на соотвествие символу StackTop.
        ///</summary>
        public bool CheckStack { get; private set; }
        ///<summary>
        /// Конструктор в случае, если при переходе необходимо проверять значение на вершине стека.
        ///</summary>
        ///<param name="start">Исходное состояние.</param>
        ///<param name="input">Входной символ.</param>
        ///<param name="stackTop">Вершина стека.</param>
        ///<param name="output">Выходной символ.</param>
        ///<param name="end">Конечное состояние.</param>
        ///<param name="stackAction">Действие с памятью.</param>
        ///<param name="toPush">Символ, который необходимо добавить в стек (если необходимо).</param>
        public IdPushDownStepSignature(TId start, ISymbol<TIn> input, ISymbol<TStack> stackTop, ISymbol<TOut> output, TId end, StackActions stackAction, ISymbol<TStack> toPush)
            : base(start, input, output, end)
        {
            StackAction = stackAction;
            StackTop = stackTop;
            ToPush = toPush;
            CheckStack = true;
        }
        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="start">Исходное состояние.</param>
        ///<param name="input">Входной символ.</param>
        ///<param name="stackTop">Вершина стека.</param>
        ///<param name="output">Выходной символ.</param>
        ///<param name="end">Конечное состояние.</param>
        ///<param name="stackAction">Действие с памятью.</param>
        ///<param name="toPush">Символ, который необходимо добавить в стек (если необходимо).</param>
        ///<param name="check">Необходимо ли проверять значение на вершине памяти.</param>
        public IdPushDownStepSignature(TId start, ISymbol<TIn> input, ISymbol<TStack> stackTop, ISymbol<TOut> output, TId end, StackActions stackAction, ISymbol<TStack> toPush,bool check)
            : base(start, input, output, end)
        {
            StackAction = stackAction;
            StackTop = stackTop;
            ToPush = toPush;
            CheckStack = check;
        }
        /// <summary>
        /// Конструктор для случая, когда все равно, какой символ лежит на вершине стека
        /// </summary>
        ///<param name="start">Исходное состояние.</param>
        ///<param name="input">Входной символ.</param>
        ///<param name="output">Выходной символ.</param>
        ///<param name="end">Конечное состояние.</param>
        ///<param name="stackAction">Действие с памятью.</param>
        ///<param name="toPush">Символ, который необходимо добавить в стек (если необходимо).</param>
        public IdPushDownStepSignature(TId start, ISymbol<TIn> input, ISymbol<TOut> output, TId end, StackActions stackAction, ISymbol<TStack> toPush)
            : base(start, input, output, end)
        {
            CheckStack = false;
            ToPush = toPush;
            StackAction = stackAction;
        }
        ///<summary>
        /// Конструктор, для случая, когда проверять вершину стека не надо и делать с ним тоже ничего не надо.
        ///</summary>
        ///<param name="start">Исходное состояние.</param>
        ///<param name="input">Входной символ.</param>
        ///<param name="output">Выходной символ.</param>
        ///<param name="end">Конечное состояние.</param>
        public IdPushDownStepSignature(TId start, ISymbol<TIn> input, ISymbol<TOut> output, TId end):base(start,input,output,end)
        {
            CheckStack = false;
            StackAction = StackActions.Nothing;
        }

        #region Implementation of IComparable<in IdPushDownStepSignature<TIn,TOut>>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(IdPushDownStepSignature<TIn, TOut, TStack, TId> other)
        {
            int cmp = base.CompareTo(other);
            if (cmp != 0)
                return cmp;
            cmp = StackAction.CompareTo(other.StackAction);
            if(cmp!=0)
                return cmp;
          
            if (StackAction != StackActions.Nothing)
            {
                cmp = ToPush.CompareTo(other.ToPush);
                if(cmp!=0)
                    return cmp;

            }
            if (CheckStack && other.CheckStack)
                return StackTop.CompareTo(other.StackTop);
            if (!CheckStack && !other.CheckStack)
                return 0;
            return 1;

        }

        #endregion

        #region Implementation of IEquatable<IdPushDownStepSignature<TIn,TOut>>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IdPushDownStepSignature<TIn, TOut, TStack, TId> other)
        {
            return CompareTo(other) == 0;
        }

        /// <summary>
        /// Метод, сравнивающий данную сигнатуру с сигнатурой общего вида.
        /// </summary>
        /// <param name="other">Сигнатура.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        public override int CompareTo(IdStepSignature<TIn, TOut, TId> other)
        {
            var o = other as IdPushDownStepSignature<TIn, TOut, TStack, TId>;
            if(o == null)
                throw new ApplicationException("Wrong type");
            return CompareTo(o);
        }


        public override bool Equals(IdStepSignature<TIn, TOut, TId> other)
        {
            var o = other as IdPushDownStepSignature<TIn, TOut, TStack, TId>;
            if (o == null)
                throw new ApplicationException("Wrong type");
            return Equals(o);
        }
        #endregion

        public override object Clone()
        {
            return new IdPushDownStepSignature<TIn, TOut, TStack, TId>(StartState, Input, StackTop,
                                                                  Output, EndState, StackAction, ToPush,CheckStack);
        }
    }
}
