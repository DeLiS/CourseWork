using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiniteStateMachines.Processing;
using FiniteStateMachines.Utility;

using FiniteStateMachines.Interfaces;
namespace FiniteStateMachines.Grammars
{
    ///<summary>
    /// Контекстно-свободная грамматика.
    ///</summary>
    ///<typeparam name="TStack">Тип символов магазинной памяти.</typeparam>
    ///<typeparam name="TId">Тип идентификаторов состояний автомата, распознающего грамматику.</typeparam>
    public class ContextFreeGrammar<TStack, TId> : RegularGrammar<TId>
        where TStack : IComparable<TStack>, IEquatable<TStack>
        where TId : IComparable<TId>, IEquatable<TId>
    {
        /// <summary>
        /// Генератор уникальных символов для магазинной памяти.
        /// </summary>
        private readonly IGenerator<TStack> _generator;

        ///<summary>
        /// Конструктор
        ///</summary>
        ///<param name="generator">Генератор уникальных символов магазинной памяти.</param>
        ///<param name="generatorId">Генератор идентификаторов состояний автомата.</param>
        public ContextFreeGrammar(IGenerator<TStack> generator, IGenerator<TId> generatorId)
            : base(generatorId)
        {
            _generator = generator;

        }

        #region Фабричные методы
        protected override FSMConverter<string, string, TId> CreateConverter()
        {
            return new FSMConverter<string, string, TId>(GeneratorId);
        }
        protected override FSMOperator<string, string, TId> CreateOperator(Core.NFA<string, string, TId> first, Core.NFA<string, string, TId> second)
        {
            return new PdafsmOperator<string, string, TStack, TId>(first, second, _generator, GeneratorId);
        }
        protected override FSMOperator<string, string, TId> CreateOperator(List<Core.NFA<string, string, TId>> list)
        {
            return new PdafsmOperator<string, string, TStack, TId>(list, _generator, GeneratorId);
        }
        protected override FSMOperator<string, string, TId> CreateOperator()
        {
            return new PdafsmOperator<string, string, TStack, TId>(_generator, GeneratorId);
        }
        #endregion Фабричные методы
    }
}
