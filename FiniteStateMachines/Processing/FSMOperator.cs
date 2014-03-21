using System;
using System.Collections.Generic;
using FiniteStateMachines.Core;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;

namespace FiniteStateMachines.Processing
{
    /// <remarks>
    /// Класс, инкапсулирующий операции над автоматами.
    /// </remarks>
    /// <typeparam name="TIn">Тип входных символов.</typeparam>
    /// <typeparam name="TOut">Тип выходных символов.</typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний автоматов.</typeparam>
    public class FSMOperator<TIn, TOut, TId>
        where TIn : IEquatable<TIn>, IComparable<TIn>
        where TOut : IEquatable<TOut>, IComparable<TOut>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        protected enum Modes
        {
            Concatenation,
            Alternative,
            Asterisk,
            Insertion,
            Option,
            Plus
        } ;

        protected Modes Mode;
        private readonly NFA<TIn, TOut, TId> _first;
        private readonly NFA<TIn, TOut, TId> _second;
        private readonly List<NFA<TIn, TOut, TId>> _automatons;
        ///<summary>
        /// Результирующий автомат.
        ///</summary>
        public NFA<TIn, TOut, TId> Result { get; protected set; }

        private readonly SortedSet<TId> _oldFirstStartStates;
        private readonly SortedSet<TId> _oldSecondStartStates;
        private readonly SortedSet<TId> _oldFirstEndStates = new SortedSet<TId>();
        private readonly SortedSet<TId> _oldSecondEndStates = new SortedSet<TId>();
        private readonly List<SortedSet<TId>> _oldStartStates = new List<SortedSet<TId>>();
        private readonly List<SortedSet<TId>> _oldEndStates = new List<SortedSet<TId>>();
        protected readonly IGenerator<TId> GeneratorId;

        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="first">Первый операнд.</param>
        ///<param name="second">Второй операнд.</param>
        ///<param name="generatorId">Генератор идентификаторов состояний автоматов.</param>
        public FSMOperator(NFA<TIn, TOut, TId> first, NFA<TIn, TOut, TId> second,IGenerator<TId> generatorId)
        {
            GeneratorId = generatorId;
            _first = first;
            _second = second;
            if (first == null)
                throw new ApplicationException("FSN Manipulator First is null");
            _oldFirstStartStates = _first.GetStartStates();
            _oldFirstEndStates = _first.GetEndStates();
            if (second != null)
            {
                _oldSecondStartStates = _second.GetStartStates();
                _oldSecondEndStates = _second.GetEndStates();
            }
        }

        ///<summary>
        /// Конструктор, принимающий список автоматов. (Для альтернативы).
        ///</summary>
        ///<param name="automatons">Список автоматов.</param>
        ///<param name="generatorId">Генератор уникальных идентификаторов состояний автомата.</param>
        public FSMOperator(List<NFA<TIn, TOut, TId>> automatons,IGenerator<TId> generatorId)
        {
            GeneratorId = generatorId;
            _automatons = automatons;
            for (int i = 0; i < _automatons.Count; ++i)
            {
                _oldStartStates.Add(_automatons[i].GetStartStates());
                _oldEndStates.Add(_automatons[i].GetEndStates());
            }
        }

        protected FSMOperator(){}
        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="generatorId">Генератор состояний автомата.</param>
        public FSMOperator(IGenerator<TId> generatorId)
        {
            GeneratorId = generatorId;
        }

        private TId GetNewId(TId oldTId, Dictionary<TId, TId> oldNewTId, NFA<TIn,TOut,TId> source,bool first=true)
        {
            TId newState;
            TId current = oldTId;
            if (oldNewTId.TryGetValue(current, out newState))
                return newState;
            var type = source.GetStateType(current);
            switch (type)
            {
                case StateType.StartState:
                    switch (Mode)
                    {
                        case Modes.Concatenation:
                            newState = Result.CreateNewState(first ? StateType.StartState : StateType.TransitionalState);
                            break;
                        case Modes.Alternative:
                            newState = Result.CreateNewState(StateType.StartState);
                            break;
                        case Modes.Asterisk:
                        case Modes.Option:
                        case Modes.Plus:
                            newState = Result.CreateNewState(StateType.StartState);
                            break;
                        case Modes.Insertion:
                            newState = Result.CreateNewState(first ? StateType.StartState : StateType.TransitionalState);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case StateType.EndState:
                    switch (Mode)
                    {
                        case Modes.Concatenation:
                            newState = Result.CreateNewState(first ? StateType.TransitionalState : StateType.EndState);
                            break;
                        case Modes.Alternative:
                            newState = Result.CreateNewState(StateType.EndState);
                            break;
                        case Modes.Asterisk:
                        case Modes.Option:
                        case Modes.Plus:
                            newState = Result.CreateNewState(StateType.EndState);
                            break;
                        case Modes.Insertion:
                            newState = Result.CreateNewState(first? StateType.EndState:StateType.TransitionalState);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case StateType.TransitionalState:
                    switch (Mode)
                    {
                        case Modes.Concatenation:
                        case Modes.Alternative:
                        case Modes.Asterisk:
                        case Modes.Insertion:
                        case Modes.Option:
                        case Modes.Plus:
                            newState = Result.CreateNewState(StateType.TransitionalState);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case StateType.StartEndState:
                    switch (Mode)
                    {
                        case Modes.Concatenation:
                            newState = Result.CreateNewState(first ? StateType.StartState : StateType.EndState);
                            break;
                        case Modes.Alternative:
                            newState = Result.CreateNewState(StateType.StartEndState);
                            break;
                        case Modes.Asterisk:
                        case Modes.Option:
                        case Modes.Plus:
                            newState = Result.CreateNewState(StateType.StartEndState);
                            break;
                        case Modes.Insertion:
                            newState = Result.CreateNewState(first ? StateType.StartEndState : StateType.TransitionalState);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            oldNewTId[current] = newState;
            return newState;
        }

        /// <summary>
        /// Результирующий автомат - конкатенация первого и второго (A&B).
        /// </summary>
        public void Concatenate()
        {
            Mode = Modes.Concatenation;
            Result = CreateStateMachine();
            var firstOldNewTId = InsertCopy(_first, true);
            var secondOldNewTId = InsertCopy(_second, false);
           
           
            foreach (var oldFirstEndState in _oldFirstEndStates)
            {
                foreach (var oldSecondStartState in _oldSecondStartStates)
                {
                    AddEmptyStep(firstOldNewTId[oldFirstEndState], secondOldNewTId[oldSecondStartState]);
                }
            }
        }

        ///<summary>
        /// Результирующий автомат - альтернатива первого и второго (язык - объединение языков) (A|B).
        ///</summary>
        public void Alternative()
        {
            Mode = Modes.Alternative;
            Result = CreateStateMachine();
            InsertCopy(_first, true);
            InsertCopy(_second, false);
        }

        ///<summary>
        /// Альтернатива, работающая со списком автоматов. (A1|A2|...|An).
        ///</summary>
        public void Alternative2()
        {
            Mode = Modes.Alternative;
            Result = CreateStateMachine();
            foreach (var automaton in _automatons)
            {
                InsertCopy(automaton, true);
              
            }
        }

        ///<summary>
        /// Результирующий автомат - замыкание первого (А*).
        ///</summary>
        public void Asterisk()
        {
            Mode = Modes.Asterisk;
            Result = CreateStateMachine();
            var oldNewTId = InsertCopy(_first, true);

            foreach (var oldFirstStartState in _oldFirstStartStates)
            {
                foreach (var oldFirstEndState in _oldFirstEndStates)
                {
                    AddEmptyStep(oldNewTId[oldFirstStartState], oldNewTId[oldFirstEndState]);
                    AddEmptyStep(oldNewTId[oldFirstEndState], oldNewTId[oldFirstStartState]);
                }
            }
        }
       
        ///<summary>
        /// Результирующий автомат - это первый автомат со вторым, вставленным между состояниями из множества пар <paramref name="beginEnds"/>.
        ///</summary>
        ///<param name="beginEnds">Множество пар состояний, между которыми в первом автомате нужно вставить второй.</param>
        public void MultiInsertBetween(IEnumerable<KeyValuePair<TId, TId>> beginEnds)
        {
            Mode = Modes.Insertion;
            Result = CreateStateMachine();
            var firstOldNewTId = InsertCopy(_first, true);
            foreach (var keyValuePair in beginEnds)
            {
                var oldNewTId = InsertCopy(_second, false);
                foreach (var oldSecondStartState in _oldSecondStartStates)
                {
                    var begin = keyValuePair.Key;
                    AddEmptyStep(firstOldNewTId[begin], oldNewTId[oldSecondStartState]);
                }

                foreach (var oldSecondEndState in _oldSecondEndStates)
                {
                    var end = keyValuePair.Value;
                    AddEmptyStep(oldNewTId[oldSecondEndState], firstOldNewTId[end]);
                }
            }
        }

        ///<summary>
        /// Результирующий автомат добавляет в язык первого автомата пустую строку ( [A] ).
        ///</summary>
        public void Option()
        {
            Mode = Modes.Option;
            Result = CreateStateMachine();
            var oldNewTId = InsertCopy(_first, true);
            foreach (var oldFirstStartState in _oldFirstStartStates)
            {
                foreach (var oldFirstEndState in _oldFirstEndStates)
                {
                    AddEmptyStep(oldNewTId[oldFirstStartState], oldNewTId[oldFirstEndState]);
                }
            }
        }

        ///<summary>
        /// Операция + (А+)
        ///</summary>
        public void Plus()
        {
            Mode = Modes.Plus;
            Result = CreateStateMachine();
            var oldNewTId = InsertCopy(_first, true);

            foreach (var oldFirstStartState in _oldFirstStartStates)
            {
                foreach (var oldFirstEndState in _oldFirstEndStates)
                {
                    AddEmptyStep(oldNewTId[oldFirstEndState], oldNewTId[oldFirstStartState]);
                }
            }
        }
        
        /// <summary>
        /// Метод, создающий в результирующем автомате копию <paramref name="source"/>.
        /// </summary>
        /// <param name="source">Источник копирования.</param>
        /// <param name="first">Первый ли это автомат?(Вообще появилось для конкатенации, 
        /// но успешно используется и в других местах.</param>
        private Dictionary<TId, TId> InsertCopy(NFA<TIn, TOut, TId> source, bool first)
        {
            var oldNewTId = new Dictionary<TId, TId>();
            var idStepSignatures = source.IdStepSignatures;

            var states = source.States;
            foreach (var state in states)
            {
                GetNewId(state, oldNewTId, source, first);
            }

            foreach (var idStepSignature in idStepSignatures)
            {
                var newStartId = GetNewId(idStepSignature.StartState, oldNewTId,source,first);
                var newEndId = GetNewId(idStepSignature.EndState, oldNewTId,source,first);
                var newIdStepSignature = idStepSignature.Clone() as IdStepSignature<TIn,TOut,TId>;
                if(newIdStepSignature==null) throw new ApplicationException("Wrong signature");
                newIdStepSignature.StartState = newStartId;
                newIdStepSignature.EndState = newEndId;
                Result.AddStep(newIdStepSignature);
            }
            return oldNewTId;
        }

        /// <summary>
        /// Метод, разрешающий проблему правой (или левой) рекурсии.
        /// </summary>
        /// <param name="nonterminal">Нетерминал, который распознает автомат.</param>
        /// <param name="acceptor">Автомат, распознающий нетерминал.</param>
        /// <param name="right">Если истина, то метод удаляет правую рекурсию, иначе - левую.</param>
        public virtual void RecursionRemover(ISymbol<TIn> nonterminal, NFA<TIn, TOut, TId> acceptor, bool right)
        {
            Result = acceptor;
            var startStates = Result.GetStartStates();
            var endStates = Result.GetEndStates();

            var idStepSignature = Result.IdStepSignatures;
            var toRemove = new SortedSet<IdStepSignature<TIn, TOut, TId>>();
            var toAdd = new SortedSet<IdStepSignature<TIn, TOut, TId>>();
            foreach (var stepSignature in idStepSignature)
            {
                if (!stepSignature.Input.Equals(nonterminal))
                    continue;
                toRemove.Add(stepSignature);
                if (right)
                {
                    foreach (var startState in startStates)
                    {
                        var newStepSignature = stepSignature.Clone() as IdStepSignature<TIn, TOut, TId>;
                        if (newStepSignature == null) throw new ApplicationException("Wrong step signature");
                        newStepSignature.EndState = startState;
                        toAdd.Add(newStepSignature);
                    }
                }
                else
                {
                    foreach (var endState in endStates)
                    {
                        var newStepSignature = stepSignature.Clone() as IdStepSignature<TIn, TOut, TId>;
                        if (newStepSignature == null) throw new ApplicationException("Wrong step signature");
                        newStepSignature.StartState = endState;
                        toAdd.Add(newStepSignature);
                    }
                }
               

            }

            foreach (var signature in toRemove)
            {
                Result.RemoveStep(signature);
            }
            foreach (var signature in toAdd)
            {
                AddEmptyStep(signature.StartState, signature.EndState);
            }
        }

        #region Фабричные методы... ну или типа того
        protected virtual void AddEmptyStep(TId start, TId end)
        {
            Result.AddStep(new IdStepSignature<TIn, TOut, TId>(start, new Symbol<TIn>(default(TIn), SymbolType.Empty),
                                                          new Symbol<TOut>(default(TOut), SymbolType.Empty), end));
        }

        protected virtual NFA<TIn,TOut,TId> CreateStateMachine()
        {
            return new NFA<TIn, TOut, TId>(GeneratorId);
        }

        #endregion
    }
}
