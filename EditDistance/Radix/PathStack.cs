using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistance.Radix
{
    //TODO use radix tree directly
        
public class PathStack  {

    Trie trie;
	public PathStack(Trie t, int depth) {
        trie=t;
        
		DateTime s=DateTime.Now;
		DFS(depth);
		Global.time=DateTime.Now-s;
    }
    	
	// Implement a straightforward DFS algorithm on the trie
	public void DFS(int depth) {
		Stack<TrieNode> stack = new Stack<TrieNode>();
        
        TrieNode root=trie.root;
		stack.Push(root);
        ActiveTrieNodes.ht.Add(root, new Dictionary<TrieNode,int>());
		ActiveTrieNodes.getDescendant(root,ActiveTrieNodes.ht[root], depth, 0);

		while (stack.Count>0) {
			// pop the element from stack
			TrieNode n = stack.Pop();
			// System.out.println(n.id-1);

			foreach (TrieNode x in n.children) {
				stack.Push(x);
                x.parent = n;
                ActiveTrieNodes.ht.Add(x, new Dictionary<TrieNode, int>());
				ActiveTrieNodes. BuildActiveNodes(x,depth);
			}

           if (!n.isleaf())
                ActiveTrieNodes.ht.Remove(n);
		}
	}

	/**
	 * @param args
	 */
	/*public static void main(String[] args) {
		// TODO Auto-generated method stub
		long startTime = System.currentTimeMillis();
		TriePathStack r = new TriePathStack("c:\\data\\word.format", 50000, 1);
		System.out.println("-----------------------------------------------");
		// r.DFS(1);
		HashSet<String> m = r.Matches();
		System.out.println(m.size());
		long endTime = System.currentTimeMillis();
		System.out.println(endTime - startTime);
		// for (String s : m) {
		// System.out.println(s);
		// }

		// r.Stats();
	}*/

}
    }


