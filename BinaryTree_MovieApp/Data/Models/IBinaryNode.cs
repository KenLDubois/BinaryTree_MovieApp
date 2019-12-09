using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryTree_MovieApp.Data.Models
{
    public interface IBinaryNode<T> : IComparable<T>, IEquatable<T>
    {
        public T Left { get; set; }
        public T Right { get; set; }

        public abstract bool MeetsSearchCriteria(T searchTerm);
    }
}
