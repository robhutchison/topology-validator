using Topology.Application;

namespace Topology.Tests;

using Domain.Entities;

/// <summary>
/// These are tests to cover Topology level constraints
/// </summary>
[TestClass]
public sealed class TopologyTests
{
    /// <summary>
    /// Every node must appear in at least 1 link (either From or To)
    /// </summary>
    [TestMethod]
    public void Test_TopologyHasNoOrphanNodes()
    {
        // arrange
        // create a topology with no orphan nodes

        // create a helper to generate these for us
        var node1 = new Node { Id = "node1", Type = "compute"};
        var node2 = new Node { Id = "node2", Type = "storage" };

        var link = new Link
        {
            From = node1.Id,
            To = node2.Id,
            Kind = "data"
        };
        var topology = new Topology
        {
            Nodes = [node1, node2],
            Links = [link]
        };

        // act
        // pass the topology to the validator 
        var sut = new TopologyValidator();
        var result = sut.ValidateTopology(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Passed);
        Assert.IsNotEmpty(result.RuleResults);
    }
    // report if the topology has validation error for orphan nodes.

    /// <summary>
    /// Check that Linear nodes do not form a cyclic loop
    /// A -> B -> C -> A
    /// </summary>
    [TestMethod]
    public void Test_LinearNodesAreNotCyclic()
    {
        // arrange

        // act

        // assert
    }

    // todo: this needs to separate tests, and we need a validation rule for each rather than doing it like this.


    [TestMethod]
    [DataRow("storage", "capacity", DisplayName = "Storage nodes have a capacity")]
    [DataRow("compute", "cores", DisplayName = "Compute nodes have a number of cores")]
    [DataRow("secure", "encryption", DisplayName = "Secure nodes specify encryption type")]
    public void Test_CapabilityHasRequiredAttribute(string nodeType, string attributeName)
    {
        // arrange
        // act
        // hit the constraint directly for each ? make this 
        // assert
    }
}