namespace Topology.API.DTO;

public sealed class LinkDto
{
    public string From { get; init; } = null!;
    public string To { get; init; } = null!;
    public string Kind { get; init; } = null!;
}