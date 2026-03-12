namespace Topology.Domain.Solver
{
    public sealed class ValidationResult
    {
        public bool Passed { get; init; }
        public IReadOnlyList<RuleResult> RuleResults { get; init; } = null!;
    }
}
