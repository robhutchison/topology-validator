namespace Topology.Domain.Constraints
{
    /// <summary>
    /// Validate that the kind of link between 2 nodes is valid based on their types
    /// </summary>
    public class LinkKind : ITopologyRule
    {
        private readonly Dictionary<string, (string from, string to)> _allowedLinks = new()
        {
            { "data", ("compute", "storage") },
            { "control", ("compute", "compute") },
            { "replicate", ("storage", "storage") },
        };

        public string RuleName()
        {
            return nameof(LinkKind);
        }

        public RuleResult Evaluate(Entities.Topology topology)
        {
            var messages = new List<string>();
            foreach (var link in topology.Links)
            {
                if (!_allowedLinks.TryGetValue(link.Kind, out var allowedLinks))
                {
                    throw new InvalidOperationException($"Invalid link type {link.Kind}");
                }

                // todo: handle invalid node id in a cleaner way
                var fromNode = topology.Nodes.FirstOrDefault(x => x.Id == link.From) ??
                               throw new InvalidOperationException($"Invalid From node id {link.From} in link");

                // todo: handle invalid node id in a cleaner way
                var toNode = topology.Nodes.FirstOrDefault(x => x.Id == link.To) ??
                             throw new InvalidOperationException($"Invalid To node id {link.To} in link");

                if (!fromNode.Type.Equals(allowedLinks.from, StringComparison.InvariantCulture)
                    || !toNode.Type.Equals(allowedLinks.to, StringComparison.InvariantCulture))
                {
                    messages.Add($"A {link.Kind} link from a {fromNode.Type} to a {toNode.Type} is invalid");
                }
            }

            return new RuleResult
            {
                Passed = messages.Count == 0,
                RuleName = nameof(LinkKind),
                Messages = messages
            };
        }
    }
}