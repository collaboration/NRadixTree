using System;
using System.Collections.Generic;

namespace RadixTree
{
    public class RadixTree<T> : Tree<T>
    {
        public void Insert(string key, T value)
        {
            throw new NotImplementedException();
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

        public List<T> SearchPrefix(string prefix, int recordLimit)
        {
            throw new NotImplementedException();
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