using System;
using System.Collections.Generic;
using System.Linq;
using FiniteStateMachines.Utility;
using FiniteStateMachines.Interfaces;
namespace FiniteStateMachines.Core
{
    ///<remarks>
    ///</remarks>
    ///<typeparam name="TIn"></typeparam>
    ///<typeparam name="TOut"></typeparam>
    ///<typeparam name="TStack"></typeparam>
    ///<typeparam name="TId"></typeparam>
    public class NPDA<TIn,TOut,TStack,TId>:NFA<TIn,TOut,TId>
        where TIn : IEquatable<TIn>, IComparable<TIn> 
        where TOut : IEquatable<TOut>, IComparable<TOut>
        where TStack:IEquatable<TStack>,IComparable<TStack>
        where TId:IComparable<TId>,IEquatable<TId>
    {
    /*    ///<summary>
        ///</summary>
      //  public ISymbol<TStack> StackBottom { get; private set; }
        */
        public IGenerator<TStack> GeneratorTStack { get; protected set; }
        public NPDA(IGenerator<TId> generator,IGenerator<TStack> generatorTStack):base(generator)
        {
            GeneratorTStack = generatorTStack;
         //   StackBottom = new Symbol<TStack>(generatorTStack.GetUniqueId(),SymbolType.Terminal);
        }
        protected override RefStepSignature<TIn,TOut,TId> GetRefStepSignature(IdStepSignature<TIn,TOut,TId> sig)
        {
            var pdass = sig as IdPushDownStepSignature<TIn, TOut, TStack, TId>;
            if (pdass == null)
                throw new ApplicationException("Wrong signature");
            var start = GetStateById(sig.StartState);
            var end = GetStateById(sig.EndState);
            if(start == null)
                throw new ApplicationException("No such state: start");
            if(end == null)
                throw new ApplicationException("No such state: end");
            if (pdass.CheckStack)
                return new PushdownRefStepSignature<TIn, TOut, TStack, TId>(start, pdass.Input, pdass.Output, end,
                                                                       pdass.StackAction, pdass.ToPush,pdass.StackTop);
            return new PushdownRefStepSignature<TIn, TOut, TStack, TId>(start, pdass.Input, pdass.Output, end,
                                                                       pdass.StackAction, pdass.ToPush);
        }
        
        public override bool AtFinish()
        {
            foreach (var pdaTraveller in
                Travellers.Select(fsmTraveller => fsmTraveller as PDATraveller<TIn, TOut, TStack, TId>))
            {
                if(pdaTraveller == null)
                    throw new ApplicationException("wrong traveller type");
                if (pdaTraveller.CurrentState.IsEndState() && pdaTraveller.Memory.Count == 0)
                    return true;
            }
            return false;
        }

        public override void Reset()
        {
            Travellers.Clear();
            foreach (var startState in States)
            {

                if(IsStartState(startState))
                Travellers.Add(new PDATraveller<TIn, TOut, TStack, TId>(GetStateById(startState), new PDAStack<ISymbol<TStack>>()));
            }
            MakeStep(new Symbol<TIn>());
        }

        protected override IState<TIn, TOut, TId> CreateState(StateType stateType)
        {
            return new Core.PushDownState<TIn, TOut, TStack, TId> (stateType,GeneratorTId);
        }

        protected override FSMTraveller<TIn, TOut, TId> CreateTraveller(IState<TIn, TOut, TId> startState,RefStepSignature<TIn,TOut,TId> laststep=null)
        {
            var pdlaststep = laststep as PushdownRefStepSignature<TIn, TOut, TStack, TId>;
            if(laststep!=null&&pdlaststep==null)
                throw new ApplicationException("Wrong signature");
            /*var startMemory = new PDAStack<ISymbol<TStack>>();
            startMemory.Push(StackBottom);*/
            return new PDATraveller<TIn, TOut, TStack, TId>(startState, new PDAStack<ISymbol<TStack>>(),pdlaststep);
        }
    }
}
