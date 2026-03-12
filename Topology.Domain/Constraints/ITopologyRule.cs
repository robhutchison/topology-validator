namespace Topology.Domain.Constraints;

/// <summary>
/// Defines a contract for evaluating a topology against a specific rule and retrieving the rule's name.
/// </summary>
/// <remarks>Implementations of this interface should provide logic to assess whether a given topology complies
/// with the defined rule. The evaluation may involve checking various properties or constraints of the topology. This
/// interface enables extensibility for custom validation rules within topology validation frameworks.</remarks>
public interface ITopologyRule
{
    string RuleName();

    RuleResult Evaluate(Entities.Topology topology);
}