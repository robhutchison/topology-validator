namespace Topology.Domain.Constraints;

public interface ITopologyRule
{
    RuleResult Evaluate(Entities.Topology topology);
}