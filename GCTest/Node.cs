using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTest
{
    public class Node
    {
        public HashSet<Node> Childs { get; set; } = new HashSet<Node> { };

        public int Id { get; set; }

        public Node(int id)
        {
            Id = id;
        }
    }
}
