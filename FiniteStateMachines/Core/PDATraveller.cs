using System;
using System.Collections.Generic;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;

namespace FiniteStateMachines.Core
{
    /// <remarks>
    /// Класс, инкапсулирующий в себе информацию о передвижении по автомату с магазинной памятью.
    /// </remarks>
    /// <typeparam name="TIn">Тип входных символов.</typeparam>
    /// <typeparam name="TOut">Тип выходных символов</typeparam>
    /// <typeparam name="TStack">Тип символов магазинной памяти.</typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний автомата.</typeparam>
    public class PDATraveller<TIn, TOut, TStack, TId> : FSMTraveller<TIn, TOut, TId>
        where TIn : IComparable<TIn>, IEquatable<TIn>
        where TOut : IComparable<TOut>, IEquatable<TOut>
        where TStack : IComparable<TStack>, IEquatable<TStack>
        where TId : IComparable<TId>, IEquatable<TId>
    {
        ///<summary>
        /// Память автомата (магазин, стек)
        ///</summary>
        public PDAStack<ISymbol<TStack>> Memory { get; protected set; }

        ///<summary>
        /// Конструктор!
        ///</summary>
        ///<param name="startState">Начальное состояние путешественника.</param>
        ///<param name="startMemory">Начальное состояние памяти.</param>
        ///<param name="lastStep">Сигнатура последнего перехода.</param>
        public PDATraveller(IState<TIn, TOut, TId> startState, PDAStack<ISymbol<TStack>> startMemory,PushdownRefStepSignature<TIn,TOut,TStack,TId> lastStep=null):base(startState,lastStep)
        {
            Memory = startMemory;
        }

        /// <summary>
        /// Метод, сравнивающий путешественников и по состоянию, и по памяти.
        /// </summary>
        /// <param name="other">Другой путешественник.</param>
        /// <returns>-1 - если этот меньше другого;0 - если они равны; 1 - если этот больше другого.</returns>
        public override int CompareTo(FSMTraveller<TIn, TOut, TId> other)
        {
            var pdaTraveller = other as PDATraveller<TIn, TOut, TStack, TId>;
            if (pdaTraveller == null)
                throw new ApplicationException("Different travellers");
            var cmp = base.CompareTo(other);

            if (cmp != 0)
                return cmp;
            return Memory.CompareTo(pdaTraveller.Memory);
        }

        ///<summary>
        /// Метод, определяющий возможные переходы по запросу <paramref name="stepQuery"/>.
        /// Результат помещается в переменную AvailableSteps.
        ///</summary>
        ///<param name="stepQuery">Входной запрос.</param>
        ///<returns>Истина, если есть хотя бы один переход.</returns>
        public override bool CalcAvailableSteps(StepQuery<TIn> stepQuery)
        {
            var pushdownStepQuery = new PushdownStepQuery<TIn, TStack>(stepQuery.Input,
                                                                       Memory.Count > 0
                                                                           ? Memory.Peek()
                                                                           : new Symbol<TStack>());
            return base.CalcAvailableSteps(pushdownStepQuery);
         
        }
        protected virtual ISet<RefStepSignature<TIn, TOut, TId>> AddEpsilonTransitions(ISet<RefStepSignature<TIn, TOut, TId>> signatures, SortedSet<TId> used = null)
        {
            if (used == null)
                used = new SortedSet<TId>();

            var result = new SortedSet<RefStepSignature<TIn, TOut, TId>>();
            foreach (var refStepSignature in signatures)
            {
                if (!used.Contains(refStepSignature.TargetState.Id))
                {
                    used.Add(refStepSignature.TargetState.Id);
                    var tmp = refStepSignature.TargetState.GetStepResult(CreateEmptyQuery());
                    foreach (var stepSignature in tmp)
                    {

                        result.Add(stepSignature);
                    }
                }
            }
            ISet<RefStepSignature<TIn, TOut, TId>> closure = new SortedSet<RefStepSignature<TIn, TOut, TId>>();
            if (result.Count > 0)
                closure = AddEpsilonTransitions(result, used);
            result.UnionWith(closure);
            return result;
        }
        public override StepQuery<TIn> CreateEmptyQuery()
        {
            return new PushdownStepQuery<TIn, TStack>(new Symbol<TIn>(),Memory.Count>0?Memory.Peek():new Symbol<TStack>());
        }
        public override FSMTraveller<TIn, TOut, TId> CreateNewTraveller(RefStepSignature<TIn, TOut, TId> refStepSignature)
        {
            var signature = refStepSignature as PushdownRefStepSignature<TIn, TOut, TStack, TId>;
            if(signature == null)
                throw new ApplicationException("Wrong signature");
            var newMemory = new PDAStack<ISymbol<TStack>>(this.Memory);
            switch (signature.StackAction)
            {
                case StackActions.Pop:
                    newMemory.Pop();
                    break;
                case StackActions.Push:
                    newMemory.Push(signature.ToPush);
                    break;
                case StackActions.PopPush:
                    newMemory.Pop();
                    newMemory.Push(signature.ToPush);
                    break;
                case StackActions.Nothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new PDATraveller<TIn, TOut, TStack, TId>(refStepSignature.TargetState, newMemory,signature);
        }
    }
}
