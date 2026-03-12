using System.Collections.ObjectModel;
using Topology.Domain.Entities;

namespace Topology.Tests;

public class Shared
{
    public const string DataLink = "data";
    public const string ControlLink = "control";
    public const string ReplicateLink = "replicate";

    public static Node CreateNode(string type, List<string>? capabilities = null, Dictionary<string, object>? attributes = null)
    {
        return new Node
        {
            Id = DateTime.Now.Ticks.ToString(),
            Type = type,
            Capabilities = capabilities?.AsReadOnly() ?? [],
            Attributes = attributes?.AsReadOnly() ?? new ReadOnlyDictionary<string, object>(new Dictionary<string, object>())
        };
    }

    public static Node CreateComputeNode()
    {
        return CreateNode("compute");
    }

    public static Node CreateStorageNode()
    {
        return CreateNode("storage");
    }

    public static Node CreateLinearNode()
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
}