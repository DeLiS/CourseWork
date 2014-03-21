namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Тип узла дерева разбора регулярного выражения.
    ///</summary>
    public enum NodeType
    {
        ///<summary>
        /// Операция.
        ///</summary>
        Operation,
        ///<summary>
        /// Терминальный символ.
        ///</summary>
        Terminal,
        ///<summary>
        /// Нетерминальный символ.
        ///</summary>
        NonTerminal
    }
}
