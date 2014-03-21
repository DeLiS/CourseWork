using System;
using FiniteStateMachines.Utility;
using FiniteStateMachines.Interfaces;

namespace FiniteStateMachines.Core
{
    ///<summary>
    /// Смежное состояние для автомата с магазинной памятью.
    ///</summary>
    ///<typeparam name="TIn">Тип входных символов.</typeparam>
    ///<typeparam name="TOut">Тип выходных символов.</typeparam>
    ///<typeparam name="TStack">Тип символа магазинной памяти (стека).</typeparam>
    ///<typeparam name="TId">Тип идентификаторов состояний.</typeparam>
    public class PushdownAdjacentState<TIn, TOut, TStack, TId> : AdjacentState<TIn, TOut, TId>
        where TIn : IComparable<TIn>, IEquatable<TIn>
        where TOut : IComparable<TOut>, IEquatable<TOut>
        where TStack : IComparable<TStack>, IEquatable<TStack>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        ///<summary>
        /// Действие, которое необходимо выполнить с памятью при переходе.
        ///</summary>
        public StackActions StackAction { get; private set; }

        ///<summary>
        /// Символ, который необходимо добавить в стек при переходе (если это необходимо).
        ///</summary>
        public ISymbol<TStack> ToPush { get; private set; }

        ///<summary>
        /// Истина, если следует проверять на эквивалентность символ на вершине памяти путешественника и символ StackTop.
        /// 
        ///</summary>
        public bool Check { get; private set; }

        ///<summary>
        /// Символ, с которым сравнивается вершина памяти путешественника, если значение Check - истина.
        ///</summary>
        public ISymbol<TStack> StackTop { get; private set; }

        ///<summary>
        /// Конструктор смежного состояния.
        ///</summary>
        ///<param name="signature">Сигнатура перехода</param>
        public PushdownAdjacentState(PushdownRefStepSignature<TIn, TOut, TStack, TId> signature)
            : base(signature)
        {
            StackAction = signature.StackAction;
            ToPush = signature.ToPush;
            Check = signature.CheckStack;
            StackTop = signature.StackTop;
        }


        public override int CompareTo(AdjacentState<TIn, TOut, TId> other)
        {
            int baseCmp = base.CompareTo(other);
            if (baseCmp != 0)
                return baseCmp;
            var pdas = other as PushdownAdjacentState<TIn, TOut, TStack, TId>;
            if (pdas == null)
                throw new ApplicationException("Wrong adjacent state type");
            int actionCmp = StackAction.CompareTo(pdas.StackAction);
            if (actionCmp != 0)
                return actionCmp;
            if (StackAction != StackActions.Nothing)
            {
                int toPushCmp = ToPush.CompareTo(pdas.ToPush);
                if (toPushCmp != 0)
                    return toPushCmp;
            }
            if (!Check && !pdas.Check)
                return 0;
            if (Check && pdas.Check)
                return StackTop.CompareTo(pdas.StackTop);
            return Check ? 1 : -1;

        }

        public override bool Equals(AdjacentState<TIn, TOut, TId> other)
        {
            var o = other as PushdownAdjacentState<TIn, TOut, TStack, TId>;
            if (o == null)
                return false;
            return CompareTo(o) == 0;
        }
    }
}
