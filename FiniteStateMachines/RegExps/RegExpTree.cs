using System;
using FiniteStateMachines.Utility;

namespace FiniteStateMachines.RegExps
{
    ///<summary>
    ///Дерево разбора регулярного выражения.
    ///</summary>
    ///<typeparam name="T">Тип символа.</typeparam>
    public class RegExpTree<T>
        where T:IComparable<T>
    {
        /// <summary>
        /// Корень дерева разбора.
        /// </summary>
        protected TreeNode<T> Root { get; set; }

        /// <summary>
        /// Текущий узел.
        /// </summary>
        protected TreeNode<T> CurrentNode { get; set; }

        ///<summary>
        /// Конструктор по умолчанию. Создает пустое дерево.
        ///</summary>
        public RegExpTree(){}

        ///<summary>
        /// Конструктор с параметром. Создает дерево с заданным корнем.
        ///</summary>
        ///<param name="root">Корневой узел.</param>
        public RegExpTree(TreeNode<T> root)
        {
            Root = root;
            CurrentNode = root;
        }
      
        ///<summary>
        /// Тип текущего узла.
        ///</summary>
        public NodeType Type
        {
            get { return CurrentNode.Type; }
        }

        ///<summary>
        /// Операция текущего узла.
        ///</summary>
        public OperationType Operation
        {
            get { return CurrentNode.Operation; }
        }

        ///<summary>
        /// Символ в текущем узле.
        ///</summary>
        public T Symbol
        {
            get { return CurrentNode.Symbol; }
        }


        ///<summary>
        /// Движение по дереву
        ///</summary>
        ///<param name="direction">Направление движения (влево, вправо, вверх)</param>
        ///<exception cref="Exception"></exception>
        ///<exception cref="ArgumentOutOfRangeException"></exception>
        public void Move(Direction direction)
        {
            if (CurrentNode == null)
            {
                throw new Exception("RegExpTree:Move:Current node is null");
            }
            switch (direction)
            {
                case Direction.Left:
                    GoLeft();
                    break;
                case Direction.Right:
                    GoRight();
                    break;
                case Direction.Up:
                    GoUp();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        ///<summary>
        /// Переход к левому потомку.
        ///</summary>
        private void GoLeft()
        {
            if(CurrentNode.Left == null)
                throw new Exception("Can't go left");
            CurrentNode = CurrentNode.Left;
        }

        ///<summary>
        /// Переход к правому потомку.
        ///</summary>
        ///<exception cref="Exception"></exception>
        private void GoRight()
        {
            if(CurrentNode.Right == null)
                throw new Exception("Can't go right");
            CurrentNode = CurrentNode.Right;
        }

        ///<summary>
        /// Переход к родительскому узлу.
        ///</summary>
        private void GoUp()
        {
            if(CurrentNode.Parent == null)
                throw new Exception("In root");
            CurrentNode = CurrentNode.Parent;
        }

        ///<summary>
        /// Устанавливает корень текущим узлом.
        ///</summary>
        public void Reset()
        {
            CurrentNode = Root;
        }

        #region Unused


        ///<summary>
        /// Проверка возможности перемещения в заданном направлении.
        ///</summary>
        ///<param name="direction">Направление.</param>
        ///<returns>Истина, если можно переместится в заданном направлении.</returns>
        ///<exception cref="ArgumentOutOfRangeException"></exception>
        public bool CanMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return CurrentNode.Left != null;
                case Direction.Right:
                    return CurrentNode.Right != null;
                case Direction.Up:
                    return CurrentNode.Parent != null;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        ///<summary>
        /// Записать символ в узел.
        ///</summary>
        ///<param name="value">Значение символа.</param>
        ///<exception cref="ApplicationException"></exception>
        private void WriteValue(T value)
        {
            if(CurrentNode == null || CurrentNode.Type == NodeType.Operation)
                throw new ApplicationException("Current node is null is wrong type");
            CurrentNode.Symbol = value;
        }

        ///<summary>
        /// Записать в узел операцию.
        ///</summary>
        ///<param name="operationType">Тип операции.</param>
        ///<exception cref="ApplicationException"></exception>
        private void WriteOperation(OperationType operationType)
        {
            if (CurrentNode == null || CurrentNode.Type != NodeType.Operation)
                throw new ApplicationException("Current node is null is wrong type");
            CurrentNode.Operation = operationType;
        }

        ///<summary>
        /// Установка корня для пустого дерева.
        ///</summary>
        ///<param name="root">Корень.</param>
        private void SetRoot(TreeNode<T> root)
        {
            if (Root == null)
                throw new ApplicationException("Root is no null");

            Root = new TreeNode<T>(root.Type) { Operation = root.Operation };
            root.Symbol = root.Symbol;
            Root.Left = Root.Right = Root.Parent = null;
            CurrentNode = Root;

        }

        ///<summary>
        /// Создание потомка.
        ///</summary>
        ///<param name="direction">Левый или правый потомок.</param>
        ///<param name="nodeType">Тип потомка.</param>
        ///<exception cref="ApplicationException"></exception>
        ///<exception cref="ArgumentOutOfRangeException"></exception>
        private void CreateChild(Direction direction, NodeType nodeType)
        {
            if (CurrentNode == null)
                throw new ApplicationException("RegExpTree:CreateChilde:Current node is null");
            switch (direction)
            {
                case Direction.Left:
                    AddLeft(nodeType);
                    break;
                case Direction.Right:
                    AddRight(nodeType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        ///<summary>
        /// Добавить левого потомка.
        ///</summary>
        ///<param name="type">Тип создаваемого узла.</param>
        private void AddLeft(NodeType type)
        {
            if (CurrentNode.Left == null)
            {
                CurrentNode.Left = new TreeNode<T>(type);
            }
            else
            {
                throw new ApplicationException("Already has left child");
            }
        }

        ///<summary>
        /// Добавление правого потомка к текущему узлу.
        ///</summary>
        ///<param name="type">Тип узла.</param>
        private void AddRight(NodeType type)
        {
            if (CurrentNode.Right == null)
            {
                CurrentNode.Right = new TreeNode<T>(type);
            }
            else
            {
                throw new ApplicationException("Already has right child");
            }
        }

        #endregion
    }
}
