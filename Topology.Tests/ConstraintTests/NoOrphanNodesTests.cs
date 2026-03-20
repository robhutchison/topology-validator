using Topology.Domain.Constraints;
using Topology.Domain.Entities;

namespace Topology.Tests.ConstraintTests;

[TestClass]
public class NoOrphanNodesTests
{
    [TestMethod]
    public void Test_NoOrphanNodes_Passes()
    {
        // arrange
        var node1 = Shared.CreateComputeNode();
        var node2 = Shared.CreateStorageNode();

        var link = Shared.LinkNodes(node1, node2, Shared.DataLink);
        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2],
            Links = [link]
        };
        // act

        var sut = new NoOrphanNodes();
        var result = sut.Evaluate(topology);
        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsEmpty(result.Messages);
    }

    [TestMethod]
    public void Test_NoOrphanNodes_Fails()
    {
        // arrange
        var node1 = Shared.CreateComputeNode();
        var node2 = Shared.CreateStorageNode();
        var node3 = Shared.CreateComputeNode();
        var link = new Link
        {
            From = node1.Id,
            To = node2.Id,
            Kind = "data"
        };
        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2, node3],
            Links = [link]
        };
        // act

        var sut = new NoOrphanNodes();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsNotEmpty(result.Messages);
    }
}