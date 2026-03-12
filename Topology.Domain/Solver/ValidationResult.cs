namespace Topology.Domain.Solver
{
    /// <summary>
    /// Represents the result of validation of the topology by all active constraints
    /// </summary>
    public sealed class ValidationResult
    {
        public bool Passed { get; init; }
        public IReadOnlyList<RuleResult> RuleResults { get; init; } = null!;
    }
}
