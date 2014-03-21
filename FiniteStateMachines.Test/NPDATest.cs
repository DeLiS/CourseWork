using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiniteStateMachines.Core;
using FiniteStateMachines.Utility;
namespace FiniteStateMachines.Test
{
    [TestClass]
    public class NPDATest
    {
        [TestMethod]
        public void AddStepTest()
        {
            var generator = new NumberGenerator();
            var pda = new NPDA<int, char, int, int>(generator,generator);
            var start = pda.CreateNewState(StateType.StartState);
            var end = pda.CreateNewState(StateType.EndState);
            var tr1 = pda.CreateNewState(StateType.TransitionalState);
            var tr2 = pda.CreateNewState(StateType.TransitionalState);

            var i1 = new Symbol<int>(1,SymbolType.Terminal);
            var i2 = new Symbol<int>(2,SymbolType.Terminal);
            var i3 = new Symbol<int>(3,SymbolType.Terminal);
            var i4 = new Symbol<int>(4,SymbolType.Terminal);

            var o1 = new Symbol<char>('a', SymbolType.Terminal);
            var o2 = new Symbol<char>('b', SymbolType.Terminal);
            var o3 = new Symbol<char>('c', SymbolType.Terminal);
            var o4 = new Symbol<char>('d', SymbolType.Terminal);

            var s1 = new Symbol<int>(100, SymbolType.Terminal);
            var s2 = new Symbol<int>(200, SymbolType.Terminal);
            var s3 = new Symbol<int>(300, SymbolType.Terminal);
            var s4 = new Symbol<int>(400, SymbolType.Terminal);

            pda.AddStep(new IdPushDownStepSignature<int,char,int,int>(start,i1,o1,tr1,StackActions.Push,s1) );
            pda.AddStep(new IdPushDownStepSignature<int, char, int,int>(tr1, i2, s1, o2, tr2, StackActions.PopPush, s2));
            pda.AddStep(new IdPushDownStepSignature<int, char, int,int>(tr2, i3, s2, o3, end, StackActions.Pop, null));

            pda.Reset();

            var res1 = pda.MakeStep(i1);
            Assert.AreEqual(o1,res1.First());

            var res2 = pda.MakeStep(i2);
            Assert.AreEqual(o2,res2.First());

            var res3 = pda.MakeStep(i3);
            Assert.AreEqual(o3,res3.First());

            Assert.IsTrue(pda.AtFinish());
        }
    }
}

