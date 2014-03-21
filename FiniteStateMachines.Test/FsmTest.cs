using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FiniteStateMachines.Core;
using FiniteStateMachines.Processing;
using FiniteStateMachines.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiniteStateMachines;

namespace FiniteStateMachines.Test
{
    [TestClass]
    public class FsmTest
    {
        
        [TestMethod]
        public void ConstructorTest()
        {
            var fsm = new NFA<int, int,int>(new NumberGenerator());
            Assert.AreEqual(0,fsm.TotalStates);
        }
        [TestMethod]
        public void AddStateTest()
        {
            var fsm = new NFA<int, int, int>(new NumberGenerator());
            var start = fsm.CreateNewState(StateType.StartState);
            Assert.AreEqual(1,fsm.StartStatesCount);
            var end = fsm.CreateNewState(StateType.EndState);
            Assert.AreEqual(1,fsm.EndStatesCount);
            var trans = fsm.CreateNewState(StateType.TransitionalState);
            Assert.AreEqual(1,fsm.TransitionalStatesCount);
        }
       /* [TestMethod]
        public void AddStepTest()
        {
            var fsm = new NFA<int, int, int>();
            var start = fsm.CreateNewState(StateType.StartState);
            var end = fsm.CreateNewState(StateType.EndState);
            var input = new Symbol<int>(2, SymbolType.Terminal);
            var output = new Symbol<int>(3, SymbolType.Terminal);
            fsm.AddStep(start,input,output,end);
            ISet<ISymbol<int>> outs;
            ISet<IState<int, int, int>> outst;
            Assert.IsTrue(start.StepResult(new Symbol<int>(2,SymbolType.Terminal), out outs,out outst));
        }*/
       /* [TestMethod]
        public void StepTest()
        {
            var fsm = new NFA<int, int, int>();
            var start = fsm.CreateNewState(StateType.StartState);
            var end = fsm.CreateNewState(StateType.EndState);
            var input = new Symbol<int>(2, SymbolType.Terminal);
            var output = new Symbol<int>(3, SymbolType.Terminal);
            fsm.AddStep(start, input, output, end);
            ISet<ISymbol<int>> outs;
            ISet<IState<int, int, int>> outst;
            start.StepResult(input, out outs, out outst);
            Assert.AreEqual(1,outs.Count);
            Assert.AreEqual(1,outst.Count);
            Assert.AreEqual(end,outst.First());
            Assert.AreEqual(output,outs.First());
        }*/
        [TestMethod]
        public void NullTest()
        {
            var fsm = new NFA<string, int, int>(new NumberGenerator());
            var start = fsm.CreateNewState(StateType.StartState);
            var end = fsm.CreateNewState(StateType.EndState);
            var input = new Symbol<string>();
            var output = new Symbol<int>(10, SymbolType.Terminal);
            fsm.AddStep(new IdStepSignature<string, int, int>(start, input, output, end));
            var result = fsm.MakeStep(new Symbol<string>());
            Assert.AreEqual(output,result.First());
        }
        [TestMethod]
        public void CurrentStateTest()
        {
            var fsm = new NFA<char, int, int>(new NumberGenerator());
            var start = fsm.CreateNewState(StateType.StartState);
            var end = fsm.CreateNewState(StateType.EndState);
            var input = new Symbol<char>('a', SymbolType.Terminal);
            var output = new Symbol<int>(10, SymbolType.Terminal);
            fsm.AddStep(new IdStepSignature<char, int, int>(start, input, output, end));
            Assert.AreEqual(false,fsm.AtFinish());
            var result = fsm.MakeStep(input);
            Assert.AreEqual(true,fsm.AtFinish());
        }

        [TestMethod]
        public void SimpleConvertationTest()
        {
            var nfa = new NFA<int, string, int>(new NumberGenerator());
            var start = nfa.CreateNewState(StateType.StartState);
            var transitional = nfa.CreateNewState(StateType.TransitionalState);
            var end = nfa.CreateNewState(StateType.EndState);
            var one = new Symbol<int>(1, SymbolType.Terminal);
            var zero = new Symbol<int>(0, SymbolType.Terminal);
            var oneMessage = new Symbol<string>("one",SymbolType.Terminal);
            var twoMessage = new Symbol<string>("two",SymbolType.Terminal);
            var threeMessage = new Symbol<string>("three",SymbolType.Terminal);
            nfa.AddStep(new IdStepSignature<int, string, int>(start,zero,oneMessage,start ));
            nfa.AddStep(new IdStepSignature<int, string, int>(start,one,oneMessage,start));
            nfa.AddStep(new IdStepSignature<int, string, int>(start,zero,twoMessage,transitional));
            nfa.AddStep(new IdStepSignature<int, string, int>(transitional,zero,threeMessage,end));

            var converter = new FSMConverter<int, string, int>(nfa,new NumberGenerator());
            
            converter.Convert();

            var dfa = converter.Dfa;


            var expectedFirstOutSymbol = new MultiSymbol<string>();
            expectedFirstOutSymbol.AddSymbol(oneMessage);
            var expectedSecondOutSymbol = new MultiSymbol<string>();
            expectedSecondOutSymbol.AddSymbol(oneMessage);
            expectedSecondOutSymbol.AddSymbol(twoMessage);
            var expectedThirdOutSymbol = new MultiSymbol<string>();
            expectedThirdOutSymbol.AddSymbol(oneMessage);
            expectedThirdOutSymbol.AddSymbol(twoMessage);
            expectedThirdOutSymbol.AddSymbol(threeMessage);

            var actualFirstOutSymbol = dfa.MakeStep(one);
            Assert.IsTrue(expectedFirstOutSymbol.Equals(actualFirstOutSymbol.First()));

            var actualSecondOutSymbol = dfa.MakeStep(zero);
            Assert.IsTrue(expectedSecondOutSymbol.Equals(actualSecondOutSymbol.First()));

            var actualThirdOutSymbol = dfa.MakeStep(zero);
            Assert.IsTrue(expectedThirdOutSymbol.Equals(actualThirdOutSymbol.First()));

            var actualForuthOutSymbol = dfa.MakeStep(zero);
            Assert.IsTrue(expectedThirdOutSymbol.Equals(actualForuthOutSymbol.First()));

            var actualFifthOutSymbol = dfa.MakeStep(one);
            Assert.IsTrue(expectedFirstOutSymbol.Equals(actualFifthOutSymbol.First()));
        }

    }
}
