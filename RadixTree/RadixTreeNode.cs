using System;
using System.Collections.Generic;

//This class needs to be deleted
namespace RadixTree
{
    public class RadixTreeNode<T>
    {
        public string Key { get; set; }

        public List<RadixTreeNode<T>> Children { get; private set; }

        public bool Real { get; private set; }

        public T Value { get; private set; }

        public RadixTreeNode()
        {
            Key = "";
            Children = new List<RadixTreeNode<T>>();
            Real = false;
        }


        public int NumberOfMatchingCharacters(String key)
        {
            int numberOfMatchingCharacters = 0;
            while (numberOfMatchingCharacters < key.Length && numberOfMatchingCharacters < Key.Length)
            {
//                if (key.CharAt(numberOfMatchingCharacters) != Key.charAt(numberOfMatchingCharacters))
//                {
//                    break;
//                }
                numberOfMatchingCharacters++;
            }
            return numberOfMatchingCharacters;
        }

        public override String ToString()
        {
            return Key;
        }
    }
}
