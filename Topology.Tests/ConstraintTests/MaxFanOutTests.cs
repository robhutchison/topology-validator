using Topology.Domain.Constraints;
using Topology.Domain.Entities;


namespace Topology.Tests.ConstraintTests;

[TestClass]
public class MaxFanOutTests
{
    [TestMethod]
    public void Test_MaxFanOutForCompute_TooManyLinks()
    {
        // arrange
        var node1 = Shared.CreateComputeNode();
        var node2 = Shared.CreateComputeNode();
        var node3 = Shared.CreateStorageNode();
        var node4 = Shared.CreateComputeNode();
        var node5 = Shared.CreateComputeNode();


        var link1 = Shared.LinkNodes(node1, node2, Shared.ControlLink);
        var link2 = Shared.LinkNodes(node1, node3, Shared.DataLink);
        var link3 = Shared.LinkNodes(node1, node4, Shared.ControlLink);
        var link4 = Shared.LinkNodes(node1, node5, Shared.ControlLink);

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2, node3, node4, node5],
            Links = [link1, link2, link3, link4]
        };

        // act
        var sut = new MaxFanOut();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsNotEmpty(result.Messages);
        Assert.IsTrue(result.Messages.Any(x =>
            x.Equals($"Node {node1.Id} has 4 outgoing links")));
    }

    [TestMethod]
    public void Test_MaxFanOutForCompute_Passes()
    {
        // arrange
        var node1 = Shared.CreateComputeNode();
        var node2 = Shared.CreateComputeNode();
        var node3 = Shared.CreateStorageNode();
        var node4 = Shared.CreateComputeNode();


        var link1 = Shared.LinkNodes(node1, node2, Shared.ControlLink);
        var link2 = Shared.LinkNodes(node1, node3, Shared.DataLink);
        var link3 = Shared.LinkNodes(node1, node4, Shared.ControlLink);


        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2, node3, node4],
            Links = [link1, link2, link3]
        };

        // act
        var sut = new MaxFanOut();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsEmpty(result.Messages);
    }
}
