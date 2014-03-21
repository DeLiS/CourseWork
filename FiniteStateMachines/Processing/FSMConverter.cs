#region

using System;
using System.Collections.Generic;
using System.Linq;
using FiniteStateMachines.Core;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;

#endregion

namespace FiniteStateMachines.Processing
{
    /// <remarks>
    /// Класс, преобразовывающий недетерминированный конечный автомат в детерминированный. 
    /// Также класс умеет делать из автомата автомат-распознаватель нетерминала. 
    /// </remarks>
    /// <typeparam name="TIn">Тип входящих символов.</typeparam>
    /// <typeparam name="TOut">Тип результирующих символов.</typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний автомата.</typeparam>
    public class FSMConverter<TIn, TOut, TId>
        where TIn : IComparable<TIn>, IEquatable<TIn>
        where TOut : IComparable<TOut>, IEquatable<TOut>
        where TId: IComparable<TId>, IEquatable<TId>
    {
        /// <summary>
        /// Состояния детерминированного автомата и соответствующие им подмножества множества состояний старого автомата.
        /// </summary>
        private readonly Dictionary<TId, SortedSet<TId>> _stateSubsets = new Dictionary<TId, SortedSet<TId>>();
        private readonly Dictionary<TId, TId> _oldNewStates = new Dictionary<TId, TId>();
        private readonly IGenerator<TId> _generator;

        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="nfa">Обрабатываемый автомат.</param>
        ///<param name="generator">Генератор уникальных идентификаторов состояний автомата.</param>
        public FSMConverter(NFA<TIn, TOut, TId> nfa,IGenerator<TId> generator)
        {
            Nfa = nfa;
            _generator = generator;
        }

        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="generator">Генератор уникальных идентификаторов состояний автомата.</param>
        public FSMConverter(IGenerator<TId> generator)
        {
            _generator = generator;
        }
        ///<summary>
        /// Недетерминированный автомат.
        ///</summary>
        public NFA<TIn, TOut, TId> Nfa { get; private set; }
        ///<summary>
        /// Детерминированный конечный автомат.
        ///</summary>
        public DFA<TIn, TOut, TId> Dfa { get; private set; }

        /// <summary>
        /// Метод, конвертирующий недетерминированный автомат, хранящийся в поле NFA в детерминированный, сохраняемый в поле Dfa.
        /// </summary>
        public virtual void Convert()
        {
           Dfa = new DFA<TIn, TOut, TId>(_generator);
            var oldStartStates = Nfa.GetStartStates();
            var startEpsilonClosure = EpsilonClosure(oldStartStates);
            bool end = false;
            var closures = new List<SortedSet<TId>>();
            closures.Add(startEpsilonClosure);
            foreach (var id in startEpsilonClosure)
            {
                if (Nfa.IsEndState(id))
                    end = true;
            }
            var globalStart = Dfa.CreateNewState(end ? StateType.StartEndState : StateType.StartState);
            _stateSubsets[globalStart] = startEpsilonClosure;
            var used = new SortedSet<TId>();
            var inWork = new List<TId>();
            inWork.Add(globalStart);
            for(int i=0;i<inWork.Count;++i)
            {
                var current = inWork[i];
                used.Add(current);
                var oldStates = _stateSubsets[current];
                var totalOutput = new Dictionary<ISymbol<TIn>,SortedSet<ISymbol<TOut>>>();
                var targetStates = new Dictionary<ISymbol<TIn>, SortedSet<TId>>();
                foreach (var oldState in oldStates)
                {
                    var adjacent = Nfa.AdjacentStates(oldState);
                    foreach (var inputNstates in adjacent)
                    {
                        var input = inputNstates.Key;
                        var output = Nfa.GetOutSymbol(oldState, input);
                        if(!totalOutput.ContainsKey(input))
                            totalOutput[input] = new SortedSet<ISymbol<TOut>>();
                        totalOutput[input].UnionWith(output);
                        if (input.Type == SymbolType.Empty) continue;
                        if (!targetStates.ContainsKey(input))
                            targetStates[input] = new SortedSet<TId>();
                        targetStates[input].UnionWith(inputNstates.Value);
                    }
                }
                foreach (var targetState in targetStates)
                {
                    var input = targetState.Key;
                    var subset = EpsilonClosure(targetState.Value);
                    bool alreadyBeen = false;
                    TId id = default(TId);
                    foreach (var stateSubset in _stateSubsets)
                    {
                        var someSubset = stateSubset.Value;
                        if(AreEqual(subset,someSubset))
                        {
                            alreadyBeen = true;
                            id = stateSubset.Key;
                        }
                    }
                    if(!alreadyBeen)
                    {
                        var isEndState = false;
                        foreach (var id1 in subset)
                        {
                            if (Nfa.IsEndState(id1))
                            {
                                isEndState = true;
                            }
                            
                        }
                        id = Dfa.CreateNewState(isEndState ? StateType.EndState : StateType.TransitionalState);
                        _stateSubsets[id] = subset;
                        foreach (var id1 in subset)
                        {
                            _oldNewStates[id1] = id;
                        }
                        inWork.Add(id);
                    }
                    var multiSymbol = new MultiSymbol<TOut>();
                    var symbols = totalOutput[input];
                    foreach (var symbol in symbols)
                    {
                        multiSymbol.AddSymbol(symbol);
                    }
                    Dfa.AddStep(new IdStepSignature<TIn, TOut, TId>(current,input,multiSymbol,id));
                }

            }
        }

        /// <summary>
        /// Эпсилон-замыкание множества состояний. Реализовано чем-то похожим на поиск в глубину.
        /// </summary>
        /// <param name="states">Множество состояний.</param>
        /// <returns>Начальные состояния + состояния, достижимые из них по пустым переходам.</returns>
        private SortedSet<TId> EpsilonClosure(IEnumerable<TId> states)
        {
            var result = new SortedSet<TId>();
            var used = new Dictionary<TId, bool>();
            var stack = new Stack<TId>();
            foreach (TId guid in states)
            {
                stack.Push(guid);
                used[guid] = true;
            }

            while (stack.Count > 0)
            {
                TId current = stack.Pop();
                result.Add(current);
                SortedDictionary<ISymbol<TIn>, SortedSet<TId>> adjacentStates = Nfa.AdjacentStates(current);
                foreach (KeyValuePair<ISymbol<TIn>, SortedSet<TId>> adjacentState in adjacentStates)
                {
                    if (adjacentState.Key.Type == SymbolType.Empty)
                        foreach (TId state in adjacentState.Value)
                            if (!used.ContainsKey(state) || used[state] == false)
                            {
                                stack.Push(state);
                                used[state] = true;
                            }
                }


            }
            return result;
        }

        ///<summary>
        /// Метод, заменяющий в автомате все выходные символы на пустые, если это не переход в конечное состояние, 
        /// иначе - на символ <paramref name="endSignal"/>.
        ///</summary>
        ///<param name="nfa">Недетерминированный автомат.</param>
        ///<param name="endSignal">Символ, который автомат должен выводить при переходе в конечное состояние.</param>
        public virtual void MakeAcceptor(NFA<TIn, TOut, TId> nfa, ISymbol<TOut> endSignal)
        {
            var idStepSignatures = nfa.IdStepSignatures;
            var toMakeEmpty = new SortedSet<IdStepSignature<TIn, TOut, TId>>();
            var toMakeSignal = new SortedSet<IdStepSignature<TIn, TOut, TId>>();
            foreach (var idStepSignature in idStepSignatures)
            {
                if (nfa.IsEndState(idStepSignature.EndState))
                {
                    toMakeSignal.Add(idStepSignature);
                }
                else
                {
                    toMakeEmpty.Add(idStepSignature);
                }
            }
            foreach (var idStepSignature in toMakeEmpty)
            {
                nfa.RemoveStep(idStepSignature);
                idStepSignature.Output = Symbol<TOut>.Empty;
                nfa.AddStep(idStepSignature);
            }
            foreach (var idStepSignature in toMakeSignal)
            {
                nfa.RemoveStep(idStepSignature);
                idStepSignature.Output = endSignal;
                nfa.AddStep(idStepSignature);
            }
        }

        private static bool AreEqual(SortedSet<TId> first, SortedSet<TId> second)
        {
            return first.IsSubsetOf(second) && second.IsSubsetOf(first);
        }
    }
}
