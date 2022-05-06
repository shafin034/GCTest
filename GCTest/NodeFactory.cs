using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTest
{
    [MemoryDiagnoser]
    public class NodeFactory
    {
        public NodeFactory(int nodeCount)
        {
            if (nodeCount <= 0)
                throw new ArgumentOutOfRangeException("Should be greater than 0");

            NodeCount = nodeCount;
        }

        public Dictionary<int, Node> NodeIdToNode { get; set; } = new Dictionary<int, Node>();
        public Dictionary<Node, HashSet<Node>> NodeToParentNodes { get; set; } = new Dictionary<Node, HashSet<Node>>();

        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        public int NodeCount { get; set; }

        public Node MakeGraph()
        {
            Node root = null;

            for (int i = 0; i < NodeCount; i++)
            {
                Node node = GetIfPresent(i);
                if (root is null) root = node;

                for (int j = 0; j < NodeCount; j++)
                {
                    if (i != j)
                    {
                        var currNode = GetIfPresent(j);
                        node.Childs.Add(currNode);
                        if (NodeToParentNodes.ContainsKey(currNode))
                        {
                            NodeToParentNodes[currNode].Add(node);
                        }
                        else
                        {
                            NodeToParentNodes.Add(currNode, new HashSet<Node>() { node });
                        }
                    }
                }
            }

            return root;
        }

        public void DeleteNodes()
        {
            var nodeToDeleteStart = _random.Next(10, NodeCount);
            var nodeToDeleteEnd = _random.Next(10, NodeCount);
            var min = Math.Min(nodeToDeleteStart, nodeToDeleteEnd);
            var max = Math.Max(nodeToDeleteStart, nodeToDeleteEnd);
            for (int i = min; i < max; i++)
            {
                var node = GetIfPresent(i);
                if (node is not null)
                {
                    node.Childs.Clear();
                    if (NodeToParentNodes.ContainsKey(node))
                    {
                        var parents = NodeToParentNodes[node];
                        foreach (var parent in parents)
                        {
                            parent.Childs.Remove(node);
                        }
                    }
                    NodeIdToNode.Remove(i);
                    node = null;
                }
            }
        }


        private Node GetIfPresent(int id)
        {
            if (NodeIdToNode.ContainsKey(id))
                return NodeIdToNode[id];
            else
            {
                var node = new Node(id);
                UpdateDictionary(node);
                return node;
            };
        }

        private void UpdateDictionary(Node node)
        {
            NodeIdToNode.Add(node.Id, node);
        }
    }
}
