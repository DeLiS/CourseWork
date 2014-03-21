namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Типы символов.
    ///</summary>
    public enum SymbolType
    {
        ///<summary>
        /// Пустой символ.
        ///</summary>
        Empty,
        ///<summary>
        /// Терминальный символ.
        ///</summary>
        Terminal,
        ///<summary>
        /// Нетерминальный символ.
        ///</summary>
        NonTerminal,
        ///<summary>
        /// Составной символ.
        ///</summary>
        Complex,
        /// <summary>
        /// Граничный символ регулярного выражения.
        /// </summary>
        Border
    }
}