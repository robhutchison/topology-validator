namespace Topology.Domain.Constraints;

/// <summary>
/// Ensure that all nodes are linked to at least one other
/// </summary>
public class NoOrphanNodes : ITopologyRule
{
    public string RuleName()
    {
        return nameof(NoOrphanNodes);
    }

    public RuleResult Evaluate(Entities.Topology topology)
    {
        // make sure the nodes are all in the links list
        var links = topology.Links.Select(x => x.From).Union(topology.Links.Select(x => x.To)).ToList();
        var messages = new List<string>();
        
        foreach (var node in topology.Nodes)
        {
            if (!links.Contains(node.Id))
            {
                messages.Add($"Node {node.Id} has no incoming or outgoing links");
            }
        }

        return  new RuleResult
        {
            Passed = messages.Count==0,
            RuleName = nameof(NoOrphanNodes),
            Messages = messages
        };
    }
}