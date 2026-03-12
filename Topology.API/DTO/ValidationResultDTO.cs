namespace Topology.API.DTO
{
    public sealed class ValidationResultDto
    {
        public bool Passed { get; init; }
        public IReadOnlyList<RuleResultDto> RuleResults { get; init; } = null!;
    }
}
