namespace Topology.API.DTO;

public sealed class TopologyDto
{
    public IReadOnlyList<NodeDto> Nodes { get; init; } = null!;
    public IReadOnlyList<LinkDto> Links { get; init; } = null!;
}