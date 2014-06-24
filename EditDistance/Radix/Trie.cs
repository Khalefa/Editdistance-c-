using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadixTree;
namespace EditDistance.Radix
{
    public class Trie
    {
        Tree t;
        public Trie(Tree t)
        {
            this.t = t;
            
        }
        public TrieNode root
        {
            get{
                return new TrieNode(t.root);
            }
        }

        
    }
}
