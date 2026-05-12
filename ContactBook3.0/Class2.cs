using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewContactBook
{
    // Disjoint Set Union (Union-Find) data structure.
    // Groups contacts that represent the same person so they can be merged.
    public class DSU
    {
        private readonly int[] _parent;

        // Initialize: every element is its own root
        public DSU(int size)
        {
            _parent = new int[size];
            for (int i = 0; i < size; i++)
                _parent[i] = i;
        }

        // Find the root of element x with path compression
        public int FindRoot(int x)
        {
            if (_parent[x] != x)
                _parent[x] = FindRoot(_parent[x]); // path compression
            return _parent[x];
        }

        // Union the groups containing a and b
        public void Union(int a, int b)
        {
            int rootA = FindRoot(a);
            int rootB = FindRoot(b);
            if (rootA != rootB)
                _parent[rootB] = rootA;
        }
    }
}

