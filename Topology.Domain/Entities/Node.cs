namespace Topology.Domain.Entities
{
    public sealed class Node
    {
        public string Id { get; init; } = null!;
        public string Type { get; init; } = null!;
        public IReadOnlyDictionary<string, object> Attributes { get; init; } = null!;
        public IReadOnlyList<string> Capabilities { get; init; } = null!;
    }
}
