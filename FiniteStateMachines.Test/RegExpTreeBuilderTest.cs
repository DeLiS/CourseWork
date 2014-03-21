using FiniteStateMachines.RegExps;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FiniteStateMachines.Test
{
    /// <summary>
    /// Summary description for RegExpBuilderTest
    /// </summary>
    [TestClass]
    public class RegExpTreeBuilderTest
    {
       

        [TestMethod]
        public void BuildTreeTest1()
        {
            const string regexp = "('a'|'b')";
            const string expectedResult = "a\n|\nb\n&\n#\n";

            var builder = new RegExpTreeBuilder(regexp);
            builder.BuildTree();

            string result = builder.PrintTree();

            Assert.AreEqual(expectedResult,result);
        }

        [TestMethod]
        public void BuildTreeTest2()
        {
            const string regexp = "('abc'*)";
            const string expectedResult = "abc\n*\n&\n#\n";

            var builder = new RegExpTreeBuilder(regexp);
            builder.BuildTree();

            string result = builder.PrintTree();

            Assert.AreEqual(expectedResult,result);

        }

        [TestMethod]
        public void BuildTreeTest3()
        {
            const string regexp = "('abc'&'dfe')";
            const string expectedResult = "abc\n&\ndfe\n&\n#\n";

            var builder = new RegExpTreeBuilder(regexp);
            builder.BuildTree();

            string result = builder.PrintTree();

            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void BuildTreeTest4()
        {
            const string regexp = "(('a'|('b'|('c'|'d')))&('e'|('f'*)))";
            const string expectedResult = "a\n|\nb\n|\nc\n|\nd\n&\ne\n|\nf\n*\n&\n#\n";

            var builder = new RegExpTreeBuilder(regexp);
            builder.BuildTree();

            string result = builder.PrintTree();

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ConcatenationTest()
        {
            const string regexp = "('a'&'b')";
            var builder = new RegExpTreeBuilder(regexp);
            builder.BuildTree();
            RegExpTreeBuilder.Calculate(builder.Root);
            var root = builder.Root;
        }
        [TestMethod]
        public void AlternativeTest()
        {
            const string regexp = "('a'|'b')";
            var builder = new RegExpTreeBuilder(regexp);
            builder.BuildTree();
            RegExpTreeBuilder.Calculate(builder.Root);
            var root = builder.Root;
        }
        [TestMethod]
        public void AsteriskTest()
        {
            const string regexp = "('a'*)";
            var builder = new RegExpTreeBuilder(regexp);
            builder.BuildTree();
            RegExpTreeBuilder.Calculate(builder.Root);
            var root = builder.Root;
        }
        [TestMethod]
        public void PlusTest()
        {
            const string regexp = "('a'+)";
            var builder = new RegExpTreeBuilder(regexp);
            builder.BuildTree();
            RegExpTreeBuilder.Calculate(builder.Root);
            var root = builder.Root;
        }
        [TestMethod]
        public void OptionTest()
        {
            const string regexp = "['a']";
            var builder = new RegExpTreeBuilder(regexp);
            builder.BuildTree();
            RegExpTreeBuilder.Calculate(builder.Root);
            var root = builder.Root;
        }

    }
}
