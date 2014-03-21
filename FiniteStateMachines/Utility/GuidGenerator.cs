using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiniteStateMachines.Interfaces;
namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Генератор GUID.
    ///</summary>
    public class GuidGenerator:IGenerator<Guid>
    {
        #region Implementation of IGenerator<Guid>

        ///<summary>
        /// Генерирует уникальный GUID.
        ///</summary>
        ///<returns>Уникальный GUID.</returns>
        public Guid GetUniqueId()
        {
            return Guid.NewGuid();
        }

        #endregion
    }
}
