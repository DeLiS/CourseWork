using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiniteStateMachines.Processing;
using FiniteStateMachines.Utility;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.RegExps;
namespace FiniteStateMachines.Grammars
{
    /// <summary>
    /// Правило контекстно-свободной грамматики.
    /// </summary>
    /// <typeparam name="TStack">Тип символов магазинной памяти распознающего автомата.</typeparam>
    /// <typeparam name="TId">Тип идентификаторов состояний распознающего автомата.</typeparam>
    public class ContextFreeGrammarRule<TStack,TId>:RegularGrammarRule<TId>
        where TStack:IComparable<TStack>,IEquatable<TStack>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        private readonly IGenerator<TStack> _generatorTStack;
        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="symbol">Нетерминальный символ, для которого создается правило вывода.</param>
        ///<param name="generatorTStack">Генератор уникальных символов магазинной памяти распознающего автомата.</param>
        ///<param name="generatorTId">Генератор уникальных идентификаторов сотояний распознающего автомата.</param>
        public ContextFreeGrammarRule(string symbol,IGenerator<TStack> generatorTStack,IGenerator<TId> generatorTId):base(symbol,generatorTId)
        {
            _generatorTStack = generatorTStack;
        }

        #region Фабричные методы
        protected override FSMOperator<string, string,TId> CreateOperator()
        {
            return new PdafsmOperator<string, string,TStack, TId>(_generatorTStack,GeneratorId);
        }
        protected override FSMOperator<string, string,TId> CreateOperator(List<Core.NFA<string, string,TId>> list)
        {
            return new PdafsmOperator<string, string, TStack, TId>(list, _generatorTStack, GeneratorId);
        }
        protected override FSMConverter<string, string,TId> CreateConverter()
        {
            return new FSMConverter<string, string, TId>(GeneratorId);
        }
        protected override RegExpFSMBuilder<string,TId> GetFSMBuilder()
        {
            return new RegExpNPDABuilder<string, TStack, TId>(_generatorTStack, GeneratorId);
        }
        #endregion Фабричные методы
    }
}
