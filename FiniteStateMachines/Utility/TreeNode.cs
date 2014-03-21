using System;
using System.Collections.Generic;

namespace FiniteStateMachines.Utility
{
    ///<summary>
    /// Узел дерева формулы регулярного выражения.
    ///</summary>
    ///<typeparam name="T">Тип символа.</typeparam>
    public class TreeNode<T>:IComparable<TreeNode<T>>
        where T:IComparable<T>
    {
        ///<summary>
        /// Тип узла.
        ///</summary>
        public NodeType Type { get;  set; }

        ///<summary>
        /// Тип операции.
        ///</summary>
        public OperationType Operation { get; set; }

        ///<summary>
        /// Символ.
        ///</summary>
        public T Symbol { get;  set; }

        /// <summary>
        /// Первый аргумент операции.
        /// </summary>
        public TreeNode<T> Left { get; set; }

        /// <summary>
        /// Второй аргумент операции (если есть).
        /// </summary>
        public TreeNode<T> Right { get; set; }

        /// <summary>
        /// Родительский узел.
        /// </summary>
        public TreeNode<T> Parent { get; set; }

        ///<summary>
        /// Конструктор.
        ///</summary>
        ///<param name="type">Тип создаваемого узла.</param>
        public TreeNode(NodeType type)
        {
            this.Type = type;
            FirstPos = new SortedSet<TreeNode<T>>();
            LastPos = new SortedSet<TreeNode<T>>();
            FollowPos = new SortedSet<TreeNode<T>>();
        }

        ///<summary>
        /// Конструктор по умолчанию.
        ///</summary>
        public TreeNode(){}

        ///<summary>
        /// Конструктор с параметром.
        ///</summary>
        ///<param name="parent">Родительский узел.</param>
        public TreeNode(TreeNode<T> parent)
        {
            Parent = parent; FirstPos = new SortedSet<TreeNode<T>>();
            LastPos = new SortedSet<TreeNode<T>>();
            FollowPos = new SortedSet<TreeNode<T>>();
        }

        public int Position { get;  set; }

        public bool Nullable { get; set; }
        public SortedSet<TreeNode<T>> FirstPos { get; set; }
        public SortedSet<TreeNode<T>> LastPos { get; set; }
        public SortedSet<TreeNode<T>> FollowPos { get; set; }
        public int CompareTo(TreeNode<T> other)
        {
            int cmp = Type.CompareTo(other.Type);
            if(cmp!=0)
                return cmp;
            switch (Type)
            {
                case NodeType.Operation:
                    return Operation.CompareTo(other.Operation);
                case NodeType.Terminal:
                case NodeType.NonTerminal:
                    cmp =  Symbol.CompareTo(other.Symbol);
                    if (cmp != 0)
                        return cmp;
                    return Position.CompareTo(other.Position);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
