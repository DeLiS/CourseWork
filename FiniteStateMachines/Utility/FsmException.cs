using System;
using FiniteStateMachines.Interfaces;

namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Исключение
    ///</summary>
    public class FsmException<TIn, TOut, TId> : ApplicationException
        where TIn : IComparable<TIn>, IEquatable<TIn>
    {
        public FsmExceptionId ID { get; protected set; }
        public TId GUID { get; protected set; }
        public ISymbol<TIn> Input { get; protected set; }

        ///<summary>
        /// Конструктор
        ///</summary>
        ///<param name="id">Номер исключения</param>
        ///<param name="message">Сообщение об ошибке</param>
        public FsmException(FsmExceptionId id, string message = "oops")
            : base(message)
        {
            ID = id;
        }
        ///<summary>
        /// Конструктор с 2-мя параметрами
        ///</summary>
        ///<param name="id">Идентификатор исключения</param>
        ///<param name="guid">Идентификатор состояния</param>
        /// ///<param name="message">Сообщение об ошибке</param>
        public FsmException(FsmExceptionId id, TId guid, string message = "oops")
            : base(message)
        {
            ID = id;
            GUID = guid;
        }
        ///<summary>
        /// Конструктор
        ///</summary>
        ///<param name="id">Идентификатор исключения</param>
        ///<param name="input">Входной символ</param>
        /// ///<param name="message">Сообщение об ошибке</param>
        public FsmException(FsmExceptionId id, ISymbol<TIn> input, string message = "oops")
            : base(message)
        {
            ID = id;
            Input = input;
        }

    }
}
