using Topology.Domain.Constraints;

namespace Topology.Tests.ConstraintTests;

[TestClass]
public class LinkKindTests
{
    [TestMethod]
    public void Test_LinkKind_DataInvalid()
    {
        // arrange
        var node1 = Shared.CreateComputeNode();
        var node2 = Shared.CreateComputeNode();
        var link = Shared.LinkNodes(node1, node2, Shared.DataLink);

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2],
            Links = [link]
        };

        // act
        var sut = new LinkKind();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsNotEmpty(result.Messages);
        Assert.IsTrue(result.Messages.Any(x =>
            x.Equals($"A {link.Kind} link from a {node1.Type} to a {node2.Type} is invalid")));
    }

    [TestMethod]
    public void Test_LinkKind_ControlInvalid()
    {
        // arrange
        var node1 = Shared.CreateComputeNode();
        var node2 = Shared.CreateStorageNode();
        var link = Shared.LinkNodes(node1, node2, Shared.ControlLink);

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2],
            Links = [link]
        };

        // act
        var sut = new LinkKind();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsNotEmpty(result.Messages);
        Assert.IsTrue(result.Messages.Any(x =>
            x.Equals($"A {link.Kind} link from a {node1.Type} to a {node2.Type} is invalid")));
    }

    [TestMethod]
    public void Test_LinkKind_ReplicateInvalid()
    {
        // arrange
        var node1 = Shared.CreateComputeNode();
        var node2 = Shared.CreateComputeNode();
        var link = Shared.LinkNodes(node1, node2, Shared.ReplicateLink);

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2],
            Links = [link]
        };

        // act
        var sut = new LinkKind();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsNotEmpty(result.Messages);
        Assert.IsTrue(result.Messages.Any(x =>
            x.Equals($"A {link.Kind} link from a {node1.Type} to a {node2.Type} is invalid")));
    }

    [TestMethod]
    public void Test_LinkKind_DataValid()
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
        var sut = new LinkKind();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsEmpty(result.Messages);
    }

    [TestMethod]
    public void Test_LinkKind_ControlValid()
    {
        // arrange
        var node1 = Shared.CreateComputeNode();
        var node2 = Shared.CreateComputeNode();
        var link = Shared.LinkNodes(node1, node2, Shared.ControlLink);

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2],
            Links = [link]
        };

        // act
        var sut = new LinkKind();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsEmpty(result.Messages);
    }

    [TestMethod]
    public void Test_LinkKind_ReplicateValid()
    {
        // arrange
        var node1 = Shared.CreateStorageNode();
        var node2 = Shared.CreateStorageNode();
        var link = Shared.LinkNodes(node1, node2, Shared.ReplicateLink);

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1, node2],
            Links = [link]
        };

        // act
        var sut = new LinkKind();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsEmpty(result.Messages);
    }
}