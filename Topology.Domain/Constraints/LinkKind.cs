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
            var faults = new List<string>();
            foreach (var link in topology.Links)
            {
                if (!_allowedLinks.TryGetValue(link.Kind, out var allowedLinks))
                {
                    faults.Add($"Invalid link type {link.Kind}");
                }

                var fromNode = topology.Nodes.FirstOrDefault(x => x.Id == link.From);
                if (fromNode == null)
                {
                    faults.Add($"Invalid From node id {link.From} in link");
                }

                var toNode = topology.Nodes.FirstOrDefault(x => x.Id == link.To);

                if (toNode == null)
                {
                    faults.Add($"Invalid To node id {link.To} in link");
                }

                if (fromNode == null || toNode == null) continue;

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
                Messages = messages,
                Faults = faults
            };
        }
    }
}