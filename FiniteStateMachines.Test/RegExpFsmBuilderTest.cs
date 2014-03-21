using FiniteStateMachines.RegExps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiniteStateMachines.Utility;
using FiniteStateMachines.Grammars;
namespace FiniteStateMachines.Test
{
    [TestClass]
    public class RegExpFsmBuilderTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            const string regexp = "(('a'|('b'|('c'|'d')))&('e'|('f'*)))";
            var builder = new RegExpFSMBuilder<string,int>(new NumberGenerator());
           // builder.Process(regexp);
        }
        [TestMethod]
        public void DFSM_BuilderTest()
        {
            const string regexp = "((((('a'|'b')*)&'a')&'b')&'b')";
            var generator = new NumberGenerator();
            var symbol = new Symbol<string>("smth",SymbolType.NonTerminal);
            var rule = new RegularGrammarRule<int>("smth",generator);
            rule.AddSequence(regexp);
            var grammar = new RegularGrammar<int>(generator);
            grammar.AddRule(rule);
            grammar.BuildAcceptorForEachRule(false);
            var result = grammar.Accepts("ababb");
            Assert.IsTrue(result.Count>0);


        }
    }
}
