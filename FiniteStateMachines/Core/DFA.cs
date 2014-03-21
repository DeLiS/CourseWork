using System;
using System.Collections.Generic;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;

namespace FiniteStateMachines.Core
{
    /// <summary>
    /// Детерминированный конечный автомат.
    /// </summary>
    /// <typeparam name="TIn">Тип входящих символов.</typeparam>
    /// <typeparam name="TOut">Тип результирующих символов.</typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний.</typeparam>
    public class DFA<TIn,TOut,TId>:NFA<TIn,TOut,TId>
        where TIn:IComparable<TIn>,IEquatable<TIn>
        where TOut:IComparable<TOut>,IEquatable<TOut>
        where TId:IComparable<TId>,IEquatable<TId>
    {

        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="generator">Генератор идентификаторов состояний.</param>
        public DFA(IGenerator<TId> generator):base(generator){}
        /// <summary>
        /// Метод, добавляющий переход в автомате.
        /// </summary>
        /// <param name="idStepSignature">Сигнатура перехода.</param>
        public override void AddStep(IdStepSignature<TIn, TOut, TId> idStepSignature)
        {
            IState<TIn, TOut, TId> startState = GetStateById(idStepSignature.StartState);
            IState<TIn, TOut, TId> endState = GetStateById(idStepSignature.EndState);

            if (startState == null)
                throw new FsmException<TIn,TOut,TId>(FsmExceptionId.NoStateWithSuchGuid, idStepSignature.StartState);
            if (endState == null)
                throw new FsmException<TIn, TOut, TId>(FsmExceptionId.NoStateWithSuchGuid, idStepSignature.EndState);

            if(idStepSignature.Input == null)
                throw new FsmException<TIn, TOut, TId>(FsmExceptionId.InputSymbolIsNull);
            if (idStepSignature.Output == null)
                throw new FsmException<TIn, TOut, TId>(FsmExceptionId.OutputSymbolIsNull);

            if (idStepSignature.Input.Type == SymbolType.Empty)
                throw new FsmException<TIn, TOut, TId>(FsmExceptionId.InputSymbolIsEmpty);

            var query = new StepQuery<TIn>(idStepSignature.Input);

            if (startState.GetStepResult(query).Count>0) // если из данного состояния уже есть переход по данному символу
                throw new FsmException<TIn,TOut,TId>(FsmExceptionId.SameInput,idStepSignature.Input,"Second transition");
            
            var refSignature = new RefStepSignature<TIn, TOut, TId>(startState,idStepSignature.Input,idStepSignature.Output,endState);
            startState.AddStep(refSignature);
        }
        /// <summary>
        /// Метод, создающий в автомате новое состояние.
        /// </summary>
        /// <param name="stateType">Тип создаваемого состояния.</param>
        /// <returns>Идентификатор созданного состояния.</returns>
        public override TId CreateNewState(StateType stateType)
        {
            if(stateType == StateType.StartState && base.StartStatesCount > 0)
                throw new ApplicationException("DFA: CreateNewState: Can't have more than one start state");
            return base.CreateNewState(stateType);
        }

        /// <summary>
        /// Метод, моделирующий переход автомата по входному символу.
        /// </summary>
        /// <param name="input">Входной символ.</param>
        /// <returns>Множество выходных символов.</returns>
        public override ISet<ISymbol<TOut>> MakeStep(ISymbol<TIn> input)
        {
            if(input == null)
                throw new ApplicationException("DFA:MakeStep:input is null");
            
            if(input.Type == SymbolType.Empty)
                throw new ApplicationException("DFA:MakeStep:input is empty");
            var result =  base.MakeStep(input);

            if(result.Count > 1)
                throw new ApplicationException("DFA:MakeStep: not determenstic automaton");

            return result;
        }

        public override void Reset()
        {
            Travellers.Clear();
            foreach (var startState in _startStates)
            {
                Travellers.Add(CreateTraveller(startState));
            }
            foreach (var startEndState in _startEndStates)
            {
                Travellers.Add(CreateTraveller(startEndState));
            }
        }
    }
}
