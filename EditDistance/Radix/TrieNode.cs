using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadixTree;
namespace EditDistance.Radix
{
    public class TrieNode
    {
        public char c;
        public Node n;
        public TrieNode parent;
        int indx;
        public List<TrieNode> children {
        get
            { return SubTrieNodes;}
        }

        public List<TrieNode> SubTrieNodes
        {
            get
            {
                List<TrieNode> l = new List<TrieNode>();
                if (indx < n.Label.Length-1)
                {
                    TrieNode a = new TrieNode(n, indx);
                    a.parent = this;
                    l.Add(a);
                }
                else
                {
                    foreach (Node m in n.SubNodes)
                    {
                        l.Add(new TrieNode(m));
                    }
                }
                return l;
            }
        }
        public TrieNode(Node n)
        {
            this.n = n;
            indx = 0;
            if (n.Label.Length == 0)
            {
                c = '$';
            }
            else
            c = n.Label[indx];
        }
        //a node is a leaf if it has no children and indx is re
        public bool isleaf(){
            if(SubTrieNodes.Count>0) return false;
            if(indx <n.Label.Length-1 ) return false;
            return true;
        }
        public TrieNode(Node n, int indx)
        {
            this.n = n;
            this.indx = indx + 1;
            c = n.Label[this.indx];            
        }
       public override string ToString()
        {
            return n.Label+":"+indx;
        }
        public override bool Equals(Object obj)
        {
            TrieNode t = (TrieNode)obj;
            if( (t.n.id==n.id) && (t.indx==indx)) return true;
            return false;
        }
        public override int GetHashCode()
        {
            string s=n.id+":"+indx;
            return s.GetHashCode();
        }
    }
}
