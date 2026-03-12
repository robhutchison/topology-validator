namespace Topology.Domain.Entities;

/// <summary>
/// Represents a link between 2 nodes and the type of link
/// </summary>
public sealed class Link
{
    public string From { get; init; } = null!;
    public string To { get; init; } = null!;
    public string Kind { get; init; } = null!;
}