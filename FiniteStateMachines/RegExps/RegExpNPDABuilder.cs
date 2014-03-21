using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiniteStateMachines.Core;
using FiniteStateMachines.Processing;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;
namespace FiniteStateMachines.RegExps
{
    ///<summary>
    /// Класс, строящий автомат с магазинной памятью по дереву регулярного выражения.
    ///</summary>
    ///<typeparam name="TOut">Тип входных символов.</typeparam>
    ///<typeparam name="TStack">Тип символов магазинной памяти.</typeparam>
    ///<typeparam name="TId">Тип идентификаторов состояний автомата.</typeparam>
    public class RegExpNPDABuilder<TOut,TStack,TId> : RegExpFSMBuilder<TOut,TId>
        where TOut : IComparable<TOut>, IEquatable<TOut>
        where TStack:IComparable<TStack>,IEquatable<TStack>
        where TId:IComparable<TId>,IEquatable<TId>
    {
        private readonly IGenerator<TStack> _generatorTStack;
        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="generatorTStack">Генератор уникальных символов магазиной памяти.</param>
        ///<param name="generatorTId">Генератор уникальных идентификаторов состояний.</param>
        public RegExpNPDABuilder(IGenerator<TStack> generatorTStack,IGenerator<TId> generatorTId):base(generatorTId)
        {
            _generatorTStack = generatorTStack;
        }
        #region Фабричные методы
        protected override NFA<string, TOut,TId> CreateStateMatchine()
        {
            return new NPDA<string, TOut, TStack,TId>(_generatorTId,_generatorTStack);
        }
        protected override FSMOperator<string, TOut, TId> GetOperator(NFA<string, TOut, TId> left, NFA<string, TOut, TId> right)
        {
            return new PdafsmOperator<string, TOut, TStack, TId>(left, right, _generatorTStack,_generatorTId);
        }
        protected override IdStepSignature<string, TOut, TId> EmptyOutputStepSignature(TId start, ISymbol<string> input, TId end)
        {
            return new IdPushDownStepSignature<string, TOut, TStack, TId>(start, input,
                                                                new Symbol<TOut>(default(TOut), SymbolType.Empty), end);
        }
        protected override NFA<string, TOut, TId> CreateDeterminateStateMatchine()
        {
            return CreateStateMatchine();
        }
        #endregion Фабричные методы
    }
}
