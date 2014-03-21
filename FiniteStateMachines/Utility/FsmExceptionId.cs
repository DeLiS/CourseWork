namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Идентификатор исключения
    ///</summary>
    public enum FsmExceptionId
    {
        ///<summary>
        ///</summary>
        NoTransition,
        ///<summary>
        ///</summary>
        WrongStateType,
        ///<summary>
        ///</summary>
        NoStateWithSuchGuid,
        ///<summary>
        ///</summary>
        InputSymbolIsNull,
        ///<summary>
        ///</summary>
        OutputSymbolIsNull,
        ///<summary>
        ///</summary>
        InputSymbolIsEmpty,
        ///<summary>
        /// Из состояния уже определен переход по данному символу
        ///</summary>
        SameInput,
        /// <summary>
        /// Грамматика не является регулярной
        /// </summary>
        NotRegular,
    }
}