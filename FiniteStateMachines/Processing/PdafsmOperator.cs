using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiniteStateMachines.Core;
using FiniteStateMachines.Utility;
using FiniteStateMachines.Interfaces;
namespace FiniteStateMachines.Processing
{
    /// <remarks>
    /// Класс, инкапсулирующий операции над автоматами с магазинной памятью.
    /// </remarks>
    /// <typeparam name="TIn">Тип входных символов.</typeparam>
    /// <typeparam name="TOut">Тип выходных символов.</typeparam>
    /// <typeparam name="TStack">Тип символов магазинной памяти. </typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний автомата.</typeparam>
    public class PdafsmOperator<TIn, TOut, TStack, TId> : FSMOperator<TIn, TOut,TId>
        where TIn : IComparable<TIn>, IEquatable<TIn>
        where TOut : IComparable<TOut>, IEquatable<TOut>
        where TStack : IComparable<TStack>, IEquatable<TStack>
        where TId:IComparable<TId>, IEquatable<TId>
    {
        private readonly IGenerator<TStack> _generator;
        private readonly Dictionary<Pair<TId, TId>, TStack> _stackSymbol = new Dictionary<Pair<TId, TId>, TStack>();
        ///<summary>
        /// Конструктор
        ///</summary>
        ///<param name="first">Первый операнд.</param>
        ///<param name="second">Второй операнд.</param>
        ///<param name="generator">Генератор уникальных символов магазинной памяти.</param>
        ///<param name="generatorId">Генератор уникальных идентификаторов состояний.</param>
        public PdafsmOperator(NFA<TIn, TOut,TId> first, NFA<TIn, TOut, TId> second, IGenerator<TStack> generator,IGenerator<TId> generatorId)
            : base(first, second,generatorId)
        {
            _generator = generator;
        }

        ///<summary>
        /// Конструктор, принимающий список автоматов.
        ///</summary>
        ///<param name="automatons">Список автоматов.</param>
        ///<param name="generator">Генератор уникальных символов магазинной памяти автомата.</param>
        ///<param name="generatorId">Генератор уникальных идентификаторов состояний автомата.</param>
        public PdafsmOperator(List<NFA<TIn, TOut, TId>> automatons, IGenerator<TStack> generator,IGenerator<TId> generatorId)
            : base(automatons,generatorId)
        {
            _generator = generator;
        }
        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="generator">Генератор уникальных символов магазинной памяти.</param>
        ///<param name="generatorId">Генератор уникальных идентификаторов состояний автомата.</param>
        public PdafsmOperator(IGenerator<TStack> generator,IGenerator<TId> generatorId):base(generatorId)
        {
            _generator = generator;
        }
        /// <summary>
        /// Метод, удаляющий рекурсию в первом операнде.
        /// </summary>
        /// <param name="nonterminal">Нетерминал, который распознает автомат <paramref name="acceptor"/></param>.
        /// <param name="acceptor">Автомат, распознающий нетерминал. <paramref name="nonterminal"/></param>
        /// <param name="right">Абсолютно неважное для данного метода поле.</param>
        public override void RecursionRemover(ISymbol<TIn> nonterminal, NFA<TIn, TOut, TId> acceptor, bool right)
        {
            Result = acceptor;
            var idStepSignatures = acceptor.IdStepSignatures;
            var toRemove = new SortedSet<IdStepSignature<TIn,TOut,TId>>();
            var beginsEnd = new SortedSet<Pair<TId, TId>>();
            var startStates = acceptor.GetStartStates();
            var endStates = acceptor.GetEndStates();
            foreach (var idStepSignature in idStepSignatures)
            {
                if (!idStepSignature.Input.Equals(nonterminal)) continue;
                toRemove.Add(idStepSignature);
                TStack toPush = _generator.GetUniqueId();
                var keyValue = new Pair<TId, TId>(idStepSignature.StartState, idStepSignature.EndState);
                _stackSymbol[keyValue] = toPush;
                beginsEnd.Add(keyValue);
            }
            foreach (var idStepSignature in toRemove)
            {
                acceptor.RemoveStep(idStepSignature);
            }

            foreach (var keyValuePair in beginsEnd)
            {
                if (acceptor.IsStartState(keyValuePair.Key) && acceptor.IsEndState(keyValuePair.Value))
                    continue;
                if (acceptor.IsStartState(keyValuePair.Key))
                {
                    foreach (var endState in endStates)
                    {
                        AddEmptyStep(endState, keyValuePair.Value);
                    }
                    continue;
                }
                if (acceptor.IsEndState(keyValuePair.Value))
                {
                    foreach (var startState in startStates)
                    {
                        AddEmptyStep(keyValuePair.Key, startState);
                    }
                    continue;
                }
                var toPush = _stackSymbol[keyValuePair];
                var pushSymbol = new Symbol<TStack>(toPush, SymbolType.Terminal);
                foreach (var startState in startStates)
                {
                    acceptor.AddStep(new IdPushDownStepSignature<TIn, TOut, TStack, TId>(start:keyValuePair.Key, input:Symbol<TIn>.Empty,
                                                                                    output:Symbol<TOut>.Empty, end:startState,
                                                                                    stackAction:StackActions.Push, toPush:pushSymbol));
                }
                foreach (var endState in endStates)
                {
                    acceptor.AddStep(new IdPushDownStepSignature<TIn, TOut, TStack, TId>(start:endState, input:Symbol<TIn>.Empty,
                                                                                    stackTop:pushSymbol, output:Symbol<TOut>.Empty,
                                                                                    end: keyValuePair.Value, stackAction: StackActions.Pop,
                                                                                    toPush:Symbol<TStack>.Empty, check:true));
                }
            }

            AddFinishOnEmptyStack(acceptor);
        }

        protected override void AddEmptyStep(TId start, TId end)
        {
            var pda = Result as NPDA<TIn, TOut, TStack, TId>;
            if (pda == null) throw new ApplicationException("Can't convert nfa to npda");
            pda.AddStep(new IdPushDownStepSignature<TIn, TOut, TStack, TId>(start: start, input: Symbol<TIn>.Empty, output:Symbol<TOut>.Empty, end:end,
                                                                       stackAction: StackActions.Nothing, toPush: Symbol<TStack>.Empty));
        }
        protected override NFA<TIn, TOut, TId> CreateStateMachine()
        {
            return new NPDA<TIn, TOut, TStack, TId>(GeneratorId,_generator);
        }
        protected virtual void AddFinishOnEmptyStack(NFA<TIn,TOut,TId> acceptor)
        {
            var pdaAcceptor = acceptor as NPDA<TIn, TOut, TStack, TId>;
            if(pdaAcceptor==null)
                throw new ApplicationException("Wrong automaton");
            var pda = new NPDA<TIn, TOut, TStack, TId>(pdaAcceptor.GeneratorTId, pdaAcceptor.GeneratorTStack);
            var start = pda.CreateNewState(StateType.StartState);
            var transitionalFirst = pda.CreateNewState(StateType.TransitionalState);
            var transitionalSecond = pda.CreateNewState(StateType.TransitionalState);
            var end = pda.CreateNewState(StateType.EndState);
            var stackBottom = pda.GeneratorTStack.GetUniqueId();
            var bottomSymbol = new Symbol<TStack>(stackBottom, SymbolType.Terminal);
            var firstSignature = new IdPushDownStepSignature<TIn, TOut, TStack, TId>(start, Symbol<TIn>.Empty,
                                                                                     Symbol<TOut>.Empty,
                                                                                     transitionalFirst, StackActions.Push,
                                                                                     bottomSymbol);
            var secondSignature = new IdPushDownStepSignature<TIn, TOut, TStack, TId>(transitionalSecond,
                                                                                      Symbol<TIn>.Empty, bottomSymbol,
                                                                                      Symbol<TOut>.Empty, end,
                                                                                      StackActions.Pop,
                                                                                      Symbol<TStack>.Empty, true);
            pda.AddStep(firstSignature);
            pda.AddStep(secondSignature);
            var insertBetween = new PdafsmOperator<TIn, TOut, TStack, TId>(pda, acceptor,
                                                                           pdaAcceptor.GeneratorTStack,
                                                                           pdaAcceptor.GeneratorTId);
            var list = new List<KeyValuePair<TId, TId>>();
            list.Add(new KeyValuePair<TId, TId>(transitionalFirst,transitionalSecond));
            insertBetween.MultiInsertBetween(list);
            Result = insertBetween.Result;
        }
    }
}
