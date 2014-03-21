namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Перечисление возможных действий со стековой памятью автомата с магазинной памятью.
    ///</summary>
    public enum StackActions
    {

        ///<summary>
        /// Вытолкнуть верхний элемент.
        ///</summary>
        Pop,

        ///<summary>
        /// Добавить на вершину элемент.
        ///</summary>
        Push,
        /// <summary>
        /// Сначала удалить старый символ с верхушки стека, а потом поместить туда новый.
        /// </summary>
        PopPush,
        ///<summary>
        /// Ничего не делать с памятью.
        ///</summary>
        Nothing
    }
}
