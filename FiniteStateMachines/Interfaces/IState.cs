using System;
using System.Collections.Generic;
using FiniteStateMachines.Utility;

namespace FiniteStateMachines.Interfaces
{
    /// <summary>
    /// Интерфейс состояний автомата.
    /// </summary>
    /// <typeparam name="TIn">Тип входных символов.</typeparam>
    /// <typeparam name="TOut">Тип выходных символов.</typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний.</typeparam>
    public interface IState<TIn, TOut, TId> : IComparable<IState<TIn, TOut, TId>>, IEquatable<IState<TIn, TOut, TId>>
        where TIn : IEquatable<TIn>,IComparable<TIn>
        where TOut : IEquatable<TOut>,IComparable<TOut>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        ///<summary>
        /// Глобальный уникальный идентификатор состояния.
        ///</summary>
        TId Id { get; }
        ///<summary>
        /// Добавление перехода из данного состояния c сигнатурой <paramref name="signature"/>.
        ///</summary>
        ///<param name="signature">Ссылочная сигнатура перехода.</param>
        void AddStep(RefStepSignature<TIn, TOut, TId> signature);
        ///<summary>
        /// Удаление перехода из данного состояния.
        ///</summary>
        /// <param name="signature">Ссылочная сигнатура перехода.</param>
        void RemoveStep(RefStepSignature<TIn, TOut, TId> signature);

        ///<summary>
        /// Метод, проверяющий, является ли данное состояние начальным.
        ///</summary>
        ///<returns>Истина, если состояние начальное, ложь в противном случае</returns>
        bool IsStartState();

        ///<summary>
        /// Метод, проверяющий, является ли данное состояние конечным.
        ///</summary>
        ///<returns>Истина, если состояние конечное, ложь в противном случае.</returns>
        bool IsEndState();

        ///<summary>
        /// Получает результирующий символ и состояние при переходе из данного состояния по данному символу.
        ///</summary>
        ///<param name="query">Входной запрос.</param>
        ///<returns>Истина, если переход возможен, ложь в противном случае.</returns>
        ISet<RefStepSignature<TIn, TOut, TId>> GetStepResult(StepQuery<TIn> query);

        /// <summary>
        /// Метод, возвращающий смежные состояния данного состояния.
        /// </summary>
        /// <returns>Словарь "Входящий символ - Множество смежных состояний".</returns>
        SortedDictionary<ISymbol<TIn>, SortedSet<IState<TIn, TOut,TId>>> GetAdjacentStates();

        ///<summary>
        /// Метод возвращает множество выходных символов при переходе по данному запросу из данного состояния в состояние <paramref name="target"/>.
        ///</summary>
        ///<param name="query">Запрос для перехода.</param>
        ///<param name="target">Результирующее состояние.</param>
        ///<returns>Множество результирующих символов</returns>
        ISet<ISymbol<TOut>> GetOutSymbol(StepQuery<TIn> query, IState<TIn, TOut,TId> target);

        ///<summary>
        /// Перечисление сигнатур переходов, присутствующих в автомате.
        ///</summary>
        IEnumerable<IdStepSignature<TIn, TOut, TId>> AdjacentStates { get; }
    }
}
