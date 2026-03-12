namespace Topology.Domain.Constraints;

public class CapabilityRequiredAttribute : ITopologyRule
{
    private readonly Dictionary<string, string> _requiredCapAttributes = new()
    {
        {"storage","capacity"},
        {"compute","cores"},
        {"secure","encryption"},
    };

    public string RuleName()
    {
        return nameof(CapabilityRequiredAttribute);
    }

    public RuleResult Evaluate(Entities.Topology topology)
    {
        var messages = new List<string>();
        foreach (var node in topology.Nodes)
        {
            foreach (var (capability, attribute) in _requiredCapAttributes)
            {
                if (node.Capabilities.Contains(capability) && !node.Attributes.ContainsKey(attribute))
                {
                    messages.Add($"Node {node.Id} has capability {capability} but no {attribute} attribute");
                }
            }
        }

        return new RuleResult
        {
            Passed = messages.Count == 0,
            RuleName = nameof(CapabilityRequiredAttribute),
            Messages = messages
        };
    }
}