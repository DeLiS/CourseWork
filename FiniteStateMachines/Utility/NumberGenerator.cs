using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiniteStateMachines.Interfaces;
namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Генератор целых чисел.
    ///</summary>
    public class NumberGenerator:IGenerator<int>
    {
        private static int _currentNumber = 1;
        #region Implementation of IGenerator<int>

        ///<summary>
        /// Генерирует уникальные целые.
        ///</summary>
        ///<returns>Уникальное целое число.</returns>
        public int GetUniqueId()
        {
            _currentNumber += 1;
            return _currentNumber;
        }

        #endregion
    }
}
