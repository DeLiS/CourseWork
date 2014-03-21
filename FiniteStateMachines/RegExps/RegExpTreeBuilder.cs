using System;
using System.Collections.Generic;
using FiniteStateMachines.Interfaces;
using FiniteStateMachines.Utility;

namespace FiniteStateMachines.RegExps
{
    /// <remarks>
    /// Класс, который по полученному регулярному выражению строит дерево разбора.
    /// </remarks>
    public class RegExpTreeBuilder
    {

        ///<summary>
        /// Дерево разбора регулярного выражения.
        ///</summary>
        public RegExpTree<ISymbol<string>> Tree { get; private set; }

        public TreeNode<ISymbol<string>> Root { get; set; }

        ///<summary>
        /// Регулярное выражение.
        ///</summary>
        public String Regexp { get; private set; }

        private int _index;

        private int _regExpLength;

        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="regexp">Регулярное выражение.</param>
        public RegExpTreeBuilder(string regexp)
        {
            Regexp = regexp;
            _regExpLength = regexp.Length;
        }

        ///<summary>
        /// Метод, создающий дерево. note:Пока что работает только с выражениями с расставленными скобками и 3-мя основными операциями
        /// todo:Не забыть написать метод, расставляющий скобки в выражении и преобразовывающий неосновные операции в основные.
        ///</summary>
        public void BuildTree()
        {
            _regExpLength = Regexp.Length;

            Root = new TreeNode<ISymbol<string>>(NodeType.Operation);
            Root.Operation = OperationType.Concatenation;
            Root.Left = new TreeNode<ISymbol<string>>(Root);
            Root.Right = new TreeNode<ISymbol<string>>(NodeType.Terminal); // аналог сигнальной карты, конец выражения
            Root.Right.Symbol = new Symbol<string>("#",SymbolType.Border);

            TreeNode<ISymbol<string>> current = Root.Left;

            while (_index < _regExpLength)
            {
                switch (Regexp[_index])
                {
                    case '[':
                        current.Type = NodeType.Operation;
                        current.Operation = OperationType.Option;

                        current.Left = new TreeNode<ISymbol<string>>(current);
                        current = current.Left;
                        break;
                    case ']':
                        current = current.Parent;
                        break;

                    case '(':
                        current.Left = new TreeNode<ISymbol<string>>(current);
                        current = current.Left;
                        break;

                    case ')':
                        current = current.Parent;
                        break;

                    case '*':
                        current.Type = NodeType.Operation;
                        current.Operation = OperationType.Asterisk;
                        break;

                    case '|':
                        current.Type = NodeType.Operation;
                        current.Operation = OperationType.Alternative;

                        current.Right = new TreeNode<ISymbol<string>>(current);
                        current = current.Right;
                        break;

                    case '&':
                        current.Type = NodeType.Operation;
                        current.Operation = OperationType.Concatenation;

                        current.Right = new TreeNode<ISymbol<string>>(current);
                        current = current.Right;
                        break;

                    case '\'':
                        current.Type = NodeType.Terminal;
                        current.Position = _index + 1;
                        current.Symbol = ReadName(SymbolType.Terminal, '\'');
                        current = current.Parent;
                        break;

                    case '<':
                        current.Type = NodeType.NonTerminal;
                        current.Position = _index + 1;
                        current.Symbol = ReadName(SymbolType.NonTerminal, '>');
                        current = current.Parent;
                        break;
                    case '+':
                        current.Type = NodeType.Operation;
                        current.Operation = OperationType.Plus;

                        break;
                    default:
                        throw new ApplicationException("Something goes wrong!");

                }
                _index = _index + 1;
            }

            Tree = new RegExpTree<ISymbol<string>>(Root);
        }

        /// <summary>
        /// Перед вызовом функции индекс указывает на открывающий символ (кавычку или скобку)
        /// </summary>
        /// <param name="type">Терминал или нетерминал</param>
        /// <param name="end">Закрывающий символ</param>
        /// <returns>Создает символ с прочитанным именем и переданным типом. После выхода индекс указывает на закрывающий символ</returns>
        private Symbol<string> ReadName(SymbolType type, char end)
        {
            string buffer = "";
            _index = _index + 1;
            while (Regexp[_index] != end)
            {
                if (Regexp[_index] == '\\')
                {
                    _index++;
                    switch (Regexp[_index])
                    {
                        case '\\':
                            buffer += '\\';
                            break;
                        case 't':
                            buffer += '\t';
                            break;
                        case 'n':
                            buffer += '\n';
                            break;
                        case 'r':
                            buffer += '\r';
                            break;
                        case '\'':
                            buffer += '\'';
                            break;
                        case '<':
                            buffer += '<';
                            break;
                    }

                }
                else
                {
                    buffer += Regexp[_index];
                }
                _index = _index + 1;
            }
            if (buffer.Length == 0)
            {
                if (type == SymbolType.Terminal)
                    return new Symbol<string>(); //пустой символ
                throw new Exception("Empty name");

            }
            var symbol = new Symbol<string>(buffer, type);
            return symbol;

        }

        ///<summary>
        /// Печать дерева.
        ///</summary>
        ///<returns>Строковое представление.</returns>
        public string PrintTree()
        {
            return Print(Root);
        }

        ///<summary>
        /// Печать дерева "левое-корень-правое".
        ///</summary>
        ///<param name="node">Корень дерева.</param>
        private static string Print(TreeNode<ISymbol<string>> node)
        {
            string tmp = "";
            if (node.Left != null)
                tmp += Print(node.Left);

            //  for (int i = 0; i < dist; ++i)
            //      tmp += " ";
            switch (node.Type)
            {
                case NodeType.Operation:
                    switch (node.Operation)
                    {
                        case OperationType.Concatenation:
                            tmp += "&";
                            break;
                        case OperationType.Alternative:
                            tmp += '|';
                            break;
                        case OperationType.Asterisk:
                            tmp += '*';
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case NodeType.Terminal:
                    tmp += node.Symbol.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            tmp += "\n";

            if (node.Right != null)
                tmp += Print(node.Right);
            return tmp;
        }

        ///<summary>
        /// Метод, подсчитывающий в дереве регулярного выражения значения nullable, firstpos, lastpos, followpos
        ///</summary>
        ///<param name="root">корень дерева</param>
        ///<exception cref="ArgumentOutOfRangeException"></exception>
        public static void Calculate(TreeNode<ISymbol<string>> root)
        {
           if(root.Type == NodeType.NonTerminal || root.Type == NodeType.Terminal)
           {
               root.Nullable = root.Symbol.Type == SymbolType.Empty;
               if (!root.Nullable)
               {
                   root.FirstPos.Add(root);
                   root.LastPos.Add(root);
               }
               return;
           }
           
            switch (root.Operation)
            {
                case OperationType.Concatenation:
                    Calculate(root.Left);
                    Calculate(root.Right);
                    root.Nullable = root.Left.Nullable && root.Right.Nullable;
                    if(root.Left.Nullable)
                    {
                        root.FirstPos.UnionWith(root.Right.FirstPos);
                    }
                    root.FirstPos.UnionWith(root.Left.FirstPos);
                    
                    if(root.Right.Nullable)
                    {
                        root.LastPos.UnionWith(root.Left.LastPos);
                    }
                    root.LastPos.UnionWith(root.Right.LastPos);
                    
                    var leftLastPoses = root.Left.LastPos;
                    foreach (var leftLastPos in leftLastPoses)
                    {
                        leftLastPos.FollowPos.UnionWith(root.Right.FirstPos);
                    }
                    break;
                case OperationType.Alternative:
                    Calculate(root.Left);
                    Calculate(root.Right);
                    root.Nullable = root.Left.Nullable || root.Right.Nullable;
                    root.FirstPos.UnionWith(root.Left.FirstPos);
                    root.FirstPos.UnionWith(root.Right.FirstPos);
                    root.LastPos.UnionWith(root.Left.LastPos);
                    root.LastPos.UnionWith(root.Right.LastPos);
                    break;
                case OperationType.Asterisk:
                    Calculate(root.Left);
                    root.Nullable = true;
                    root.FirstPos.UnionWith(root.Left.FirstPos);
                    root.LastPos.UnionWith(root.Left.LastPos);
                    foreach (var lastpos in root.LastPos)
                    {
                        lastpos.FollowPos.UnionWith(root.FirstPos);
                    }
                    break;
                case OperationType.Option:
                    Calculate(root.Left);
                    root.Nullable = true;
                    root.FirstPos.UnionWith(root.Left.FirstPos);
                    root.LastPos.UnionWith(root.Left.LastPos);
                    break;
                case OperationType.Plus:
                    Calculate(root.Left);
                    root.Nullable = root.Left.Nullable;
                    root.FirstPos.UnionWith(root.Left.FirstPos);
                    root.LastPos.UnionWith(root.Left.LastPos);
                    foreach (var treeNode in root.LastPos)
                    {
                        treeNode.FollowPos.UnionWith(root.FirstPos);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
