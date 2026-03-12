namespace Topology.Domain.Entities;

public sealed class Link
{
    public string From { get; init; } = null!;
    public string To { get; init; } = null!;
    public string Kind { get; init; } = null!;
}