namespace Topology.API.DTO
{
    public sealed class ValidationResultDto
    {
        public bool Passed { get; init; }
        public IList<RuleResultDto> RuleResults { get; init; } = null!;
    }
}
