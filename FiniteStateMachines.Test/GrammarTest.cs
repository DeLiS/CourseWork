using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FiniteStateMachines.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiniteStateMachines.Grammars;
using FiniteStateMachines.Processing;
namespace FiniteStateMachines.Test
{
    [TestClass]
    public class GrammarTest
    {
        [TestMethod]
        public void SimpleCheck1()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new Symbol<string>(Digit, SymbolType.NonTerminal);
            string def = "('0'|'1')";
            var rule = new RegularGrammarRule<int>(Digit, generator);
            rule.AddSequence(def);
            rule.BuildAcceptor(false);
            var converter = new FSMConverter<string, string, int>(rule.Acceptor, generator);
         //   converter.MakeAcceptor(rule.Acceptor,digitSymbol);
            var result = Checker<int>.StaticCheck(rule, "0");
            Assert.IsTrue(digitSymbol.Equals(result));
        }
        [TestMethod]
        public void SimpleCheck2()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new FiniteStateMachines.Utility.Symbol<string>(Digit, SymbolType.NonTerminal);
            string def = "[('0'|'1')]";
            var rule = new RegularGrammarRule<int>(Digit, generator);
            rule.AddSequence(def);
            rule.BuildAcceptor(false);
            var result = Checker<int>.StaticCheck(rule, "0");
            Assert.IsTrue(digitSymbol.Equals(result));

            result = Checker<int>.StaticCheck(rule, "1");
            Assert.IsTrue(digitSymbol.Equals(result));

            result = Checker<int>.StaticCheck(rule, "3");
            Assert.IsFalse(digitSymbol.Equals(result));
        }
        [TestMethod]
        public void SimpleCheck3()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new FiniteStateMachines.Utility.Symbol<string>(Digit, SymbolType.NonTerminal);
            string def = "(('0'|'1')+)";
            var rule = new RegularGrammarRule<int>(Digit, generator);
            rule.AddSequence(def);
            rule.BuildAcceptor(false);
            var result = Checker<int>.StaticCheck(rule, "0101011100001101010101010101001011");
            Assert.IsTrue(digitSymbol.Equals(result));
        }
        [TestMethod]
        public void SimpleCheck4()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new Symbol<string>(Digit, SymbolType.NonTerminal);
            string def = "('1'&(('0'|'1')+))";
            var rule = new RegularGrammarRule<int>(Digit, generator);
            rule.AddSequence(def);
            rule.BuildAcceptor(false);
            var result = Checker<int>.StaticCheck(rule, "10100");
            Assert.IsTrue(digitSymbol.Equals(result));
        }
        [TestMethod]
        public void SimpleCheck5()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new Symbol<string>(Digit, SymbolType.NonTerminal);
            //string def = "(('0'|'1')+)";
            string def2 = "(('2'|'3')*)";
           // string def3 = "((('0'|'1')+)|(('2'|'3')*))";
            var rule = new RegularGrammarRule<int>(Digit, generator);
           // rule.AddSequence(def);
            rule.AddSequence(def2);
            //rule.AddSequence(def3);
            rule.BuildAcceptor(false);
            var result = Checker<int>.StaticCheck(rule, "111000101010001100111010");
           // Assert.IsTrue(digitSymbol.Equals(result));
            result = Checker<int>.StaticCheck(rule, "332223332322232232");
            Assert.IsTrue(digitSymbol.Equals(result));
            result = Checker<int>.StaticCheck(rule, "12");
            Assert.IsFalse(digitSymbol.Equals(result));
        }
        [TestMethod]
        public void SelfRecursionCheck()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new Symbol<string>(Digit, SymbolType.NonTerminal);
            string def = "(('0'|'1')&[<Digit>])";
            var rule = new RegularGrammarRule<int>(Digit, generator);
            rule.AddSequence(def);
            rule.BuildAcceptor(false);
            var result = Checker<int>.StaticCheck(rule, "1");
            Assert.IsTrue(digitSymbol.Equals(result));
            result = Checker<int>.StaticCheck(rule, "01");
            Assert.IsTrue(digitSymbol.Equals(result));
            result = Checker<int>.StaticCheck(rule, "111000101010001100111010");
            Assert.IsTrue(digitSymbol.Equals(result));
        }
        [TestMethod]
        public void GrammarTest1()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new Symbol<string>(Digit, SymbolType.NonTerminal);
            var digitRule = new RegularGrammarRule<int>(Digit, generator);
            for(int i=0;i<=0;++i)
            {
                digitRule.AddSequence(String.Format("'{0}'",i));
            }
            string Letter = "Letter";
            var letterSymbol = new Symbol<string>(Letter, SymbolType.NonTerminal);
            var letterRule = new RegularGrammarRule<int>(Letter, generator);
            for(char i='a';i<='a';++i)
                letterRule.AddSequence(String.Format("'{0}'",i));
            var Identificator = "Identificator";
            var identificatorSymbol = new Symbol<string>(Identificator,SymbolType.NonTerminal);
            var identificatorRule = new RegularGrammarRule<int>(Identificator,generator);
            identificatorRule.AddSequence( "(<Letter>&((<Letter>|<Digit>)*))" );
            var grammar = new RegularGrammar<int>(new NumberGenerator());
            grammar.AddRule(digitRule);
            grammar.AddRule(letterRule);
            grammar.AddRule(identificatorRule);
            grammar.BuildAcceptorForEachRule(false);
            grammar.BuildUnitedAutomaton();
            var acceptor = grammar.GrammarAcceptor;
            var checker = new Checker<int>(acceptor);
            var result = checker.Check("a");
            if (result.Count == 0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var simpleSymbol = symbol as Symbol<string>;
                if(simpleSymbol==null)
                    Assert.Fail();
                Assert.IsTrue(simpleSymbol.Value.Equals(Letter) || simpleSymbol.Value.Equals(Identificator));
            }
        }
        [TestMethod]
        public void GrammarTest2()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new Symbol<string>(Digit, SymbolType.NonTerminal);
            var digitRule = new RegularGrammarRule<int>(Digit, generator);
            for (int i = 0; i <= 9; ++i)
            {
                digitRule.AddSequence(String.Format("'{0}'", i));
            }
            string Letter = "Letter";
            var letterSymbol = new Symbol<string>(Letter, SymbolType.NonTerminal);
            var letterRule = new RegularGrammarRule<int>(Letter, generator);
            for (char i = 'a'; i <= 'z'; ++i)
                letterRule.AddSequence(String.Format("'{0}'", i));
            var Identificator = "Identificator";
            var identificatorSymbol = new Symbol<string>(Identificator, SymbolType.NonTerminal);
            var identificatorRule = new RegularGrammarRule<int>(Identificator, generator);
            identificatorRule.AddSequence("(<Letter>&((<Letter>|<Digit>)*))");
            var grammar = new RegularGrammar<int>(new NumberGenerator());
            grammar.AddRule(digitRule);
            grammar.AddRule(letterRule);
            grammar.AddRule(identificatorRule);
            grammar.BuildAcceptorForEachRule(false);
            grammar.BuildUnitedAutomaton();
            var acceptor = grammar.GrammarAcceptor;
            var checker = new Checker<int>(acceptor);
            
            var result = checker.Check("a1234178jahfiubakjabg165t92bhjvq76g28grby1v");
            if(result.Count==0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var simpleSymbol = symbol as Symbol<string>;
                if (simpleSymbol == null)
                    Assert.Fail();
                Assert.IsTrue(simpleSymbol.Value.Equals(Letter) || simpleSymbol.Value.Equals(Identificator));
            }
            result = checker.Check("1a1234178jahfiubakjabg165t92bhjvq76g28grby1v");
            Assert.AreEqual(0,result.Count);
        }
        [TestMethod]
        public void GrammarTest3()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new Symbol<string>(Digit, SymbolType.NonTerminal);
            var digitRule = new RegularGrammarRule<int>(Digit,generator);
            for (int i = 0; i <= 9; ++i)
            {
                digitRule.AddSequence(String.Format("'{0}'", i));
            }

            string NonZeroDigit = "NonZeroDigit";
            var NonZerodigitSymbol = new Symbol<string>(NonZeroDigit, SymbolType.NonTerminal);
            var NonZerodigitRule = new RegularGrammarRule<int>(NonZeroDigit, generator);
            for (int i = 1; i <= 9; ++i)
            {
                NonZerodigitRule.AddSequence(String.Format("'{0}'", i));
            }

            string Letter = "Letter";
            var letterSymbol = new Symbol<string>(Letter, SymbolType.NonTerminal);
            var letterRule = new RegularGrammarRule<int>(Letter, generator);
            for (char i = 'a'; i <= 'z'; ++i)
                letterRule.AddSequence(String.Format("'{0}'", i));
            
            var Identificator = "Identificator";
            var identificatorSymbol = new Symbol<string>(Identificator, SymbolType.NonTerminal);
            var identificatorRule = new RegularGrammarRule<int>(Identificator, generator);
            identificatorRule.AddSequence("(<Letter>&((<Letter>|<Digit>)*))");

            var word = "Word";
            var wordSymbol = new Symbol<string>(word, SymbolType.NonTerminal);
            var wordRule = new RegularGrammarRule<int>(word, generator);
            wordRule.AddSequence("(<Letter>+)");

            var number = "Number";
            var numberSymbol = new Symbol<string>(number, SymbolType.NonTerminal);
            var numberRule = new RegularGrammarRule<int>(number, generator);
            numberRule.AddSequence("(<NonZeroDigit>&(<Digit>*))");
            numberRule.AddSequence("<Digit>");

            var grammar = new RegularGrammar<int>(new NumberGenerator());
            grammar.AddRule(digitRule);
            grammar.AddRule(letterRule);
            grammar.AddRule(identificatorRule);
            grammar.AddRule(wordRule);
            grammar.AddRule(NonZerodigitRule);
            grammar.AddRule(numberRule);
            grammar.BuildAcceptorForEachRule(false);
            grammar.BuildUnitedAutomaton();
            var acceptor = grammar.GrammarAcceptor;
            var checker = new Checker<int>(acceptor);

            var result = checker.Check("12634689471");
            if (result.Count == 0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var simpleSymbol = symbol as Symbol<string>;
                if (simpleSymbol == null)
                    Assert.Fail();
                Assert.IsTrue(simpleSymbol.Value.Equals(number) || simpleSymbol.Value.Equals(Digit) ||
                              simpleSymbol.Value.Equals(NonZeroDigit));
            }
            result = checker.Check("012634689471");
           Assert.IsTrue(result.Count == 0);



        }
        [TestMethod]
        public void GrammarTest4()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new Symbol<string>(Digit, SymbolType.NonTerminal);
            var digitRule = new RegularGrammarRule<int>(Digit, generator);
            for (int i = 0; i <= 9; ++i)
            {
                digitRule.AddSequence(String.Format("'{0}'", i));
            }
            string Number = "Number";
            var numberSymbol = new Symbol<string>(Number,SymbolType.NonTerminal);
            var numberRule = new RegularGrammarRule<int>(Number, generator);
            numberRule.AddSequence("(<Digit>+)");
            var grammar = new RegularGrammar<int>(new NumberGenerator());
            grammar.AddRule(digitRule);
            grammar.AddRule(numberRule);
            grammar.BuildAcceptorForEachRule(false);
            grammar.BuildUnitedAutomaton();
            var checker = new Checker<int>(grammar.GrammarAcceptor);
            var result = checker.Check("1");
            foreach (var symbol in result)
            {
                var simpleSymbol = symbol as Symbol<string>;
                if (simpleSymbol == null)
                    Assert.Fail();
                Assert.IsTrue(simpleSymbol.Value.Equals(Digit) || simpleSymbol.Value.Equals(Digit) || simpleSymbol.Value.Equals(Number));
            }
        }
        [TestMethod]
        public void GrammarTest5()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new Symbol<string>(Digit, SymbolType.NonTerminal);
            var digitRule = new RegularGrammarRule<int>(Digit, generator);
            for (int i = 0; i <= 1; ++i)
            {
                digitRule.AddSequence(String.Format("'{0}'", i));
            }

            string NonZeroDigit = "NonZeroDigit";
            var NonZerodigitSymbol = new Symbol<string>(NonZeroDigit, SymbolType.NonTerminal);
            var NonZerodigitRule = new RegularGrammarRule<int>(NonZeroDigit, generator);
            for (int i = 1; i <= 1; ++i)
            {
                NonZerodigitRule.AddSequence(String.Format("'{0}'", i));
            }
            var number = "Number";
            var numberSymbol = new Symbol<string>(number, SymbolType.NonTerminal);
            var numberRule = new RegularGrammarRule<int>(number, generator);
            numberRule.AddSequence("(<NonZeroDigit>&(<Digit>*))");
            numberRule.AddSequence("<Digit>");

            var grammar = new RegularGrammar<int>(new NumberGenerator());
            grammar.AddRule(digitRule);
            grammar.AddRule(NonZerodigitRule);
            grammar.AddRule(numberRule);
            grammar.BuildAcceptorForEachRule(false);
            grammar.BuildUnitedAutomaton();
            var result = Checker<int>.StaticCheck(numberRule, "01");
            Assert.IsTrue(result.Type==SymbolType.Empty);
        }
        [TestMethod]
        public void GrammarTest6()
        {
            var generator = new NumberGenerator();
            string Digit = "Digit";
            var digitSymbol = new Symbol<string>(Digit, SymbolType.NonTerminal);
            var digitRule = new ContextFreeGrammarRule<Guid,int>(Digit,new GuidGenerator(),new NumberGenerator());
            digitRule.AddSequence("('0'|<NonZeroDigit>)");

            string NonZeroDigit = "NonZeroDigit";
            var NonZerodigitSymbol = new Symbol<string>(NonZeroDigit, SymbolType.NonTerminal);
            var NonZerodigitRule = new ContextFreeGrammarRule<Guid,int>(NonZeroDigit,new GuidGenerator(),new NumberGenerator());
            for (int i = 1; i <= 1; ++i)
            {
                NonZerodigitRule.AddSequence(String.Format("'{0}'", i));
            }
            var number = "Number";
            var numberSymbol = new Symbol<string>(number, SymbolType.NonTerminal);
            var numberRule = new ContextFreeGrammarRule<Guid,int>(number,new GuidGenerator(),new NumberGenerator());
            numberRule.AddSequence("(<NonZeroDigit>&(<Digit>*))");
            numberRule.AddSequence("<Digit>");

            var grammar = new ContextFreeGrammar<Guid,int>(new GuidGenerator(),new NumberGenerator());
            grammar.AddRule(digitRule);
            grammar.AddRule(NonZerodigitRule);
            grammar.AddRule(numberRule);
            grammar.BuildAcceptorForEachRule(false);
            grammar.BuildUnitedAutomaton();
            var result = Checker<int>.StaticCheck(digitRule, "0");
            Assert.AreEqual((result as Symbol<String>).Value,"Digit");

            result = Checker<int>.StaticCheck(digitRule, "1");
            Assert.AreEqual((result as Symbol<String>).Value, "Digit");

            result = Checker<int>.StaticCheck(NonZerodigitRule, "1");
            Assert.AreEqual((result as Symbol<String>).Value, "NonZeroDigit");

            result = Checker<int>.StaticCheck(numberRule, "111001");
            Assert.AreEqual((result as Symbol<String>).Value, "Number");

            result = Checker<int>.StaticCheck(numberRule, "01");
            Assert.AreEqual((result as Symbol<String>).Type,SymbolType.Empty);
        }
        [TestMethod]
        public void GrammarTest7()
        {
            var generator = new NumberGenerator();
            string Zero = "Zero";
            RegularGrammarRule<int> ZeroRule = new RegularGrammarRule<int>(Zero, generator);
            ZeroRule.AddSequence("'0'");
            string One = "One";
            RegularGrammarRule<int> OneRule = new RegularGrammarRule<int>(One, generator);
            OneRule.AddSequence("('1'|'0')");
            string Number = "Number";
            RegularGrammarRule<int> NumberRule = new RegularGrammarRule<int>(Number, generator);
            //NumberRule.AddSequence("((<Zero>&(<One>*))|<One>)");
           // NumberRule.AddSequence("((<Zero>&(('1'|'0')*))|<One>)");
            NumberRule.AddSequence("(<Zero>&(<One>*))");
            NumberRule.AddSequence("<One>");
            var grammar = new RegularGrammar<int>(generator);
            grammar.AddRule(ZeroRule);
            grammar.AddRule(OneRule);
            grammar.AddRule(NumberRule);
            grammar.BuildAcceptorForEachRule(false);
            grammar.BuildUnitedAutomaton();
            var checker = new Checker<int>(grammar.GrammarAcceptor);
            var result = checker.Check("0");
            foreach (var symbol in result)
            {
                var s = symbol as Symbol<string>;
                Assert.IsTrue(Number.Equals(s.Value) || Zero.Equals(s.Value) || One.Equals(s.Value));
            }
        }

        [TestMethod]
        public void ContexFreeGrammarTest1()
        {
            var generator2 = new NumberGenerator();
            var generator = new GuidGenerator();
            string Zero = "Zero";
            var zeroRule = new ContextFreeGrammarRule<Guid,int>(Zero,generator,generator2);
            zeroRule.AddSequence("'0'");
            string One = "One";
            var oneRule = new ContextFreeGrammarRule<Guid, int>(One, generator, generator2);
            oneRule.AddSequence("'1'");

            string Palindrom = "Palindrom";
            var palindromRule = new ContextFreeGrammarRule<Guid, int>(Palindrom, generator, generator2);
            palindromRule.AddSequence("''");
            palindromRule.AddSequence("'1'");
            palindromRule.AddSequence("'0'");
            palindromRule.AddSequence("('1'&(<Palindrom>&'1'))");
            palindromRule.AddSequence("('0'&(<Palindrom>&'0'))");
           var grammar = new ContextFreeGrammar<Guid, int>(generator,new NumberGenerator());
            grammar.AddRule(oneRule);
            grammar.AddRule(zeroRule);
            grammar.AddRule(palindromRule);
            grammar.BuildAcceptorForEachRule(false);
            grammar.BuildUnitedAutomaton();
            var checker = new Checker<int>(grammar.GrammarAcceptor);
            var result = checker.Check("0");
            if (result.Count == 0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var s = symbol as Symbol<string>;
                Assert.IsTrue(Zero.Equals(s.Value)||Palindrom.Equals(s.Value));
            }
           
            result = checker.Check("1");
            if (result.Count == 0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var s = symbol as Symbol<string>;
                Assert.IsTrue(One.Equals(s.Value) || Palindrom.Equals(s.Value));
            }
            result = checker.Check("010");
            if(result.Count==0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var s = symbol as Symbol<string>;
                Assert.IsTrue(Palindrom.Equals(s.Value));
            }
            result = checker.Check("01001010010");
            if (result.Count == 0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var s = symbol as Symbol<string>;
                Assert.IsTrue(Palindrom.Equals(s.Value));
            }
            result = checker.Check("1001");
            if (result.Count == 0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var s = symbol as Symbol<string>;
                Assert.IsTrue(Palindrom.Equals(s.Value));
            }
            result = checker.Check("101001");
            if (result.Count != 0)
                Assert.Fail();

            result = checker.Check("10100101");
            if (result.Count == 0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var s = symbol as Symbol<string>;
                Assert.IsTrue(Palindrom.Equals(s.Value));
            }
            result = checker.Check("");
            if (result.Count == 0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var s = symbol as Symbol<string>;
                Assert.IsTrue(Palindrom.Equals(s.Value));
            }

            result = checker.Check("100");
            if (result.Count != 0)
                Assert.Fail();
           
        }
        [TestMethod]
        public void ContexFreeGrammarTest2()
        {
            var generator2 = new NumberGenerator();
            var generator = new GuidGenerator();
           string Palindrom = "Palindrom";
            var palindromRule = new ContextFreeGrammarRule<Guid,int>(Palindrom, generator,generator2);
            palindromRule.AddSequence("'1'");
            palindromRule.AddSequence("('2'&(<Palindrom>&'3'))");
            var grammar = new ContextFreeGrammar<Guid,int>(new GuidGenerator(),new NumberGenerator());
            grammar.AddRule(palindromRule);
            grammar.BuildAcceptorForEachRule(false);
            grammar.BuildUnitedAutomaton();
            var checker = new Checker<int>(grammar.GrammarAcceptor);
          
           
            var result = checker.Check("213");
            if (result.Count == 0)
                Assert.Fail();
            foreach (var symbol in result)
            {
                var s = symbol as Symbol<string>;
                Assert.IsTrue(Palindrom.Equals(s.Value));
            }
        }
    }
}
