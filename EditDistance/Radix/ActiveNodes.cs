using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using RadixTree;
namespace EditDistance.Radix
{
    /// <summary>
    /// represents the active TrieNodes
    /// which noid-->activeTrieNodes
    /// </summary>
    public class ActiveTrieNodes
    {
        class pair
        {
            public TrieNode n;
            public int depth;

            public pair(TrieNode n, int depth)
            {
                this.n = n;
                this.depth = depth;
            }
        }

        public static Dictionary<TrieNode, Dictionary<TrieNode, int>> ht = new Dictionary<TrieNode, Dictionary<TrieNode, int>>();
        Trie t;
        public ActiveTrieNodes(Trie t)
        {
            
            this.t = t;
           // foreach (TrieNode n in t.getTrieNodes())
             //   ht.Add(n, new Dictionary<TrieNode, int>());
        }
        static public Dictionary<TrieNode, int> getDescendant(TrieNode n,
            Dictionary<TrieNode, int> descendents, int depth, int k)
        {

            Queue<pair> queue = new Queue<pair>();
            queue.Enqueue(new pair(n, k));
            if (k > depth)
                return descendents;
            if (!descendents.ContainsKey(n))
                descendents.Add(n, k);
            else
            {
                int kk = descendents[n];
                if (k > kk) descendents[n] = kk;
            }
            while (queue.Count > 0)
            {
                // get the first TrieNode of the queue
                pair p = queue.Dequeue();
                // add children to the queue
                if (p.depth < depth)
                {
                    foreach (TrieNode c in p.n.SubTrieNodes)
                    {
                        if (descendents.ContainsKey(c))
                        {
                            int v = descendents[c];
                            int vv = (int)Math.Min(p.depth + 1, v);

                            if (vv <= depth)
                            {
                                descendents[c] = vv;
                                queue.Enqueue(new pair(c, vv));
                            }
                        }
                        else
                        {
                            descendents.Add(c, p.depth + 1);
                        }
                    }
                }
            }
            return descendents;
        }
        static public Dictionary<TrieNode, int> getDescendant(TrieNode n, int depth, int k)
        {
            Dictionary<TrieNode, int> descendents = new Dictionary<TrieNode, int>();
            getDescendant(n, descendents, depth, k);
            return descendents;
        }

        //for inner TrieNodes
       static private void mybuildActiveTrieNodes(TrieNode parent, TrieNode TrieNode, int depth)
        {
            Dictionary<TrieNode, int> parentActiveTrieNodes = ht[parent];
            Dictionary<TrieNode, int> activeTrieNodes = ht[TrieNode];
            // deletion
            // add all p active TrieNode to this, with distance +1 if possible
            foreach (TrieNode n in parentActiveTrieNodes.Keys)
            {
                if (n == parent && depth > 0)
                    ht[TrieNode].Add(n, 1);
                else
                {
                    int l = parentActiveTrieNodes[n] + 1;
                    if (l <= depth)
                        ht[TrieNode].Add(n, l);
                }
            }

            foreach (TrieNode p in parentActiveTrieNodes.Keys)
            {
                // if p.c=c // we have a match
                int d = parentActiveTrieNodes[p];

                if (p.c == TrieNode.c)
                {
                    getDescendant(TrieNode, activeTrieNodes, depth, d);
                }

                foreach (TrieNode c in p.SubTrieNodes)
                {
                    if (c == TrieNode)
                        continue;
                    // insertion
                    if (c.c == TrieNode.c)
                    {// we have a match
                        getDescendant(c, activeTrieNodes, depth, d);
                    }
                    else
                        if (d <= depth)
                        {
                            if (activeTrieNodes.ContainsKey(c))
                            {
                                int m = Math.Min(d + 1, activeTrieNodes[c]);
                                if (m <= depth)
                                    activeTrieNodes[c] = m;
                            }
                            else
                            {
                                activeTrieNodes.Add(c, d + 1);
                            }
                        }
                }
            }
            // add myself & my Descendant
            if (activeTrieNodes.ContainsKey(TrieNode))
            {
                activeTrieNodes[TrieNode] = 0;
            }
            else
                activeTrieNodes.Add(TrieNode, 0);
            getDescendant(TrieNode, activeTrieNodes, depth, 0);

            // System.out.println("Active TrieNode " + id + ":" + Text() + ":"
            // + activeTrieNodes);
        }

        public void printActiveTrieNodes()
        {
            foreach (TrieNode n in ht.Keys)
            {
                if (!n.isleaf()) continue;
                Dictionary<TrieNode, int> v = ht[n];
                Console.Write(n.n.Label + ":");
                foreach (TrieNode vv in v.Keys)
                {
                    Console.Write(vv + " " + v[vv] + ",");
                }
                Console.WriteLine();
            }
        }
        public static void BuildActiveNodes(TrieNode n, int depth){
            TrieNode r=n.parent;
            mybuildActiveTrieNodes(r, n, depth);
        }
        
        public void buildActiveTrieNode(int depth)
        {
            Queue<TrieNode> queue = new Queue<TrieNode>();
            TrieNode root = t.root;
            queue.Enqueue(root);
            Dictionary<TrieNode, int> activeTrieNodes = new Dictionary<TrieNode, int>();
            
            activeTrieNodes.Add(root, 0);
            getDescendant(root, activeTrieNodes, depth, 0);
            ht.Add(root, activeTrieNodes) ;
            
            while (queue.Count > 0)
            {
                TrieNode r = queue.Dequeue();
                foreach (TrieNode n in r.SubTrieNodes)
                {
                    ht[n] = new Dictionary<TrieNode, int>();
                    queue.Enqueue(n);
                    mybuildActiveTrieNodes(r, n, depth);
                }
            }
        }
    }
}
