using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryTree_MovieApp.Data.Models
{
    public class BinaryTree<T> where T : IBinaryNode<T>
    {
        private T root;
        private List<T> nodes;
        private List<T> nodeTraversalResults;
        private string nodeTraversalString;

        public T Root { get { return root; } }

        public BinaryTree()
        {
            root = default(T);
            nodes = new List<T>();
        }

        public BinaryTree(List<T> _nodes, out List<T> duplicates)
        {
            duplicates = null;
            if (_nodes == null) return;

            duplicates = RemoveDuplicates(_nodes);
            _nodes.Sort();
            
            nodes = _nodes;
            root = Build(0, nodes.Count - 1);
        }

        private T Build(int startIndex, int endIndex)
        {
            if (endIndex < startIndex) return default(T);

            int middleIndex = (int)Math.Floor(((double)startIndex + (double)endIndex) / 2);

            T newNode = nodes[middleIndex];

            newNode.Left = Build(startIndex, middleIndex - 1);
            newNode.Right = Build(middleIndex + 1, endIndex);

            return newNode;
        }

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

        public T Search(T searchTerm)
        {
            T current = root;

            while (true)
            {
                if (current == null) return default(T);
                if (current.MeetsSearchCriteria(searchTerm)) return current;

                if (searchTerm.CompareTo(current) < 0)
                    current = current.Left;
                else current = current.Right;
            }
        }

        // TODO: make this more efficient
        private List<T> RemoveDuplicates(List<T> inputs)
        {
            List<T> duplciates = new List<T>();

            foreach(T input in inputs)
            {
                int countOfItem = inputs.Where(i => i.Equals(input)).Select(i => i).Count();
                if(countOfItem > 1 && !(duplciates.Contains(input)))
                {
                    duplciates.Add(input);
                }
            }

            foreach(T duplicate in duplciates)
            {
                inputs.Remove(duplicate);
            }

            return duplciates;
        }

        public override string ToString()
        {
            return nodeTraversalString;
        }

        public List<T> GetNodes(bool depthFirst = true)
        {
            nodeTraversalResults = new List<T>();
            nodeTraversalString = "";

            if (depthFirst)
            {
                DepthFirstTraversal();
            }
            else
            {
                BreadthFirstTraversal();
            }

            return nodeTraversalResults;
        }

        public void BreadthFirstTraversal()
        {
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

                if(current.Left != null || current.Right != null)
                {
                    nodeTraversalResults.Add(current.Left);
                    nodeTraversalResults.Add(current.Right);
                }
                
                nodeTraversalString += $"{current.ToString()}\n";

            }

        }

        private List<T> DepthFirstTraversal()
        {

            nodeTraversalResults.Add(root);
            nodeTraversalString = $"{root.ToString()}\n";

            DepthFirstTraversal(root.Left, root.Right);

            return nodeTraversalResults;
        }

        public void DepthFirstTraversal(T left, T right)
        {
            if(left != null)
            {
                nodeTraversalResults.Add(left);
                nodeTraversalString += $"{left.ToString()}\n";
                DepthFirstTraversal(left.Left, left.Right);
            }

            if(right != null)
            {
                nodeTraversalResults.Add(right);
                nodeTraversalString += $"{right.ToString()}\n";
                DepthFirstTraversal(right.Left, right.Right);
            }
        }
    }
}
