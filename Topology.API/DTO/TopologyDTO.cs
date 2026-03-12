namespace Topology.API.DTO;

public sealed class TopologyDto
{
    public IList<NodeDto> Nodes { get; init; } = null!;
    public IList<LinkDto> Links { get; init; } = null!;
}