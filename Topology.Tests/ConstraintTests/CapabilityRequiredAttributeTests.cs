using Topology.Domain.Constraints;

namespace Topology.Tests.ConstraintTests;

[TestClass]
public class CapabilityRequiredAttributeTests
{
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
}