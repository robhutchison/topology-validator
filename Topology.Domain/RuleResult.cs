namespace Topology.Domain;

public sealed class RuleResult
{
    public bool Passed { get; init; }
    public string RuleName { get; init; } = null!;
    public IReadOnlyList<string> Messages { get; init; } = [];
}