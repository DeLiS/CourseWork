using System;
using System.Collections.Generic;
using System.Linq;
using FiniteStateMachines.Core;
using FiniteStateMachines.Utility;
using Graphs.lib.DataStructure;
using Graphs.lib.Algorithms;
using FiniteStateMachines.Processing;
using FiniteStateMachines.Interfaces;
namespace FiniteStateMachines.Grammars
{
    ///<summary>
    /// Регулярная грамматика
    ///</summary>
    public class RegularGrammar<TId>
        where TId : IComparable<TId>, IEquatable<TId>
    {
        private readonly SortedDictionary<string, RegularGrammarRule<TId>> _rules = new SortedDictionary<string, RegularGrammarRule<TId>>();
        private readonly Graph<string> _dependencyGraph = new Graph<string>();
        private string[] _sortedGraph;
        private bool[] _recursive;
        private bool _rulesBuilt = false;
        private bool _determinate = false;
        protected readonly IGenerator<TId> GeneratorId;
        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="generatorId">Генератор уникальных идентификаторов состояний.</param>
        public RegularGrammar(IGenerator<TId> generatorId)
        {
            GeneratorId = generatorId;
        }
        ///<summary>
        /// Автомат, распознающий все нетерминалы грамматики.
        ///</summary>
        public NFA<string, string, TId> GrammarAcceptor { get; private set; }
        ///<summary>
        /// Метод, добавляющий правило в грамматику.
        ///</summary>
        ///<param name="rule">Правило</param>
        public void AddRule(RegularGrammarRule<TId> rule)
        {
            _rules.Add(rule.DefinedSymbol.Value, rule);
        }
        ///<summary>
        /// Метод, который строит граф зависимостей между правилами, проводит его топологическую сортировку и находит рекурсивные зависимости.
        ///</summary>
        private void BuildGraph()
        {
            string someName = "";
            foreach (var regGrammarRule in _rules)
            {
                someName = regGrammarRule.Value.DefinedSymbol.Value;
                break;
            }
            if (someName == "")
                return;
            foreach (var regGrammarRule in _rules.Values)
            {
                _dependencyGraph.AddVertex(regGrammarRule.DefinedSymbol.Value);

            }
            foreach (var regGrammarRule in _rules.Values)
            {
                var dependencies = regGrammarRule.Dependencies;
                foreach (var dependency in dependencies)
                {
                    _dependencyGraph.Connect(dependency, regGrammarRule.DefinedSymbol.Value, new ConnectionArgs<string>(true));
                }
            }
            var topsort = new TopologicalSort<string>(_dependencyGraph, someName);
            topsort.Run();
            _sortedGraph = topsort.Result();
            _recursive = new bool[_sortedGraph.Length];
            for (int i = 1; i < _sortedGraph.Length; ++i)//O(N^2) TLE
                for (int j = 0; j < i; ++j)
                {
                    ConnectionArgs<string> args;
                    if (_dependencyGraph.AreConnected(_sortedGraph[i], _sortedGraph[j], out args))
                        _recursive[i] = true;
                }
        }

        ///<summary>
        /// Метод, строящий один общий автомат, проверяющий принадлежность слова сразу ко всем правилам грамматики. 
        /// Работа возможна только с недетерминированными автоматами (на самом деле, с любыми, не содержащими состояния типа StartEnd).
        ///</summary>
        public void BuildUnitedAutomaton()
        {
            if (!_determinate)
            {
                if (!_rulesBuilt)
                    BuildAcceptorForEachRule(false);
                GrammarAcceptor = _rules[_sortedGraph[0]].Acceptor;
                var list = _rules.Select(regGrammarRule => regGrammarRule.Value.Acceptor).ToList();

                var manipulator = CreateOperator(list);
                manipulator.Alternative2();
                GrammarAcceptor = manipulator.Result;
            }
        }

        ///<summary>
        /// Метод, который строит автомат, который может распознавать все нетерминалы грамматики.
        ///</summary>
        ///<param name="determinate"></param>
        public void BuildAcceptorForEachRule(bool determinate)
        {
            BuildGraph();
            BuildRules(determinate);
            _rulesBuilt = true;
        }

        ///<summary>
        /// Перечисление всех правил грамматики
        ///</summary>
        private IEnumerable<RegularGrammarRule<TId>> Rules
        {
            get
            {
                foreach (var grammarRule in _rules)
                {
                    yield return grammarRule.Value;
                }
            }
        }

        ///<summary>
        /// Метод, проверяющий, соответствует ли слово грамматике.
        ///</summary>
        ///<param name="word">Некоторое слово.</param>
        ///<returns>Множество нетерминальных символов, правилам вывода которых соответствует слово.</returns>
        public ISet<ISymbol<string>> Accepts(string word)
        {
            var result = new SortedSet<ISymbol<string>>();
            foreach (var grammarRule in Rules)
            {
                if(grammarRule.Accepts(word))
                {
                    result.Add(grammarRule.DefinedSymbol);
                }
            }
            return result;
        }

        ///<summary>
        /// Метод, выделяющий из текста найбольшие идущие подряд лексемы.
        ///</summary>
        ///<param name="text">Текст</param>
        ///<returns>Список пар "(начало лексемы, конеч лексемы), тип"</returns>
        ///<exception cref="ApplicationException">Если строка некорректна.</exception>
        public List<Pair<Pair<int, int>, Symbol<string>>> CheckText(string text)
        {
            var result = new List<Pair<Pair<int, int>, Symbol<string>>>(); //общий список подстрок и соответствующих им лексем
            foreach (var regularGrammarRule in _rules)
            {
                regularGrammarRule.Value.Acceptor.Reset();
            }
            //храним текущую крайнюю самую длинную распознанную подстроку и нетерминал, которому она соответствует
            var currentNonterminals = new List<Pair<Pair<int, int>, Symbol<string>>>(); 
            var start = 0; //первый символ текущей подстроки
            var lastReturn = -1;
            for (int i = 0; i < text.Length;++i )
            {
                var input = new Symbol<string>(text[i].ToString(), SymbolType.NonTerminal);
                var stillWorking = false; //истина, если есть хотя бы один автомат, который еще может совершать переходы
                var firstOutputInIteration = true; //истина, если текущий список пуст.
                foreach (var regularGrammarRule in _rules)
                {
                    regularGrammarRule.Value.Acceptor.MakeStep(input);
                    if (regularGrammarRule.Value.Acceptor.IsWorking)
                        stillWorking = true;
                    if(regularGrammarRule.Value.Acceptor.AtFinish()) //если мы на текущем шаге распознали что-то
                    {
                        if (firstOutputInIteration) //и это первая итерация
                        {
                            firstOutputInIteration = false; //то следующая навряд ли будет первой
                            currentNonterminals.Clear();//и эта подстрока точно длиннее предыдущей, предыдущую можно удалить
                        }
                        currentNonterminals.Add(
                                new Pair<Pair<int, int>, Symbol<string>>(
                                    new Pair<int, int>(start, i),
                                    new Symbol<string>(regularGrammarRule.Key, SymbolType.NonTerminal)
                                    )
                                ); //а новую подстроку добавить
                        
                    }
                }
                if(stillWorking&&(i!= text.Length-1)) //если еще кто-то работает, и мы не дошли до конца строки
                    continue; // то пусть работают
                //иначе (если никто уже не работает или строка закончилась)
                //откатываемся к символу, где заканчивается последняя наибольшая распознанная подстрока
                if(currentNonterminals.Count==0) //если такой нет
                    throw new ApplicationException("Wrong string"); //то строка содержит слова, не принадлежащие грамматике
                var lastSuccess = currentNonterminals[0].Key.Value; //последний символ последней наибольшей распознанной подстроки
                result.AddRange(currentNonterminals); //добавляем её в результат
                foreach (var regularGrammarRule in _rules) //т.к. у нас никто не работает, то надо всех сбросить в начальное состояние
                {
                    regularGrammarRule.Value.Acceptor.Reset();
                }
                if (lastReturn == lastSuccess)
                    throw new Exception("Error on symbol" + text.Substring(lastReturn, i - lastReturn));
                i = lastSuccess; //на следующей итерации текущим будет первый символ после распознанной подстроки
                lastReturn = lastSuccess;
                start = i + 1; // он же обязан быть началом новой подстроки, которую мы хотим распознать
            }
            return result;
        }

        ///<summary>
        /// Делает все автоматы, распознающие нетерминалы детерминированными.
        ///</summary>
        public void MakeAcceptorsDeterminate()
        {
            foreach (var grammarRule in Rules)
            {
                grammarRule.MakeAcceptorDeterminate();
            }
            _determinate = true;
        }

        ///<summary>
        /// Метод, который строит для каждого правила автомат, который распознает нетерминал этого правила. 
        /// При этом автомат не содержит переходов по нетерминальным символам (т.е. устраняются все зависимости между нетерминалами).
        ///</summary>
        ///<param name="determinate"></param>
        ///<exception cref="ApplicationException"></exception>
        private void BuildRules(bool determinate)
        {
            foreach (var regGrammarRule in _rules)
            {
                regGrammarRule.Value.BuildAcceptor(determinate);
            }
            for (int i = 0; i < _sortedGraph.Length; i++)
            {
                var currentNonTerminal = _sortedGraph[i];
                var currentRule = _rules[currentNonTerminal];
                var currentNonTerminalSymbol = currentRule.DefinedSymbol;
                var currentAcceptor = currentRule.Acceptor;
                if (_recursive[i])
                {
                    var fsmOperator = CreateOperator();
                    fsmOperator.RecursionRemover(currentNonTerminalSymbol, currentAcceptor, true);//note: не забыть поправить, true - правая рекурсия, добавить опции
                    currentAcceptor = fsmOperator.Result;
                }
                var incident = _dependencyGraph.AdjacentVertexes(currentNonTerminal);
                var beginEndsSet = new List<KeyValuePair<TId, TId>>();
                foreach (var dependentVertex in incident)
                {
                    beginEndsSet.Clear();
                    var dependent = dependentVertex.Vertex.Value;
                    var dependentAcceptor = _rules[dependent].Acceptor;
                    var idStepSignatures = dependentAcceptor.IdStepSignatures;
                    var toRemove = new List<IdStepSignature<string, string, TId>>();
                    foreach (var idStepSignature in idStepSignatures)
                    {
                        if (idStepSignature.Input.CompareTo(currentNonTerminalSymbol) == 0)
                        {
                            toRemove.Add(idStepSignature);
                            beginEndsSet.Add(new KeyValuePair<TId, TId>(idStepSignature.StartState, idStepSignature.EndState));
                        }
                    }
                    foreach (var idStepSignature in toRemove)
                    {
                        dependentAcceptor.RemoveStep(idStepSignature);
                    }
                    var manipulator = CreateOperator(dependentAcceptor, currentAcceptor);
                    manipulator.MultiInsertBetween(beginEndsSet);
                    _rules[dependent].Acceptor = manipulator.Result;

                }
                var converter2 = CreateConverter();
                converter2.MakeAcceptor(currentAcceptor, currentNonTerminalSymbol);
            }


        }

      

        #region Фабричные методы
        protected virtual FSMOperator<string, string, TId> CreateOperator(List<NFA<string, string, TId>> list)
        {
            return new FSMOperator<string, string, TId>(list, GeneratorId);
        }
        protected virtual FSMOperator<string, string, TId> CreateOperator(NFA<string, string, TId> first, NFA<string, string, TId> second)
        {
            return new FSMOperator<string, string, TId>(first, second, GeneratorId);
        }
        protected virtual FSMOperator<string, string, TId> CreateOperator()
        {
            return new FSMOperator<string, string, TId>(GeneratorId);
        }
        protected virtual FSMConverter<string, string, TId> CreateConverter()
        {
            return new FSMConverter<string, string, TId>(GeneratorId);
        }
        #endregion Фабричные методы
    }
}
