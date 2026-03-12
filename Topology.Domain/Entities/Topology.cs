namespace Topology.Domain.Entities;

public sealed class Topology
{
    public IReadOnlyList<Node> Nodes { get; init; } = null!;
    public IReadOnlyList<Link> Links { get; init; } = null!;
}