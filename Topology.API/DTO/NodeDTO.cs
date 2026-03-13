namespace Topology.API.DTO
{
    public sealed class NodeDto
    {
        public string Id { get; init; } = null!;
        public string Type { get; init; } = null!;
        public IDictionary<string, object> Attributes { get; init; } = null!;
        public IList<string> Capabilities { get; init; } = null!;
    }
}