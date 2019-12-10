using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryTree_MovieApp.Data.Models
{
    public class BinaryTree<T> where T : IBinaryNode<T>
    {
        // PRIVATE PROPS

        private T root;
        private List<T> nodes;
        private List<T> nodeTraversalResults;
        private string nodeTraversalString;
        private T searchResult;

        // CONSTRUCTORS

        /// <summary>
        /// A Binary tree organizes nodes into
        /// a structure suitable for binary search
        /// </summary>
        public BinaryTree()
        {
            nodes = new List<T>();
        }

        /// <summary>
        /// A binary tree organizes nodes into
        /// a structure suitable for binary search
        /// </summary>
        /// <param name="_nodes">
        /// A list of nodes whose type implements
        /// the 'IBinaryNode' interface
        /// </param>
        public BinaryTree(List<T> _nodes)
        {
            if (_nodes == null) return;

            nodes = new List<T>();

            foreach(T node in _nodes)
            {
                if (!nodes.Contains(node))
                    nodes.Add(node);
            }

            nodes.Sort();

            nodes = _nodes;
            root = Build(0, nodes.Count - 1);
        }

        /// <summary>
        /// A binary tree organizes nodes into
        /// a structure suitable for binary search
        /// </summary>
        /// <param name="_nodes">
        /// A list of nodes whose type implements
        /// the 'IBinaryNode' interface
        /// </param>
        /// <param name="duplicates">
        /// A binary tree can only contain ONE of
        /// instance of each node. Duplicates will
        /// be passed into a list as an out variable
        /// </param>
        public BinaryTree(List<T> _nodes, out List<T> duplicates)
        {
            duplicates = null;
            if (_nodes == null) return;
            
            nodes = new List<T>();
            duplicates = new List<T>();

            foreach (T node in _nodes)
            {
                if (!nodes.Contains(node))
                    nodes.Add(node);
                else
                    duplicates.Add(node);
            }

            nodes.Sort();

            root = Build(0, nodes.Count - 1);
        }

        // METHODS

        // Building the tree

        private T Build(int startIndex, int endIndex)
        {
            if (endIndex < startIndex) return default(T);

            int middleIndex = (int)Math.Floor(((double)startIndex + (double)endIndex) / 2);

            T newNode = nodes[middleIndex];

            newNode.Left = Build(startIndex, middleIndex - 1);
            newNode.Right = Build(middleIndex + 1, endIndex);

            return newNode;
        }

        // Updating the tree

        /// <summary>
        /// Add a new, unique node to the binary tree.
        /// Duplicates or nodes that otherwise cannot
        /// be added will return a value of 'false.'
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Add(T node)
        {
            if (nodes.Contains(node)) return false;

            try
            {
                nodes.Add(node);
                nodes.Sort();
                root = Build(0, nodes.Count - 1);
            } 
            catch { return false; }

            return true;
        }

        // Searching the tree

        /// <summary>
        /// Performs a fast search for a specific
        /// binary node. An item is considered a
        /// match if it produces a 'true' value
        /// in the node's 'Equals' method.
        /// </summary>
        /// <param name="searchTerm">
        /// The item being searched for.
        /// </param>
        /// <returns></returns>
        public T BinarySearch(T searchTerm)
        {
            T current = root;

            while (true)
            {
                if (current == null) return default(T);
                if (current.Equals(searchTerm)) return current;

                if (searchTerm.CompareTo(current) < 0)
                    current = current.Left;
                else current = current.Right;
            }
        }

        /// <summary>
        /// Performs a complete search of the 
        /// binary tree and returns the first
        /// node that causes the 'MeetsSearchCriteria'
        /// method to resolve to 'true'.
        /// </summary>
        /// <param name="searchTerm">
        /// The item being searched for.
        /// </param>
        /// <returns></returns>
        public T TraversalSearch(T searchTerm)
        {
            if (searchTerm == null) return default(T);

            searchResult = default(T);
            DepthFirstTraversal(root, searchTerm);

            return searchResult;
        }

        // Displaying the tree
        
        public List<T> GetNodesDepthFirst()
        {
            DepthFirstTraversal();
            return nodeTraversalResults;
        }

        public List<T> GetNodesBreadthFirst()
        {
            BreadthFirstTraversal();
            return nodeTraversalResults;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(nodeTraversalString))
            {
                DepthFirstTraversal();
            }
            
            return nodeTraversalString;
        }

        // Perform Traversals

        // Breadth-first
        public void BreadthFirstTraversal()
        {
            nodeTraversalResults = new List<T>();

            Queue<T> s = new Queue<T>();
            s.Enqueue(root);

            nodeTraversalResults.Add(root);

            while (s.Count > 0)
            {
                //pop a node
                T current = s.Dequeue();
                //           if (current == null) continue;
                //push its children

                //right favored breadth first traversal

                if (current.Left != null)
                    s.Enqueue(current.Left);
                if (current.Right != null)
                    s.Enqueue(current.Right);

                if (current.Left != null || current.Right != null)
                {
                    nodeTraversalResults.Add(current.Left);
                    nodeTraversalResults.Add(current.Right);
                }
                
            }

        }

        // Depth-first

        private void DepthFirstTraversal()
        {
            nodeTraversalResults = new List<T>();
            nodeTraversalString = "";
            DepthFirstTraversal(root);
        }

        private void DepthFirstTraversal(T current)
        {
            if (current == null) return;

            nodeTraversalResults.Add(current);
            nodeTraversalString += $"{current.ToString()}\n";

            DepthFirstTraversal(current.Left);
            DepthFirstTraversal(current.Right);
        }

        private void DepthFirstTraversal(T current, T searchTerm)
        {
            if (current == null) return;

            if (current.MeetsSearchCriteria(searchTerm))
            {
                searchResult = current;
                return;
            }

            DepthFirstTraversal(current.Left, searchTerm);
            DepthFirstTraversal(current.Right, searchTerm);
        }

    }
}
