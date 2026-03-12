namespace Topology.API.DTO;

public sealed class RuleResultDto
{
    public bool Passed { get; init; }
    public string RuleName { get; init; } = null!;
    public IList<string> Messages { get; init; } = [];
}