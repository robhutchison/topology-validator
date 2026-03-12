namespace Topology.Domain.Constraints;

public interface ITopologyRule
{
    string RuleName();

    RuleResult Evaluate(Entities.Topology topology);
}