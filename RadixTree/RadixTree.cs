using System;
using System.Collections.Generic;

namespace RadixTree
{
    public class RadixTree<T> : Tree<T>
    {
        public void Insert(string key, T value)
        {
//            new List<RadixTreeNode>
        }

        public bool Delete(string key)
        {
            throw new NotImplementedException();
        }

        public T Find(string key)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string key)
        {
            throw new NotImplementedException();
        }

        public List<T> Search(string prefix)
        {
            return new List<T>();
        }

        public long Size()
        {
            throw new NotImplementedException();
        }

        public string Complete(string prefix)
        {
            throw new NotImplementedException();
        }
    }    

}