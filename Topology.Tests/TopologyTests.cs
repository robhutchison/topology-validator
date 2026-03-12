using Topology.Application;
using Topology.Domain.Constraints;

namespace Topology.Tests;

using Domain.Entities;

/// <summary>
/// These are tests to cover Topology level constraints
/// </summary>
[TestClass]
public sealed class TopologyTests
{
    private TopologyValidator CreateValidator()
    {
        return new TopologyValidator([new NoOrphanNodes(), new NoCyclicLinks(), new CapabilityRequiredAttribute(),new LinkKind(), new MaxFanOut()]);
    }

    /// <summary>
    /// Every node must appear in at least 1 link (either From or To)
    /// </summary>
    [TestMethod]
    public void Test_TopologyHasNoOrphanNodes()
    {
        // arrange
        // create a topology with no orphan nodes

        var node1 = Shared.CreateComputeNode();
        var node2 = Shared.CreateStorageNode();

        var link = Shared.LinkNodes(node1, node2, Shared.DataLink);
        var topology = new Topology
        {
            Nodes = [node1, node2],
            Links = [link]
        };

        // act
        // pass the topology to the validator 
        var sut = CreateValidator();
        var result = sut.ValidateTopology(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Passed);
        Assert.IsNotEmpty(result.RuleResults);
        Assert.HasCount(5,result.RuleResults);
        Assert.HasCount(5, result.RuleResults.Where(x=>x.Passed));
    }

    /// <summary>
    /// Every node must appear in at least 1 link (either From or To)
    /// </summary>
    [TestMethod]
    public void Test_TopologyHasOrphanNodes()
    {
        // arrange
        // create a topology with no orphan nodes

        var node1 = Shared.CreateComputeNode();
        var node2 = Shared.CreateStorageNode();
        var node3 = Shared.CreateStorageNode();

        var link = Shared.LinkNodes(node1, node2, Shared.DataLink);
        var topology = new Topology
        {
            Nodes = [node1, node2, node3],
            Links = [link]
        };

        // act
        // pass the topology to the validator 
        var sut = CreateValidator();
        var result = sut.ValidateTopology(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Passed);
        Assert.IsNotEmpty(result.RuleResults);
        Assert.HasCount(5, result.RuleResults);
        Assert.HasCount(1, result.RuleResults.Where(x => x is { Passed: false, RuleName: nameof(NoOrphanNodes) }), "only 1 rule should fail");
    }
}