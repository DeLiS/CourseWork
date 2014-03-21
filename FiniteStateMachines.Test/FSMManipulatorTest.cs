using System.Linq;
using FiniteStateMachines.Core;
using FiniteStateMachines.Processing;
using FiniteStateMachines.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FiniteStateMachines.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class FSMManipulatorTest
    {
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void SimpleConcatenation()
        {
            var one = new Symbol<int>(1, SymbolType.Terminal);
            var two = new Symbol<int>(2, SymbolType.Terminal);
            var three = new Symbol<int>(3, SymbolType.Terminal);
            var four = new Symbol<int>(4, SymbolType.Terminal);
            var nfa1 = new NFA<int, int, int>(new NumberGenerator());
            var start1 = nfa1.CreateNewState(StateType.StartState);
            var end1 = nfa1.CreateNewState(StateType.EndState);
            nfa1.AddStep(new IdStepSignature<int, int, int>(start1,one,two, end1 ));


            NFA<int, int, int> nfa2 = new NFA<int, int, int>(new NumberGenerator());
            var start2 = nfa2.CreateNewState(StateType.StartState);
            var end2 = nfa2.CreateNewState(StateType.EndState);
            
            nfa2.AddStep(new IdStepSignature<int, int, int>(start2, three, four, end2));

            var manipulator = new FSMOperator<int, int, int>(nfa1, nfa2, new NumberGenerator());
            manipulator.Concatenate();
            var nfa3 = manipulator.Result;
            var result = nfa3.MakeStep(one);
            Assert.AreEqual(two,result.First());
            nfa3.MakeStep(new Symbol<int>(0, SymbolType.Empty));
            result =  nfa3.MakeStep(three);
            Assert.AreEqual(four,result.First());
        }
    }
}
