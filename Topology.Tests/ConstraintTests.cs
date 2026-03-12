using System.Collections.ObjectModel;
using Topology.Domain.Constraints;
using Topology.Domain.Entities;


namespace Topology.Tests
{
    /// <summary>
    /// Tests at the constraint level
    /// </summary>
    [TestClass]
    public class ConstraintTests
    {
        private const string DataLink = "data";
        private const string ControlLink = "control";
        private const string ReplicateLink = "replicate";

        private static Node CreateNode(string type, List<string>? capabilities = null, Dictionary<string, object>? attributes = null)
        {
            return new Node
            {
                Id = DateTime.Now.Ticks.ToString(),
                Type = type,
                Capabilities = capabilities?.AsReadOnly() ?? [],
                Attributes = attributes?.AsReadOnly() ?? new ReadOnlyDictionary<string, object>(new Dictionary<string, object>())
            };
        }


        private static Node CreateComputeNode()
        {
            return CreateNode("compute");
        }

        private static Node CreateStorageNode()
        {
            return CreateNode("storage");
        }

        private static Node CreateLinearNode()
        {
            return CreateNode("linear");
        }

        public static Link LinkNodes(Node node1, Node node2, string kind)
        {
            return new Link
            {
                From = node1.Id,
                To = node2.Id,
                Kind = kind
            };
        }

        [TestMethod]
        public void Test_NoOrphanNodes_Passes()
        {
            // arrange
            var node1 = CreateComputeNode();
            var node2 = CreateStorageNode();

            var link = LinkNodes(node1, node2, DataLink);
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
            var node1 = CreateComputeNode();
            var node2 = CreateStorageNode();
            var node3 = CreateComputeNode();
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
            var node1 = CreateLinearNode();
            var node2 = CreateLinearNode();
            var node3 = CreateLinearNode();

            var link1 = LinkNodes(node1, node2,ControlLink);
            var link2 = LinkNodes(node2, node3, ControlLink);
            var link3 = LinkNodes(node3, node1, ControlLink);

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
            Assert.IsTrue(result.Messages.Any(x=>x.StartsWith("Nodes of type linear must not form cycles",StringComparison.InvariantCulture)));
        }


        [TestMethod]
        public void Test_LinearNodesNotCyclic_PassesWhenNotLooping()
        {
            // arrange
            var node1 = CreateLinearNode();
            var node2 = CreateLinearNode();
            var node3 = CreateLinearNode();
            var node4 = CreateLinearNode();

            var link1 = LinkNodes(node1, node2, ControlLink);
            var link2 = LinkNodes(node2, node3, ControlLink);
            var link3 = LinkNodes(node3, node4, ControlLink);

            var topology = new Domain.Entities.Topology
            {
                Nodes = [node1, node2, node3,node4],
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
            var node1 = CreateComputeNode();

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
        [DataRow("storage","capacity")]
        [DataRow("compute", "cores")]
        [DataRow("secure", "encryption")]
        public void Test_CapabilityWithoutNoAttributes(string capability, string requiredAttrib)
        {
            // arrange
            var node1 = CreateNode("test",[capability]);

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
            Assert.IsTrue(result.Messages.Any(x=>x.Equals($"Node {node1.Id} has capability {capability} but no {requiredAttrib} attribute")));
        }

        [TestMethod]
        [DataRow("storage", "capacity")]
        [DataRow("compute", "cores")]
        [DataRow("secure", "encryption")]
        public void Test_CapabilityWithoutRequiredAttribute(string capability, string requiredAttrib)
        {
            // arrange
            var node1 = CreateNode("test", [capability], new Dictionary<string, object> { { "dummy", "1" } });

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
            Assert.IsTrue(result.Messages.Any(x => x.Equals($"Node {node1.Id} has capability {capability} but no {requiredAttrib} attribute")));
        }

        [TestMethod]
        [DataRow("storage", "capacity")]
        [DataRow("compute", "cores")]
        [DataRow("secure", "encryption")]
        public void Test_CapabilityWithRequiredAttribute(string capability, string requiredAttrib)
        {
            // arrange
            var node1 = CreateNode("test", [capability],new Dictionary<string, object>{{requiredAttrib,"1"}});

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
            Assert.IsFalse(result.Messages.Any(x => x.Equals($"Node {node1.Id} has capability {capability} but no {requiredAttrib} attribute")));
        }

        // link tests

        [TestMethod]
        public void Test_LinkKindInvalid()
        {
            // arrange
            var node1 = CreateComputeNode();
            var node2 = CreateComputeNode();
            var link = LinkNodes(node1, node2, DataLink);

            var topology = new Domain.Entities.Topology
            {
                Nodes = [node1,node2],
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
        public void Test_LinkKindValid()
        {
            // arrange
            var node1 = CreateComputeNode();
            var node2 = CreateComputeNode();
            var link = LinkNodes(node1, node2, ControlLink);

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
            var node1 = CreateComputeNode();
            var node2 = CreateComputeNode();
            var node3 = CreateStorageNode();
            var node4 = CreateComputeNode();
            var node5 = CreateComputeNode();
            

            var link1 = LinkNodes(node1, node2, ControlLink);
            var link2 = LinkNodes(node1, node3, DataLink);
            var link3 = LinkNodes(node1, node4, ControlLink);
            var link4 = LinkNodes(node1, node5, ControlLink);

            var topology = new Domain.Entities.Topology
            {
                Nodes = [node1, node2, node3,node4,node5],
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
            var node1 = CreateComputeNode();
            var node2 = CreateComputeNode();
            var node3 = CreateStorageNode();
            var node4 = CreateComputeNode();


            var link1 = LinkNodes(node1, node2, ControlLink);
            var link2 = LinkNodes(node1, node3, DataLink);
            var link3 = LinkNodes(node1, node4, ControlLink);
            

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
}