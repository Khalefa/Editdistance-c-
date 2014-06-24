using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistance.Radix
{
    class engine
    {
        static public void run(ArrayList words)
        {
            DateTime t = DateTime.Now;
            RadixTree.Tree tree=new RadixTree.Tree();
            
            foreach (string word in words)
            {
                tree.Insert(word);
            }
            Trie trie = new Trie(tree);
            //PathStack pst=new PathStack(trie,1);
            
           /* ActiveTrieNodes an = new ActiveTrieNodes(trie);
            an.buildActiveTrieNode(1);
            an.printActiveTrieNodes();*/
            //foreach (TrieNode n in ActiveTrieNodes.ht.Keys)
            //{
            //    Dictionary<TrieNode, int> v = ActiveTrieNodes.ht[n];
            //    Console.Write(n + " ");
            //    foreach (TrieNode vv in v.Keys)
            //    {
            //        Console.Write("(" + vv + ":" + v[vv] + ")");
            //    }
            //    Console.WriteLine();
            //}
            TimeSpan ts = DateTime.Now - t;
            Console.WriteLine(ts);
        }
    }
}
