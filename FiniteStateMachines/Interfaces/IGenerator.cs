namespace FiniteStateMachines.Interfaces
{
    /// <remarks>
    /// Класс, генерирующий объекты типа <typeparamref name="T"/>.
    /// </remarks>
    /// <typeparam name="T">Некоторый тип.</typeparam>
    public interface IGenerator<out T>
    {
        ///<summary>
        /// Метод, генерирующий уникальные объекты типа <typeparamref name="T"/>.
        ///</summary>
        ///<returns>Уникальный объект типа <typeparamref name="T"/>.</returns>
        T GetUniqueId();
    }
}
