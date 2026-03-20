namespace Topology.Domain;

/// <summary>
/// Represents the result of a constraint after validating the topology
/// </summary>
public sealed class RuleResult
{
    /// <summary>
    /// Did the rule named in RuleName pass successfully for all the relevant parts of the topology without any errors
    /// </summary>
    public bool Passed { get; init; }

    /// <summary>
    /// The name of the rule which created these results
    /// </summary>
    public string RuleName { get; init; } = null!;

    /// <summary>
    /// Details of any errors in the topology that were identified by the rule named in RuleName
    /// </summary>
    public IReadOnlyList<string> Messages { get; init; } = [];

    /// <summary>
    /// The list of faults encountered by the rule named in RuleName, or empty if none
    /// </summary>
    public IReadOnlyList<string> Faults { get; set; } = [];

    /// <summary>
    /// Did the rule named in RuleName encounter a serious error in the data
    /// </summary>
    public bool Faulted => Faults.Count != 0;
}