using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadixTree
{
    /// <summary>
    /// represents a node in the radix tree
    /// stores:
    /// a label - string that the tree holds
    /// a list of the node's subnodes - a list of other objects of this type
    /// </summary>
    public class Node
    {
        public Node()
        {
            Label = "";
            SubNodes = new List<Node>();
            id = Tree.id_counter++;
        }

        public Node(string l)
        {
            Label = l;
            SubNodes = new List<Node>();
            id = Tree.id_counter++;
        }
        public override string ToString()
        {
            return Label;
        }
        public string Label;
        public List<Node> SubNodes;
        public int id;
        public override bool Equals(object obj)
        {
            Node n = (Node)obj;
            if (n.id == id) return true;
            return false;
        }
        public override int GetHashCode()
        {
            return id;
        }
    }
    
}
