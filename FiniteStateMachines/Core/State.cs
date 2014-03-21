using System;
using System.Collections.Generic;
using System.Linq;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;
using System.Diagnostics;
namespace FiniteStateMachines.Core
{
    /// <summary>
    /// Состояние конечного автомата.
    /// </summary>
    /// <typeparam name="TIn">Тип входных символов.</typeparam>
    /// <typeparam name="TOut">Тип выходных символов.</typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний.</typeparam>
    [DebuggerDisplay("{Id}")]
    public class State<TIn, TOut, TId> : IState<TIn, TOut,TId>
        where TIn : IEquatable<TIn>,IComparable<TIn>
        where TOut : IEquatable<TOut>,IComparable<TOut>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        private readonly IGenerator<TId> _generator;
        private readonly TId _id;
        private readonly StateType _stateType = StateType.TransitionalState;
        protected readonly List<AdjacentState<TIn, TOut, TId>> AdjacentList = new List<AdjacentState<TIn, TOut, TId>>();

        protected State(){}

        ///<summary>
        /// Конструктор по умолчанию, состояние помечается как промежуточное.
        ///</summary>
        private State(IGenerator<TId> generator)
        {
            _generator = generator;
            _id = _generator.GetUniqueId();
        }

        ///<summary>
        /// Конструктор, создающий состояние типа <paramref name="type"/>.
        ///</summary>
        ///<param name="type">Тип состояния (начальное, конечное, промежуточное)</param>
        ///<param name="generator">Генератор идентификаторво состояний.</param>
        public State(StateType type,IGenerator<TId> generator):this(generator)
        {
            _stateType = type;
        }

        ///<summary>
        /// Глобальный уникальный идентификатор состояния.
        ///</summary>
        public TId Id
        {
            get { return _id; }
        }

        ///<summary>
        /// Добавление перехода из данного состояния c сигнатурой <paramref name="signature"/>.
        ///</summary>
        ///<param name="signature">Ссылочная сигнатура перехода.</param>
        public virtual void AddStep(RefStepSignature<TIn, TOut, TId> signature)
        {
            var adjacentState = new AdjacentState<TIn, TOut, TId>(signature);
            AdjacentList.Add(adjacentState);
        }

        ///<summary>
        /// Удаление перехода из данного состояния.
        ///</summary>
        /// <param name="signature">Ссылочная сигнатура перехода.</param>
        public virtual void RemoveStep(RefStepSignature<TIn, TOut, TId> signature)
        {
            AdjacentList.RemoveAll(adjacentState =>
                                            adjacentState.Input.Equals(signature.InputSymbol)
                                            && adjacentState.Output.Equals(signature.OutputSymbol)
                                            && adjacentState.TargetState.Equals(signature.TargetState)
                                        );
        }

        ///<summary>
        /// Метод, проверяющий, является ли данное состояние начальным.
        ///</summary>
        ///<returns>Истина, если состояние начальное, ложь в противном случае</returns>
        public virtual bool IsStartState()
        {
            return _stateType == StateType.StartState || _stateType == StateType.StartEndState;
        }

        ///<summary>
        /// Метод, проверяющий, является ли данное состояние конечным.
        ///</summary>
        ///<returns>Истина, если состояние конечное, ложь в противном случае.</returns>
        public virtual bool IsEndState()
        {
            return _stateType == StateType.EndState || _stateType == StateType.StartEndState;
        }

        ///<summary>
        /// Получает результирующий символ и состояние при переходе из данного состояния по данному символу.
        ///</summary>
        ///<param name="query">Входной запрос.</param>
        ///<returns>Истина, если переход возможен, ложь в противном случае.</returns>
        public virtual ISet<RefStepSignature<TIn, TOut, TId>> GetStepResult(StepQuery<TIn> query)
        {
            var result = new SortedSet<RefStepSignature<TIn, TOut,TId>>();
            var input = query.Input;

            foreach (var adjacentState in AdjacentList)
            {
                if(adjacentState.Input.Equals(input))
                {
                    result.Add(new RefStepSignature<TIn, TOut,TId>(this, input, adjacentState.Output, adjacentState.TargetState));
                }
            }
          
            return result;
        }

        /// <summary>
        /// Метод, возвращающий смежные состояния данного состояния.
        /// </summary>
        /// <returns>Словарь "Входящий символ - Множество смежных состояний".</returns>
        public SortedDictionary<ISymbol<TIn>, SortedSet<IState<TIn, TOut, TId>>> GetAdjacentStates()
        {
            var dictionary = new SortedDictionary<ISymbol<TIn>, SortedSet<IState<TIn, TOut,TId>>>();

           

            foreach (var adjacentState in AdjacentList)
            {
                if(!dictionary.ContainsKey(adjacentState.Input))
                    dictionary.Add(adjacentState.Input,new SortedSet<IState<TIn, TOut,TId>>());
                dictionary[adjacentState.Input].Add(adjacentState.TargetState);
            }
            return dictionary;
        }

        ///<summary>
        /// Метод возвращает множество выходных символов при переходе по данному запросу из данного состояния в состояние <paramref name="target"/>.
        ///</summary>
        ///<param name="query">Запрос для перехода.</param>
        ///<param name="target">Результирующее состояние.</param>
        ///<returns>Множество результирующих символов</returns>
        public ISet<ISymbol<TOut>> GetOutSymbol(StepQuery<TIn> query, IState<TIn, TOut, TId> target)
        {
            var symbol = query.Input;
            var result = new SortedSet<ISymbol<TOut>>();
            foreach (var adjacentState in AdjacentList)
            {
                if(adjacentState.Input.Equals(symbol) && adjacentState.TargetState.Equals(target))
                {
                    result.Add(adjacentState.Output);
                }
            }
            return result;
        }

        public int CompareTo(IState<TIn, TOut, TId> other)
        {
            return Id.CompareTo(other.Id);
        }

        public bool Equals(IState<TIn, TOut, TId> other)
        {
            return Id.Equals(other.Id);
        }

        ///<summary>
        /// Перечисление сигнатур переходов, присутствующих в автомате.
        ///</summary>
        public virtual IEnumerable<IdStepSignature<TIn, TOut, TId>> AdjacentStates
        {
            get
            {
                return AdjacentList.Select(adjacentState => new IdStepSignature<TIn, TOut, TId>(Id, adjacentState.Input, adjacentState.Output,
                                                                                            adjacentState.TargetState.Id));
            }
        }
    }
}
