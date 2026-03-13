namespace Topology.Domain.Entities
{
    /// <summary>
    /// Represents a node in the system
    /// </summary>
    public sealed class Node
    {
        public string Id { get; init; } = null!;
        public string Type { get; init; } = null!;
        public IReadOnlyDictionary<string, object> Attributes { get; init; } = null!;
        public IReadOnlyList<string> Capabilities { get; init; } = null!;
    }
}