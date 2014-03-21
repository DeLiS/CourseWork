namespace FiniteStateMachines.Utility
{
    /// <summary>
    /// Типы состояний.
    /// </summary>
    public enum StateType
    {
        /// <summary>
        /// Начальное состояние.
        /// </summary>
        StartState,
        /// <summary>
        /// Конечное состояние.
        /// </summary>
        EndState,
        /// <summary>
        /// Промежуточное состояние.
        /// </summary>
        TransitionalState,
        /// <summary>
        /// Состояние одновременно и начальное, и конечное
        /// </summary>
        StartEndState
    }
}
