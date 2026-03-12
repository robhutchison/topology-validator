namespace Topology.API.DTO;

public sealed class RuleResultDto
{
    public bool Passed { get; init; }
    public string RuleName { get; init; } = null!;
    public IReadOnlyList<string> Messages { get; init; } = [];
}