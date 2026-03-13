using Topology.Domain.Constraints;
using Topology.Domain.Entities;


namespace Topology.Tests;

/// <summary>
/// Tests at the constraint level
/// </summary>
[TestClass]
public class ConstraintTests
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


    // linear tests

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


    // capability required attributes 

    [TestMethod]
    public void Test_NodeWithoutCapabilityOrRequiredAttribute()
    {
        // arrange
        var node1 = Shared.CreateComputeNode();

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1]
        };

        // act
        var sut = new CapabilityRequiredAttribute();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsEmpty(result.Messages);
    }

    [TestMethod]
    [DataRow("storage", "capacity")]
    [DataRow("compute", "cores")]
    [DataRow("secure", "encryption")]
    public void Test_CapabilityWithoutNoAttributes(string capability, string requiredAttrib)
    {
        // arrange
        var node1 = Shared.CreateNode("test", [capability]);

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1]
        };

        // act
        var sut = new CapabilityRequiredAttribute();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsNotEmpty(result.Messages);
        Assert.IsTrue(result.Messages.Any(x =>
            x.Equals($"Node {node1.Id} has capability {capability} but no {requiredAttrib} attribute")));
    }

    [TestMethod]
    [DataRow("storage", "capacity")]
    [DataRow("compute", "cores")]
    [DataRow("secure", "encryption")]
    public void Test_CapabilityWithoutRequiredAttribute(string capability, string requiredAttrib)
    {
        // arrange
        var node1 = Shared.CreateNode("test", [capability], new Dictionary<string, object>
        {
            { "dummy", "1" }
        });

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1]
        };

        // act
        var sut = new CapabilityRequiredAttribute();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsNotEmpty(result.Messages);
        Assert.IsTrue(result.Messages.Any(x =>
            x.Equals($"Node {node1.Id} has capability {capability} but no {requiredAttrib} attribute")));
    }

    [TestMethod]
    [DataRow("storage", "capacity")]
    [DataRow("compute", "cores")]
    [DataRow("secure", "encryption")]
    public void Test_CapabilityWithRequiredAttribute(string capability, string requiredAttrib)
    {
        // arrange
        var node1 = Shared.CreateNode("test", [capability],
            new Dictionary<string, object>
            {
                { requiredAttrib, "1" }
            });

        var topology = new Domain.Entities.Topology
        {
            Nodes = [node1]
        };

        // act
        var sut = new CapabilityRequiredAttribute();
        var result = sut.Evaluate(topology);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Passed);
        Assert.IsNotEmpty(result.RuleName);
        Assert.IsEmpty(result.Messages);
        Assert.IsFalse(result.Messages.Any(x =>
            x.Equals($"Node {node1.Id} has capability {capability} but no {requiredAttrib} attribute")));
    }

    // link tests

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

    // max fan out tests

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