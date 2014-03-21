using System;
using System.Collections.Generic;
using System.Linq;
using FiniteStateMachines.Core;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Processing;
using FiniteStateMachines.Utility;
using System.Threading;
namespace FiniteStateMachines.RegExps
{
    /// <remarks>
    /// Класс, строящий конечный автомат по дереву регулярного выражения.
    /// </remarks>
    /// <typeparam name="TOut">Тип выходного символа автомата.</typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний автомата.</typeparam>
    public class RegExpFSMBuilder<TOut, TId>
        where TOut : IComparable<TOut>, IEquatable<TOut>
        where TId : IComparable<TId>, IEquatable<TId>
    {
        ///<summary>
        /// Дерево разбора регулярного выражения
        ///</summary>
        private RegExpTree<ISymbol<string>> Tree { get; set; }

        private TreeNode<ISymbol<string>> Root { get; set; }

        private NFA<string, TOut, TId> _nfa;

        protected readonly IGenerator<TId> _generatorTId;

        private SortedDictionary<TId, SortedSet<TreeNode<ISymbol<string>>>> _dictionary = new SortedDictionary<TId, SortedSet<TreeNode<ISymbol<string>>>>();

        ///<summary>
        /// Недетерминированный конечный автомат, построенный по регулярному выражению.
        ///</summary>
        public NFA<string, TOut, TId> NFSM { get { return _nfa; } }

        protected RegExpFSMBuilder() { }
        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="generatorTId">Генератор уникальных идентификаторов состояний автомата.</param>
        public RegExpFSMBuilder(IGenerator<TId> generatorTId)
        {
            _generatorTId = generatorTId;
        }


        ///<summary>
        /// Метод, строящий дерево выражения и недетерминированный автомат по нему.
        ///</summary>
        ///<param name="regexp">Регулярное выражение.</param>
        public void Process(String regexp)
        {
            var treeBuilder = new RegExpTreeBuilder(regexp);
            treeBuilder.BuildTree();
            RegExpTreeBuilder.Calculate(treeBuilder.Root);
          //  Tree = treeBuilder.Tree;
            Root = treeBuilder.Root;

            BuildDFA();
          //  BuildNfa();
        }

        ///<summary>
        /// Метод, создающий недетерминированный автомат по регулярному выражению.
        ///</summary>
        ///<param name="regexp">Регулярное выражение.</param>
        public void BUildNfaAcceptor(String regexp)
        {
            var treeBuilder = new RegExpTreeBuilder(regexp);
            treeBuilder.BuildTree();
            Root = treeBuilder.Root;
            BuildNfa();
        }
        ///<summary>
        /// Метод, создающий детерминированный автомат по регулярному выражению.
        ///</summary>
        ///<param name="regexp"></param>
        public void BuildDfaAcceptor(String regexp)
        {
            var treeBuilder = new RegExpTreeBuilder(regexp);
            treeBuilder.BuildTree();
            RegExpTreeBuilder.Calculate(treeBuilder.Root);
            Root = treeBuilder.Root;
            BuildDFA();
        }

        /// <summary>
        /// Метод строящий автомат по дереву.
        /// </summary>
        private void BuildNfa()
        {
           // Tree.Reset();
            // _nfa = BuildSubtree(Direction.Left);
            BuildSubtree(Root.Left, out _nfa);
        }

        /// <summary>
        /// Строит автомат поддерева дерева регулярного выражения.
        /// </summary>
        /// <param name="root"> Корень дерева регулярного выражения.</param>
        /// <param name="nfa"> Результирующий автомат.</param>
        /// <returns>Автомат, распознающий язык, задаваемый поддеревом дерева регулярного выражения</returns>
        private void BuildSubtree(TreeNode<ISymbol<string>> root, out NFA<string, TOut, TId> nfa)
        {
            switch (root.Type)
            {
                case NodeType.Operation:
                    NFA<string, TOut, TId> left, right;
                    FSMOperator<string, TOut, TId> manipulator;
                    switch (root.Operation)
                    {
                        case OperationType.Concatenation:
                            BuildSubtree(root.Left, out left);
                            BuildSubtree(root.Right, out right);
                            manipulator = GetOperator(left, right);
                            manipulator.Concatenate();
                            break;
                        case OperationType.Alternative:
                            BuildSubtree(root.Left, out left);
                            BuildSubtree(root.Right, out right);
                            manipulator = GetOperator(left, right);
                            manipulator.Alternative();
                            break;
                        case OperationType.Asterisk:
                            BuildSubtree(root.Left, out left);
                            manipulator = GetOperator(left, null);
                            manipulator.Asterisk();
                            break;
                        case OperationType.Option:
                            BuildSubtree(root.Left, out left);
                            manipulator = GetOperator(left, null);
                            manipulator.Option();
                            break;
                        case OperationType.Plus:
                            BuildSubtree(root.Left, out left);
                            manipulator = GetOperator(left, null);
                            manipulator.Plus();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    nfa = manipulator.Result;
                    break;
                case NodeType.Terminal:
                case NodeType.NonTerminal:
                    nfa = CreateStateMatchine();
                    var start = nfa.CreateNewState(StateType.StartState);
                    var end = nfa.CreateNewState(StateType.EndState);
                    nfa.AddStep(EmptyOutputStepSignature(start, root.Symbol, end));
                    return;
                default:
                    throw new ArgumentOutOfRangeException("BuildNFA2 Problem");
            }
        }

        /// <summary>
        /// Метод, строящий детерминированный автомат по выражению.
        /// </summary>
        private void BuildDFA()
        {
            _nfa = CreateDeterminateStateMatchine();
            var startEnd = Root.FirstPos.Any(treenode => treenode.Symbol.Type == SymbolType.Border);
            var rootId = _nfa.CreateNewState(startEnd?StateType.StartEndState:StateType.StartState);
            _dictionary[rootId] = Root.FirstPos;
            var queue = new Queue<TId>();
            queue.Enqueue(rootId);
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                var set = _dictionary[cur];
                var union = new SortedDictionary<ISymbol<string>, SortedSet<TreeNode<ISymbol<string>>>>();
                foreach (var treeNode in set)
                {
                    var followPoses = treeNode.FollowPos;
                    if (!union.ContainsKey(treeNode.Symbol))
                    {
                        union[treeNode.Symbol] = new SortedSet<TreeNode<ISymbol<string>>>();
                    }
                    union[treeNode.Symbol].UnionWith(followPoses);

                    /*SortedSet<TreeNode<ISymbol<string>>> treeNodeSet;
                    if(union.TryGetValue(treeNode.Symbol,out treeNodeSet))
                    {
                        treeNodeSet.UnionWith(followPoses);
                    }
                    else
                    {
                        union[treeNode.Symbol] = new SortedSet<TreeNode<ISymbol<string>>>();
                        union[treeNode.Symbol].UnionWith(followPoses);
                    }*/
                }

                foreach (var symbolSet in union)
                {
                    if (symbolSet.Key.Type != SymbolType.Border)
                    {
                        TId id = default(TId);
                        bool flag = true;
                        foreach (var keyValue in _dictionary)
                        {
                            if (AreEqual(keyValue.Value, symbolSet.Value))
                            {
                                id = keyValue.Key;
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            var nodes = symbolSet.Value;
                            bool isEndState = nodes.Any(treeNode => treeNode.Symbol.Type == SymbolType.Border);
                            id = _nfa.CreateNewState(isEndState ? StateType.EndState : StateType.TransitionalState);
                            _dictionary[id] = symbolSet.Value;
                            queue.Enqueue(id);
                        }

                        _nfa.AddStep(EmptyOutputStepSignature(cur, symbolSet.Key, id));
                    }
                }
            }
        }

        protected virtual NFA<string, TOut, TId> CreateDeterminateStateMatchine()
        {
            return new DFA<string, TOut, TId>(_generatorTId);
        }

        #region Фабричные методы
        protected virtual FSMOperator<string, TOut, TId> GetOperator(NFA<string, TOut, TId> left, NFA<string, TOut, TId> right)
        {
            return new FSMOperator<string, TOut, TId>(left, right, _generatorTId);
        }
        protected virtual NFA<string, TOut, TId> CreateStateMatchine()
        {
            return new NFA<string, TOut, TId>(_generatorTId);
        }
        protected virtual IdStepSignature<string, TOut, TId> EmptyOutputStepSignature(TId start, ISymbol<string> input, TId end)
        {
            return new IdStepSignature<string, TOut, TId>(start, input, new Symbol<TOut>(), end);
        }
        private static bool AreEqual(SortedSet<TreeNode<ISymbol<string>>> first, SortedSet<TreeNode<ISymbol<string >>> second)
        {
            return first.IsSubsetOf(second) && first.IsSupersetOf(second);
        }
        #endregion Фабричные методы
    }
}
