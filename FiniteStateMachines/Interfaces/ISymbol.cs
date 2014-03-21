using System;
using FiniteStateMachines.Utility;

namespace FiniteStateMachines.Interfaces
{
    ///<summary>
    /// Интерфейс символа конечного автомата.
    ///</summary>
    ///<typeparam name="T">Тип значения символа.</typeparam>
    public interface ISymbol<T>:IComparable<ISymbol<T>>,IEquatable<ISymbol<T>>
        where T:IComparable<T>,IEquatable<T>
    {
        ///<summary>
        /// Тип символа.
        ///</summary>
        SymbolType Type { get; }
    }
}
