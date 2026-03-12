namespace Topology.Domain.Entities;

/// <summary>
/// Represents a system topology including all the nodes and how they are connected
/// </summary>
public sealed class Topology
{
    public IReadOnlyList<Node> Nodes { get; init; } = null!;
    public IReadOnlyList<Link> Links { get; init; } = null!;
}