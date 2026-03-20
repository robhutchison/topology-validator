using Topology.Domain.Constraints;

namespace Topology.Tests.ConstraintTests;

[TestClass]
public class NoCyclicLinksTests
{
    [TestMethod]
    public void Test_LinearNodesNotCyclic_FailsForLoop()
    {
        // arrange
        var node1 = Shared.CreateLinearNode();
        var node2 = Shared.CreateLinearNode();
        var node3 = Shared.CreateLinearNode();

        var link1 = Shared.LinkNodes(node1, node2, Shared.ControlLink);
        var link2 = Shared.LinkNodes(node2, node3, Shared.ControlLink);
        var link3 = Shared.LinkNodes(node3, node1, Shared.ControlLink);

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2, node3],
            Links = [link1, link2, link3]
        };

        // act

        var sut = new NoCyclicLinks();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsNotEmpty(result.Messages);
        Assert.IsTrue(result.Messages.Any(x =>
            x.StartsWith("Nodes of type linear must not form cycles", StringComparison.InvariantCulture)));
    }

    [TestMethod]
    public void Test_LinearNodesNotCyclic_PassesWhenNotLooping()
    {
        // arrange
        var node1 = Shared.CreateLinearNode();
        var node2 = Shared.CreateLinearNode();
        var node3 = Shared.CreateLinearNode();
        var node4 = Shared.CreateLinearNode();

        var link1 = Shared.LinkNodes(node1, node2, Shared.ControlLink);
        var link2 = Shared.LinkNodes(node2, node3, Shared.ControlLink);
        var link3 = Shared.LinkNodes(node3, node4, Shared.ControlLink);

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2, node3, node4],
            Links = [link1, link2, link3]
        };

        // act

        var sut = new NoCyclicLinks();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsEmpty(result.Messages);
    }
}