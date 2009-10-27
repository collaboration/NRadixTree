using System;
using System.Collections.Generic;

namespace RadixTree
{
    public interface Tree<T>
    {
        void Insert(String key, T value);
        bool Delete(String key);
        T Find(String key);
        bool Contains(String key);
        List<T> Search(string prefix);
        long Size();
        String Complete(String prefix);
    }
}