namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Тип операции.
    ///</summary>
    public enum OperationType
    {
        ///<summary>
        /// Конкатенация (&).
        ///</summary>
        Concatenation,
        ///<summary>
        /// Альтернатива (|).
        ///</summary>
        Alternative,
        ///<summary>
        ///Группировка (()).
        ///</summary>
        Grouping,
        ///<summary>
        /// Ноль или более раз (*).
        ///</summary>
        Asterisk,
        ///<summary>
        /// 0 или 1 раз ([]).
        ///</summary>
        Option,
        /// <summary>
        /// 1 или более раз (+).
        /// </summary>
        Plus
    }
}
