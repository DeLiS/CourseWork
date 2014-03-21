using System;
using System.Collections.Generic;
using System.Linq;
using FiniteStateMachines.Core;
using FiniteStateMachines.Grammars;
using FiniteStateMachines.Interfaces;

namespace FiniteStateMachines.Utility
{
    /// <summary>
    /// Класс, проверяющий некоторое слово на принадлежность к языку, определяемому автоматом.
    /// </summary>
    /// <typeparam name="TId">Тип идентификатора состояния автомата.</typeparam>
   public class Checker<TId>
       where TId:IComparable<TId>,IEquatable<TId>
   {
       /// <summary>
       /// Автомат, построенный по некоторой грамматике.
       /// </summary>
       private readonly NFA<string, string, TId> _acceptor;

       ///<summary>
       /// Конструктор.
       ///</summary>
       ///<param name="acceptor">Автомат, построенный по некоторой грамматике.</param>
       public Checker(NFA<string,string,TId > acceptor)
       {
           _acceptor = acceptor;
       }

       ///<summary>
       /// Метод, пропускающий строку <paramref name="someString"/> через автомат правила <paramref name="rule"/>.
       ///</summary>
       ///<param name="rule">Правило грамматики.</param>
       ///<param name="someString">Некоторая строка.</param>
       ///<returns>Последний выходной символ автомата правила.</returns>
       public static ISymbol<string> StaticCheck(RegularGrammarRule<TId> rule, String someString )
       {
           ISet<ISymbol<string>> result = new SortedSet<ISymbol<string>>();
           rule.Acceptor.Reset();
           for(int i = 0; i< someString.Count();++i)
           {
               result = rule.Acceptor.MakeStep(new Symbol<string>(someString[i].ToString(),SymbolType.NonTerminal));
              
           }
            foreach (var symbol in result)
               {
                   if(symbol.Type != SymbolType.Empty)
                   {
                       var simpleSymbol = symbol as Symbol<string>;
                       if (simpleSymbol != null)
                           return simpleSymbol;
                   }
               }
           return new Symbol<string>();
       }

       /// <summary>
       /// Метод, возвращающий последний выходной символ автомата, переданного в конструктор после обработки всех символов строки <paramref name="someString"/>.
       /// </summary>
       /// <param name="someString">Строка символов.</param>
       /// <returns></returns>
       public ISet<ISymbol<string >> Check(String someString,bool deterministic = false)
       {
           ISet<ISymbol<string>> result = new SortedSet<ISymbol<string>>();
           _acceptor.Reset();
           if (someString.Count() == 0 && !deterministic)
               result = _acceptor.MakeStep(new Symbol<string>());
           for (int i = 0; i < someString.Count(); ++i)
           {
               result = _acceptor.MakeStep(new Symbol<string>(someString[i].ToString(), SymbolType.NonTerminal));

           }
           result.Remove(new Symbol<string>());
           return  result;
       }
    }
}
