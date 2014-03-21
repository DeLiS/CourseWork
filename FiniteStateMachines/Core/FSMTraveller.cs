using System;
using System.Collections.Generic;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;

namespace FiniteStateMachines.Core
{
    ///<remarks>
    /// Класс, инкапсулирующий в себе информацию о передвижении по автомату.
    ///</remarks>
    ///<typeparam name="TIn">Тип входных символов.</typeparam>
    ///<typeparam name="TOut">Тип выходных символов.</typeparam>
    ///<typeparam name="TId">Тип идентификаторов состояний.</typeparam>
    public class FSMTraveller<TIn, TOut, TId> : IComparable<FSMTraveller<TIn, TOut, TId>>,
                                                IEquatable<FSMTraveller<TIn, TOut, TId>>
        where TIn : IComparable<TIn>, IEquatable<TIn>
        where TOut : IComparable<TOut>, IEquatable<TOut>
        where TId : IComparable<TId>, IEquatable<TId>
    {

        ///<summary>
        /// Текущее состояние, в котором находится объект класса.
        ///</summary>
        public IState<TIn, TOut, TId> CurrentState { get; protected set; }

        ///<summary>
        /// Возможные переходы.
        ///</summary>
        public ISet<RefStepSignature<TIn, TOut, TId>> AvailableSteps { get; protected set; }

        ///<summary>
        /// Сигнатура последнего перехода.
        ///</summary>
        public RefStepSignature<TIn, TOut, TId> LastStep { get; protected set; }

        protected FSMTraveller(){}

        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="startState">Начальное состояние.</param>
        ///<param name="lastStep">Сигнатура последнего перехода.</param>
        public FSMTraveller(IState<TIn, TOut, TId> startState, RefStepSignature<TIn,TOut,TId> lastStep=null)
        {
            CurrentState = startState;
            LastStep = lastStep;
        }

        ///<summary>
        /// Метод, определяющий возможные переходы по запросу <paramref name="stepQuery"/>.
        /// Результат помещается в переменную AvailableSteps.
        ///</summary>
        ///<param name="stepQuery">Входной запрос.</param>
        ///<returns>Истина, если есть хотя бы один переход.</returns>
        public virtual bool CalcAvailableSteps(StepQuery<TIn> stepQuery)
        {
            var result = CurrentState.GetStepResult(stepQuery);
            AvailableSteps = result;
            return AvailableSteps.Count > 0;
        }

        #region Implementation of IComparable<in FSMTraveller<TIn,TOut>>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual int CompareTo(FSMTraveller<TIn, TOut, TId> other)
        {
            return this.CurrentState.CompareTo(other.CurrentState);
        }

        #endregion

        #region Implementation of IEquatable<FSMTraveller<TIn,TOut>>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual bool Equals(FSMTraveller<TIn, TOut, TId> other)
        {
            return this.CompareTo(other) == 0;
        }

        #endregion

        ///<summary>
        /// Создать запрос с пустым входным символом для данного путешественника.
        ///</summary>
        ///<returns></returns>
        public virtual StepQuery<TIn> CreateEmptyQuery()
        {
            return new StepQuery<TIn>(new Symbol<TIn>());
        }
        ///<summary>
        /// Метод, создающий путешественника, который получится, если данный путешественник перейдет по переходу с сигнатурой <paramref name="refStepSignature"/>.
        ///</summary>
        ///<param name="refStepSignature">Сигнатура перехода.</param>
        ///<returns>Новый путешественник.</returns>
        ///<exception cref="ApplicationException"></exception>
        public virtual FSMTraveller<TIn, TOut, TId> CreateNewTraveller(RefStepSignature<TIn,TOut,TId> refStepSignature)
        {
            if(!this.CurrentState.Equals(refStepSignature.StartState))
                throw new ApplicationException("Start states are not equal");
            var traveller = new FSMTraveller<TIn, TOut, TId>(refStepSignature.TargetState,refStepSignature);
            return traveller;
        }
    }
}
