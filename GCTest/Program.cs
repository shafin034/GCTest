using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using GCTest;
using System.Runtime;


BenchmarkRunner.Run<NodeBenchMarker>();


[MemoryDiagnoser]
public class NodeBenchMarker
{
    [Benchmark]
    public void BenchmarkNodes()
    {
        int NodeCount = 1000;
        if (Environment.GetCommandLineArgs().Count() == 3)
        {
            NodeCount = int.Parse(Environment.GetCommandLineArgs()[0].ToString());
        }

        NodeFactory nodeFactory = new NodeFactory(NodeCount);
        var graph = nodeFactory.MakeGraph();
        nodeFactory.DeleteNodes();
    }

    [Benchmark]
    public void BenchmarkNodesParallel()
    {
        int NodeCountParallel = 1000;
        int pThread = 20;
        if (Environment.GetCommandLineArgs().Count() == 3)
        {
            NodeCountParallel = int.Parse(Environment.GetCommandLineArgs()[1].ToString());
            pThread = int.Parse(Environment.GetCommandLineArgs()[2].ToString());
        }

        Parallel.For(0, pThread, (i) =>
        {
            NodeFactory nodeFactory = new NodeFactory(NodeCountParallel);
            var graph = nodeFactory.MakeGraph();
            nodeFactory.DeleteNodes();
        });
    }
}