using System;
using System.Collections.Generic;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;
using FiniteStateMachines.Core;
using FiniteStateMachines.RegExps;
using FiniteStateMachines.Processing;
namespace FiniteStateMachines.Grammars
{
    ///<summary>
    /// Класс, содержащий в себе правила вывода некоторого нетерминального символа;
    /// умеющий строить автомат-распознаватель по имеющимся правилам
    ///</summary>
    public class RegularGrammarRule<TId> : IComparable<RegularGrammarRule<TId>>, IEquatable<RegularGrammarRule<TId>>, IComparable<string>
        where TId : IComparable<TId>, IEquatable<TId>
    {

        protected readonly IGenerator<TId> GeneratorId;
        ///<summary>
        /// Нетерминальный символ
        ///</summary>
        public Symbol<string> DefinedSymbol { get; private set; }
        ///<summary>
        /// Правила вывода нетерминала
        ///</summary>
        public ISet<string> Sequences { get; private set; }

        ///<summary>
        /// Множество нетерминалов, от которых зависит данный нетерминал
        ///</summary>
        public ISet<string> Dependencies { get; private set; }

        ///<summary>
        /// Автомат, распознающий нетерминал
        ///</summary>
        public NFA<string, string, TId> Acceptor { get; set; }

        protected RegularGrammarRule() { }

        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="symbol">Имя нетерминала, правила вывода которого хранит объект класса.</param>
        ///<param name="generatorId">Генератор уникальных идентификаторов состояний.</param>
        public RegularGrammarRule(string symbol, IGenerator<TId> generatorId)
        {
            DefinedSymbol = new Symbol<string>(symbol, SymbolType.NonTerminal);
            Sequences = new SortedSet<string>();
            Dependencies = new SortedSet<string>();
            GeneratorId = generatorId;
        }
        ///<summary>
        /// Добавление правила вывода.
        ///</summary>
        ///<param name="sequence">Правило вывода.</param>
        public void AddSequence(string sequence)
        {
            string tmp = "";
            for (int i = 0; i < sequence.Length; ++i)
            {
                if(sequence[i]=='\\'&&sequence[i+1]=='<')
                {
                    i++;
                    continue;
                }
                if (sequence[i] == '<')
                {
                    ++i;
                    for (; sequence[i] != '>'; ++i)
                        tmp += sequence[i];
                    Dependencies.Add(tmp);
                    tmp = "";
                }
            }
            Sequences.Add(sequence);
        }

        ///<summary>
        /// Метод, строящий автомат-распознаватель по множеству правил вывода данного нетерминала.
        ///</summary>
        ///<param name="determinate"></param>
        public void BuildAcceptor(bool determinate)
        {
            var list = new List<NFA<string, string, TId>>();
            foreach (var sequence in Sequences)
            {
                var builder = GetFSMBuilder();
                if(determinate)
                    builder.BuildDfaAcceptor(sequence);
                else
                {
                    builder.BUildNfaAcceptor(sequence);
                }
                var nfa = builder.NFSM;
                list.Add(nfa);
            }

            var manupulator = CreateOperator(list);
            manupulator.Alternative2();
            Acceptor = manupulator.Result;
            var recursionRemover = CreateOperator();
            recursionRemover.RecursionRemover(DefinedSymbol, Acceptor, true);
            Acceptor = recursionRemover.Result;
            var converter2 = CreateConverter();
            converter2.MakeAcceptor(Acceptor, DefinedSymbol);
        }
        ///<summary>
        /// Метод, проверяющий, принимает ли правило данное слово.
        ///</summary>
        ///<param name="word">Слово.</param>
        ///<returns>Истина, если слово удовлетворяет правило;иначе - ложь.</returns>
        public bool Accepts(string word)
        {
            Acceptor.Reset();
            for(int i=0;i<word.Length;++i)
            {
                Acceptor.MakeStep(new Symbol<string>(word[i].ToString(), SymbolType.NonTerminal));
            }
            return Acceptor.AtFinish();
        }

        ///<summary>
        /// Делает из распознавателя детерминированный автомат.
        ///</summary>
        public void MakeAcceptorDeterminate()
        {
            var converter = new FSMConverter<string, string, TId>(Acceptor, GeneratorId);
            converter.Convert();
            Acceptor = converter.Dfa;
            converter.MakeAcceptor(Acceptor,DefinedSymbol);
        }

        ///<summary>
        /// Метод, собирающий все правила вывода в одно
        ///</summary>
        public void AgregateRules()
        {
            bool first = true;
            string rule="";
            foreach (var sequence in Sequences)
            {
                if (first)
                {
                    first = false;
                    rule = sequence;
                }
                else
                {
                    rule = AppendRule(rule, sequence);
                }
            }
            Sequences.Clear();
            Sequences.Add(rule);
        }
        private static string AppendRule(string rule1, string rule2)
        {
            return String.Format("({0}|{1})", rule1, rule2);
        }

        #region Фабричные методы
        protected virtual FSMConverter<string, string, TId> CreateConverter()
        {
            return new FSMConverter<string, string, TId>(GeneratorId);
        }
        protected virtual FSMOperator<string, string, TId> CreateOperator(List<NFA<string, string, TId>> list)
        {
            return new FSMOperator<string, string, TId>(list, GeneratorId);
        }
        protected virtual FSMOperator<string, string, TId> CreateOperator()
        {
            return new FSMOperator<string, string, TId>(GeneratorId);
        }
        protected virtual RegExpFSMBuilder<string, TId> GetFSMBuilder()
        {
            return new RegExpFSMBuilder<string, TId>(GeneratorId);
        }
        #endregion Фабричные методы

        #region Implementation of IComparable<in GrammarRule>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(RegularGrammarRule<TId> other)
        {
            return DefinedSymbol.CompareTo(other.DefinedSymbol);
        }

        #endregion

        #region Implementation of IEquatable<GrammarRule>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(RegularGrammarRule<TId> other)
        {
            return DefinedSymbol.Equals(other.DefinedSymbol);
        }

        #endregion

        #region Implementation of IComparable<in string>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(string other)
        {
            return DefinedSymbol.Value.CompareTo(other);
        }

        #endregion
    }
}
