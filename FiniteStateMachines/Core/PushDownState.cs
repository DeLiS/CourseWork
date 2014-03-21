using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiniteStateMachines.Utility;
using FiniteStateMachines.Interfaces;
namespace FiniteStateMachines.Core
{
    /// <remarks>
    /// Состояние конечного автомата с магазинной памятью
    /// </remarks>
    /// <typeparam name="TIn">Тип входных символов.</typeparam>
    /// <typeparam name="TOut">Тип выходных символов.</typeparam>
    /// <typeparam name="TStack">Тип символов, хранимых в памяти.</typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний.</typeparam>
    public class PushDownState<TIn, TOut, TStack, TId> : State<TIn, TOut, TId>
        where TIn : IComparable<TIn>, IEquatable<TIn>
        where TOut : IComparable<TOut>, IEquatable<TOut>
        where TStack : IComparable<TStack>, IEquatable<TStack>
        where TId : IComparable<TId>, IEquatable<TId>
    {
        ///<summary>
        /// Конструктор без параметров.
        ///</summary>
        protected PushDownState(){}

        ///<summary>
        /// Конструктор, создающий состояние заданного типа.
        ///</summary>
        ///<param name="type">Тип состояния.</param>
        ///<param name="generator">Генератор идентификаторов состояний.</param>
        public PushDownState(StateType type,IGenerator<TId> generator): base(type,generator){}

        public override void AddStep(RefStepSignature<TIn, TOut, TId> signature)
        {
            var sig = signature as PushdownRefStepSignature<TIn, TOut, TStack, TId>;
            if (sig == null)
                throw new ApplicationException("Wrong signature type");
            var newAdjState = new PushdownAdjacentState<TIn, TOut, TStack, TId>(sig);
            if (!AdjacentList.Contains(newAdjState))
                AdjacentList.Add(newAdjState);
        }

        public override void RemoveStep(RefStepSignature<TIn, TOut, TId> signature)
        {
            var sig = signature as PushdownRefStepSignature<TIn, TOut, TStack, TId>;
            if (sig == null)
                throw new ApplicationException("Wrong signature type");
            var newAdjState = new PushdownAdjacentState<TIn, TOut, TStack, TId>(sig);
            AdjacentList.RemoveAll(x => x.Equals(newAdjState));
        }
        
        public override ISet<RefStepSignature<TIn, TOut, TId>> GetStepResult(StepQuery<TIn> query)
        {
            var q = query as PushdownStepQuery<TIn, TStack>;
            if (q == null)
                throw new ApplicationException("Wrong query type");
            var result = new SortedSet<RefStepSignature<TIn, TOut, TId>>();
            foreach (var adjacentState in AdjacentList)
            {
                var adj = adjacentState as PushdownAdjacentState<TIn, TOut, TStack, TId>;
                if (adj == null)
                    throw new ApplicationException("Wrong adjacent state type");
                if (adj.Input.Equals(q.Input))
                {
                    if (adj.Input.Equals(q.Input))
                    {
                        if (adj.Check && adj.StackTop.Equals(q.StackTop))
                            result.Add(new PushdownRefStepSignature<TIn, TOut, TStack, TId>(this, adj.Input, adj.Output,
                                                                                       adj.TargetState, adj.StackAction,
                                                                                       adj.ToPush, adj.StackTop));
                        if (!adj.Check)
                            result.Add(new PushdownRefStepSignature<TIn, TOut, TStack, TId>(this, adj.Input, adj.Output,
                                                                                       adj.TargetState, adj.StackAction,
                                                                                       adj.ToPush));
                    }
                }
            }
            return result;

        }

        public override IEnumerable<IdStepSignature<TIn, TOut, TId>> AdjacentStates
        {
            get
            {
                return
                    AdjacentList.Select(adjacentState => adjacentState as PushdownAdjacentState<TIn, TOut, TStack, TId>).Select(
                        pdaAdjState =>
                        new IdPushDownStepSignature<TIn, TOut, TStack, TId>(Id, pdaAdjState.Input, pdaAdjState.StackTop,
                                                                       pdaAdjState.Output, pdaAdjState.TargetState.Id,
                                                                       pdaAdjState.StackAction, pdaAdjState.ToPush,
                                                                       pdaAdjState.Check));
            }
        }
    }
}
