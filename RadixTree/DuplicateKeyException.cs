using System;

namespace RadixTree
{
    public class DuplicateKeyException: Exception
    {
        public DuplicateKeyException(String msg): base(msg)
        {
        
        }
    
    }
}