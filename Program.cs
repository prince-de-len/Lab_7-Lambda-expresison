using System;
using System.Collections;
using System.Collections.Generic;

public class BinaryTreeCollection<T> : IEnumerable<T>
{
    public class BinaryTreeNode
    {
        public T Value { get; set; }
        public BinaryTreeNode Left { get; set; }
        public BinaryTreeNode Right { get; set; }

        public BinaryTreeNode(T value)
        {
            Value = value;
        }

        public void InitializeNodes(T rootValue)
        {
            Left = new BinaryTreeNode(default(T));
            Right = new BinaryTreeNode(default(T));
            Left.Left = new BinaryTreeNode(default(T));
            Right.Left = new BinaryTreeNode(default(T));
            Left.Right = new BinaryTreeNode(default(T));
            Right.Right = new BinaryTreeNode(default(T));
        }

        public void AddNodes(T firstNode, T SecondNode, T ThirdNode, T FourthNode, T FifthNode, T SixthNode)
        {
            Left.Value = firstNode;
            Right.Value = SecondNode;
            Left.Left.Value = ThirdNode;
            Left.Right.Value = FourthNode;
            Right.Left.Value = SecondNode;
            Right.Right.Value = SecondNode;
        }
    }

    public BinaryTreeNode Root;

    public void Add(T value)
    {
        Root = AddRecursive(Root, value);
    }

    private BinaryTreeNode AddRecursive(BinaryTreeNode current, T value)
    {
        if (current == null)
        {
            return new BinaryTreeNode(value);
        }

        int compareResult = Comparer<T>.Default.Compare(value, current.Value);
        if (compareResult < 0)
        {
            current.Left = AddRecursive(current.Left, value);
        }
        else if (compareResult > 0)
        {
            current.Right = AddRecursive(current.Right, value);
        }

        return current;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new InOrderTraversalEnumerator(Root);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class InOrderTraversalEnumerator : IEnumerator<T>
    {
        private BinaryTreeNode _current;
        private Stack<BinaryTreeNode> _stack;

        public InOrderTraversalEnumerator(BinaryTreeNode root)
        {
            _current = root;
            _stack = new Stack<BinaryTreeNode>();
            FillStack(_current);
        }

        private void FillStack(BinaryTreeNode node)
        {
            while (node != null)
            {
                _stack.Push(node);
                node = node.Left;
            }
        }

        public T Current => _current.Value;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_stack.Count == 0)
            {
                _current = null;
                return false;
            }

            _current = _stack.Pop();
            FillStack(_current.Right);
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}

public class BinaryTree<T>
{
    private BinaryTreeCollection<T> _collection;

    public BinaryTree(BinaryTreeCollection<T>.BinaryTreeNode root)
    {
        _collection = new BinaryTreeCollection<T>();
        AddNode(root);
    }

    private void AddNode(BinaryTreeCollection<T>.BinaryTreeNode node)
    {
        _collection.Add(node.Value);
        if (node.Left != null)
        {
            AddNode(node.Left);
        }
        if (node.Right != null)
        {
            AddNode(node.Right);
        }
    }

    public IEnumerable<T> GetCentralTraversal(Func<BinaryTreeCollection<T>.BinaryTreeNode, IEnumerable<T>> centralTraversal)
    {
        return centralTraversal(_collection.Root);
    }
}

class Program
{
    static void Main()
    {
        Random rand = new Random();
        BinaryTreeCollection<int>.BinaryTreeNode root = new BinaryTreeCollection<int>.BinaryTreeNode(5);
        root.InitializeNodes(1);
        root.AddNodes(1, 2, 3, 4, 5, 6);

        BinaryTree<int> tree = new BinaryTree<int>(root);

        Func<BinaryTreeCollection<int>.BinaryTreeNode, IEnumerable<int>> centralTraversal = null;
        centralTraversal = node =>
        {
            if (node == null)
                return new List<int>();

            var result = new List<int>();
            result.AddRange(centralTraversal(node.Left));
            result.Add(node.Value);
            result.AddRange(centralTraversal(node.Right));
            return result;
        };

        var centralTraversalResult = tree.GetCentralTraversal(centralTraversal);

        Console.WriteLine("Central Traversal:");
        foreach (var value in centralTraversalResult)
        {
            Console.WriteLine(value);
        }
    }
}
