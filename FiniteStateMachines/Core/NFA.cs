using System;
using System.Collections.Generic;
using System.Linq;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;

namespace FiniteStateMachines.Core
{
    /// <summary>
    /// Недетерменированный конечный автомат автомат
    /// </summary>
    /// <typeparam name="TIn">Тип входных символов.</typeparam>
    /// <typeparam name="TOut">Тип выходных символов.</typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний.</typeparam>
    public class NFA<TIn, TOut, TId>
        where TIn : IEquatable<TIn>, IComparable<TIn>
        where TOut : IEquatable<TOut>, IComparable<TOut>
        where TId : IComparable<TId>, IEquatable<TId>
    {
        #region Переменные класса

        /// <summary>
        /// Текущие путешественники автомата.
        /// </summary>
        protected ISet<FSMTraveller<TIn, TOut, TId>> Travellers;
        /// <summary>
        /// Множество начальных состояний.
        /// </summary>
        protected readonly ISet<IState<TIn, TOut, TId>> _startStates;
        /// <summary>
        /// Множество конечных состояний.
        /// </summary>
        private readonly ISet<IState<TIn, TOut, TId>> _endStates;
        /// <summary>
        /// Множество промежуточных состояний.
        /// </summary>
        private readonly ISet<IState<TIn, TOut, TId>> _transitionalStates;

        /// <summary>
        /// Состояния, являющиеяся и начальными и конечными
        /// </summary>
        protected readonly ISet<IState<TIn, TOut, TId>> _startEndStates;

        ///<summary>
        /// Генератор идентификаторов состояний.
        ///</summary>
        public IGenerator<TId> GeneratorTId { get; private set; }
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        private NFA()
        {
            _startStates = new SortedSet<IState<TIn, TOut, TId>>();
            _endStates = new SortedSet<IState<TIn, TOut, TId>>();
            _startEndStates = new SortedSet<IState<TIn, TOut, TId>>();
            _transitionalStates = new SortedSet<IState<TIn, TOut, TId>>();
            //CurrentStates = StartStates;
            Travellers = new SortedSet<FSMTraveller<TIn, TOut, TId>>();
        }
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="generatorTId">Генератор идентификаторов состояний.</param>
        public NFA(IGenerator<TId> generatorTId)
            : this()
        {
            GeneratorTId = generatorTId;
        }

        #endregion

        #region Открытые метод изменения автомата

        ///<summary>
        /// Метод, добавляющий в автомат новый переход.
        ///</summary>
        ///<param name="idStepSignature">Сигнатура перехода.</param>
        public virtual void AddStep(IdStepSignature<TIn, TOut, TId> idStepSignature)
        {
            var sig = GetRefStepSignature(idStepSignature);
            sig.StartState.AddStep(sig);
        }

        /// <summary>
        /// Метод, удаляющий из автомата переход.
        /// </summary>
        /// <param name="idStepSignature">Сигнатура удаляемого перехода.</param>
        public virtual void RemoveStep(IdStepSignature<TIn, TOut, TId> idStepSignature)
        {
            var sig = GetRefStepSignature(idStepSignature);
            sig.StartState.RemoveStep(sig);

        }

        /// <summary>
        /// Метод, моделирующий переход автомата по входному символу.
        /// </summary>
        /// <param name="input">Входной символ.</param>
        /// <returns>Множество выходных символов.</returns>
        public virtual ISet<ISymbol<TOut>> MakeStep(ISymbol<TIn> input)
        {
            var query = CreateQuery(input);
            var result = new SortedSet<ISymbol<TOut>>();
            var newTravellers = new SortedSet<FSMTraveller<TIn, TOut, TId>>();
            if (input.Type == SymbolType.Empty)
            {
                foreach (var fsmTraveller in Travellers)
                {
                    newTravellers.Add(fsmTraveller);
                }
            }
            foreach (var fsmTraveller in Travellers)
            {

                if (fsmTraveller.CalcAvailableSteps(query))
                {
                    var availableSteps = fsmTraveller.AvailableSteps;
                    foreach (var refStepSignature in availableSteps)
                    {
                        if (refStepSignature.OutputSymbol.Type != SymbolType.Empty)
                            result.Add(refStepSignature.OutputSymbol);
                        var traveller = fsmTraveller.CreateNewTraveller(refStepSignature);
                        newTravellers.Add(traveller);
                    }
                }
            }
            var closure = EpsilonClosure(newTravellers);
            foreach (var fsmTraveller in closure)
            {
                if (fsmTraveller.LastStep.OutputSymbol.Type != SymbolType.Empty)
                    result.Add(fsmTraveller.LastStep.OutputSymbol);
            }
            newTravellers.UnionWith(closure);
            Travellers = newTravellers;
            if (result.Count == 0)
                result.Add(new Symbol<TOut>());
            return result;
        }

        /// <summary>
        /// Метод, создающий в автомате новое состояние.
        /// </summary>
        /// <param name="stateType">Тип создаваемого состояния.</param>
        /// <returns>Идентификатор созданного состояния.</returns>
        public virtual TId CreateNewState(StateType stateType)
        {
            var state = CreateState(stateType);
            switch (stateType)
            {
                case StateType.StartState:
                    _startStates.Add(state);
                    Travellers.Add(CreateTraveller(state));
                    break;
                case StateType.EndState:
                    _endStates.Add(state);
                    break;
                case StateType.TransitionalState:
                    _transitionalStates.Add(state);
                    break;
                case StateType.StartEndState:
                    _startEndStates.Add(state);
                    break;
                default:
                    throw new FsmException<TIn, TOut, TId>(FsmExceptionId.WrongStateType);
            }
            return state.Id;
        }

        ///<summary>
        /// Метод, сбрасывающий автомат в исходное состояние.
        ///</summary>
        public virtual void Reset()
        {
            Travellers.Clear();
            foreach (var startState in _startStates)
            {
                Travellers.Add(CreateTraveller(startState));
            }
            MakeStep(new Symbol<TIn>());
        }

        ///<summary>
        /// Состояния автомата.
        ///</summary>
        public IEnumerable<TId> States
        {
            get
            {
                foreach (var startState in _startStates)
                {
                    yield return startState.Id;
                }
                foreach (var transitionalState in _transitionalStates)
                {
                    yield return transitionalState.Id;
                }
                foreach (var endState in _endStates)
                {
                    yield return endState.Id;
                }
                foreach (var startEndState in _startEndStates)
                {
                    yield return startEndState.Id;
                }
            }
        }

        #endregion

        #region Открытые методы получения информации о  состоянии и устройстве автомата (различные геттеры)

        ///<summary>
        /// Метод, проверяющий, находится ли автомат в конечном состоянии.
        ///</summary>
        ///<returns>Истина, если автомат находится в конечном состоянии, ложь в противном случае.</returns>
        public virtual bool AtFinish()
        {
            // return CurrentStates.Any(currentState => currentState.IsEndState());)
            return Travellers.Any(x => x.CurrentState.IsEndState());
        }

        ///<summary>
        /// Количество начальных состояний автомата.
        ///</summary>
        public int StartStatesCount { get { return _startStates.Count + _startEndStates.Count; } }

        /// <summary>
        /// Количество конечных состояний автомата.
        /// </summary>
        public int EndStatesCount { get { return _endStates.Count + _startEndStates.Count; } }

        /// <summary>
        /// Количество промежуточных состояний автомата.
        /// </summary>
        public int TransitionalStatesCount { get { return _transitionalStates.Count; } }

        /// <summary>
        /// Количество состояний, который одновременно и начальные, и конечные.
        /// </summary>
        public int StartEndStatesCount { get { return _startEndStates.Count; } }

        /// <summary>
        /// Общее количество состояний автомата.
        /// </summary>
        public int TotalStates { get { return _startStates.Count + _endStates.Count + _startEndStates.Count + _transitionalStates.Count; } }

        ///<summary>
        /// Количество переходов в автомате
        ///</summary>
        public int TransitionsCount
        {
            get { 
                int count = 0;
                foreach (var startState in _startStates)
                {
                    count += startState.AdjacentStates.Count();
                }
                foreach (var startEndState in _startEndStates)
                {

                    count += startEndState.AdjacentStates.Count();
                }
                foreach (var endState in _endStates)
                {
                    count += endState.AdjacentStates.Count();
                }
                foreach (var transitionalState in _transitionalStates)
                {
                    count += transitionalState.AdjacentStates.Count();
                }
                return count;
            }
        }

        ///<summary>
        /// Метод, возвращающий результирующий символ при переходе из состояния с идентификатором <paramref name="stateId"/> при входном символе <paramref name="symbol"/>
        ///</summary>
        ///<param name="stateId">Идентификатор состояния, из которого необходимо совершить переход.</param>
        ///<param name="symbol">Входной символ.</param>
        ///<returns>Множество выходных символов.</returns>
        public ISet<ISymbol<TOut>> GetOutSymbol(TId stateId, ISymbol<TIn> symbol)
        {
            var state = GetStateById(stateId);
            var query = new StepQuery<TIn>(symbol);
            var stepResult = state.GetStepResult(query);
            var result = new SortedSet<ISymbol<TOut>>();
            foreach (var refStepSignature in stepResult)
            {
                result.Add(refStepSignature.OutputSymbol);
            }
            return result;
        }

        ///<summary>
        /// Метод, возвращающий идентификаторы конечных состояний автомата.
        ///</summary>
        ///<returns>Множество идентификаторов конечных состояний автомата .</returns>
        public SortedSet<TId> GetEndStates()
        {
            var result = new SortedSet<TId>();
            foreach (var endState in _endStates)
            {
                result.Add(endState.Id);
            }
            foreach (var startEndState in _startEndStates)
            {
                result.Add(startEndState.Id);
            }
            return result;
        }

        ///<summary>
        /// Смежные состояния состояния <paramref name="stateId"/>.
        ///</summary>
        /// <param name="stateId">Состояние автомата.</param>
        ///<returns>Словарь "Входной символ - Множество новых состояний, в которых будет находится автомат после перехода".</returns>
        public SortedDictionary<ISymbol<TIn>, SortedSet<TId>> AdjacentStates(TId stateId)
        {
            var dictionary = new SortedDictionary<ISymbol<TIn>, SortedSet<TId>>();
            var state = GetStateById(stateId);
            var d = state.GetAdjacentStates();
            foreach (var x in d)
            {
                var set = x.Value;
                foreach (var setElement in set)
                {
                    if (!dictionary.ContainsKey(x.Key))
                        dictionary.Add(x.Key, new SortedSet<TId>());
                    dictionary[x.Key].Add(setElement.Id);
                }
            }
            return dictionary;
        }

        ///<summary>
        /// Метод, возвращающий идентификаторы начальных состояний автомата.
        ///</summary>
        ///<returns>Множество идентификаторов начальных состояний автомата.</returns>
        public SortedSet<TId> GetStartStates()
        {
            return new SortedSet<TId>(_startStates.Select(x => x.Id).Union(_startEndStates.Select(x => x.Id)));
        }

        ///<summary>
        /// Метод, проверяющий является ли состояние с идентификатором <paramref name="stateId"/> 
        /// стартовым состоянием автомата.
        ///</summary>
        ///<param name="stateId">Идентификатор состояния.</param>
        ///<returns>Истина, если состояние с идентификатором <paramref name="stateId"/> является стартовым. </returns>
        public bool IsStartState(TId stateId)
        {
            return _startStates.Contains(GetStateById(stateId)) || _startEndStates.Contains(GetStateById(stateId));
        }

        ///<summary>
        /// Метод, проверяющий является ли состояние с идентификатором <paramref name="stateId"/> 
        /// конечным состоянием автомата.
        ///</summary>
        ///<param name="stateId">Идентификатор состояния.</param>
        ///<returns>Истина, если состояние с идентификатором <paramref name="stateId"/> является конечным. </returns>
        public bool IsEndState(TId stateId)
        {
            return _endStates.Contains(GetStateById(stateId)) || _startEndStates.Contains(GetStateById(stateId));
        }

        ///<summary>
        /// Метод, возвращающий тип состояния с идентификатором <paramref name="stateId"/>.
        ///</summary>
        ///<param name="stateId">Идентификатор состояния.</param>
        ///<returns>Тип состояния с идентификатором <paramref name="stateId"/>.</returns>
        ///<exception cref="FsmException<TIn,TOut>">Исключение происходит при отстутствии состояния с заданынм номером</exception>
        public StateType GetStateType(TId stateId)
        {
            bool f1 = _startStates.Contains(GetStateById(stateId));
            if (f1)
                return StateType.StartState;
            bool f2 = _endStates.Contains(GetStateById(stateId));
            if (f2)
                return StateType.EndState;
            bool f3 = _transitionalStates.Contains(GetStateById(stateId));
            if (f3)
                return StateType.TransitionalState;
            bool f4 = _startEndStates.Contains(GetStateById(stateId));
            if (f4)
                return StateType.StartEndState;
            throw new FsmException<TIn, TOut, TId>(FsmExceptionId.NoStateWithSuchGuid);
        }

        ///<summary>
        /// Перечисление всех сигнатур переходов, присутствующих в автомате.
        ///</summary>
        public IEnumerable<IdStepSignature<TIn, TOut, TId>> IdStepSignatures
        {
            get
            {
                foreach (var startState in _startStates)
                {
                    var adj = startState.AdjacentStates;
                    foreach (var idStepSignature in adj)
                    {
                        yield return idStepSignature;
                    }
                }
                foreach (var startEndState in _startEndStates)
                {
                    var adj = startEndState.AdjacentStates;
                    foreach (var idStepSignature in adj)
                    {
                        yield return idStepSignature;
                    }
                }
                foreach (var transitionalState in _transitionalStates)
                {
                    var adj = transitionalState.AdjacentStates;
                    foreach (var idStepSignature in adj)
                    {
                        yield return idStepSignature;
                    }
                }
                foreach (var endState in _endStates)
                {
                    var adj = endState.AdjacentStates;
                    foreach (var idStepSignature in adj)
                    {
                        yield return idStepSignature;
                    }
                }
            }
        }

        ///<summary>
        /// Истина, если в автомате есть хотя бы один путешественник.
        ///</summary>
        public bool IsWorking
        {
            get { return Travellers.Count > 0; }
        }

        #endregion

        #region Вспомагательные методы

        /// <summary>
        /// Метод, создающий ссылочную сигнатуру перехода по сигнатуре с идентификаторами.
        /// </summary>
        /// <param name="idStepSignature">Сигнатура перехода с идентификаторами.</param>
        /// <returns>Ссылочная сигнатура перехода</returns>
        protected virtual RefStepSignature<TIn, TOut, TId> GetRefStepSignature(IdStepSignature<TIn, TOut, TId> idStepSignature)
        {
            IState<TIn, TOut, TId> startState = GetStateById(idStepSignature.StartState);
            IState<TIn, TOut, TId> endState = GetStateById(idStepSignature.EndState);
            if (startState == null)
                throw new FsmException<TIn, TOut, TId>(FsmExceptionId.NoStateWithSuchGuid, idStepSignature.StartState);
            if (endState == null)
                throw new FsmException<TIn, TOut, TId>(FsmExceptionId.NoStateWithSuchGuid, idStepSignature.EndState);
            return new RefStepSignature<TIn, TOut, TId>(startState, idStepSignature.Input, idStepSignature.Output, endState);
        }

        /// <summary>
        /// Метод, возвращающий ссылку на состояние с идентификатором <paramref name="stateId"/>.
        /// </summary>
        /// <param name="stateId">Идентификатор состояния.</param>
        /// <returns>Ссылка на состояние с идентификатором <paramref name="stateId"/></returns>
        protected IState<TIn, TOut, TId> GetStateById(TId stateId)
        {
            IState<TIn, TOut, TId> state;
            state = (_startStates.FirstOrDefault(x => x.Id.Equals(stateId)) ??
                     _endStates.FirstOrDefault(x => x.Id.Equals(stateId))) ??
                    _transitionalStates.FirstOrDefault(x => x.Id.Equals(stateId))??
                    _startEndStates.FirstOrDefault(x=>x.Id.Equals(stateId));
            if (state == null)
                throw new ApplicationException("No such state in automaton.");
            return state;
        }

        /// <summary>
        /// Метод, возвращающий множество путешественников, которые созданы в состояниях, 
        /// достижимых путешественниками из множества <paramref name="set"/> (НЕ включая этих путешественников)
        ///  по переходу с пустым входным символом.
        /// </summary>
        /// <param name="set">Подмножество путешествинников автомата.</param>
        /// <param name="used">Вспомагательное множество обработанных путешествинников 
        /// (при вызове метода оставить по-умолчанию)</param>
        /// <returns>Множество путешествинников</returns>
        private static IEnumerable<FSMTraveller<TIn, TOut, TId>> EpsilonClosure(IEnumerable<FSMTraveller<TIn, TOut, TId>> set, ISet<FSMTraveller<TIn, TOut, TId>> used = null)
        {
            var result = new SortedSet<FSMTraveller<TIn, TOut, TId>>();
            if (used == null)
                used = new SortedSet<FSMTraveller<TIn, TOut, TId>>();
            foreach (var fsmTraveller in set)
            {
                if (used.Contains(fsmTraveller)) continue;
                used.Add(fsmTraveller);
                var epsilonClosure = EpsilonClosure(fsmTraveller); //состояния, достижимые путешественником по одному пустому переходу
                if (epsilonClosure.Count == 0) continue;
                var epsilonClosuresq = EpsilonClosure(epsilonClosure, used); //замыкание 
                epsilonClosure.UnionWith(epsilonClosuresq); //все состояния, достижимые по пустому переходу путешественниками из передаваемого в метод множества
                result.UnionWith(epsilonClosure); //по сути result = epsilonClosure;
            }
            return result;
        }

        /// <summary>
        /// Метод, возвращающий множество путешественников, которые появятся при переходе путешественником
        /// <paramref name="traveller"/> по пустому входному символу.
        /// </summary>
        /// <param name="traveller">Путешественник автомата.</param>
        /// <returns>Множество новых путешественников.</returns>
        private static ISet<FSMTraveller<TIn, TOut, TId>> EpsilonClosure(FSMTraveller<TIn, TOut, TId> traveller)
        {
            var result = new SortedSet<FSMTraveller<TIn, TOut, TId>>();
            if (traveller.CalcAvailableSteps(traveller.CreateEmptyQuery()))
            {
                var refStepSignatures = traveller.AvailableSteps;
                foreach (var refStepSignature in refStepSignatures)
                {
                    var newTraveller = traveller.CreateNewTraveller(refStepSignature);
                    result.Add(newTraveller);
                }
            }
            return result;
        }
        #region Фабричные методы (ну или почти фабричные)

        /// <summary>
        /// Метод, создающий новый объект-состояние.
        /// </summary>
        /// <param name="stateType">Тип создаваемого состояния.</param>
        /// <returns>Объект-состояние для данного автомата.</returns>
        protected virtual IState<TIn, TOut, TId> CreateState(StateType stateType)
        {
            return new State<TIn, TOut, TId>(stateType, GeneratorTId);
        }

        /// <summary>
        /// Метод, создающий путешественника, который получится,
        /// если путешественник перейдет из состояния <paramref name="startState"/> по переходу
        /// с сигнатурой <paramref name="lastStep"/>. 
        /// Если данная сигнатура равна null, то считается, что создается путешественник,
        ///  который не совершал до этого ни одного перехода.
        /// </summary>
        /// <param name="startState">Состояние.</param>
        /// <param name="lastStep">Сигнатура перехода.</param>
        /// <returns>Путешественник.</returns>
        protected virtual FSMTraveller<TIn, TOut, TId> CreateTraveller(IState<TIn, TOut, TId> startState, RefStepSignature<TIn, TOut, TId> lastStep = null)
        {
            return new FSMTraveller<TIn, TOut, TId>(startState, lastStep);
        }

        /// <summary>
        /// Метод, создающий запрос с входным символом <paramref name="input"/>.
        /// </summary>
        /// <param name="input">Входной символ.</param>
        /// <returns>Запрос к путешественнику.</returns>
        private static StepQuery<TIn> CreateQuery(ISymbol<TIn> input)
        {
            return new StepQuery<TIn>(input);
        }
        #endregion
        #endregion
    }
}
