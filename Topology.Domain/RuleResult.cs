namespace Topology.Domain;

/// <summary>
/// Represents the result of a constraint after validating the topology
/// </summary>
public sealed class RuleResult
{
    public bool Passed { get; init; }
    public string RuleName { get; init; } = null!;
    public IReadOnlyList<string> Messages { get; init; } = [];
}