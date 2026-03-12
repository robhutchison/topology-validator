namespace Topology.Domain.Constraints
{
    public class NoCyclicLinks : ITopologyRule
    {
        public RuleResult Evaluate(Entities.Topology topology)
        {
            var previousLinearNodeIds = new List<string>();
            var messages = new List<string>();
            // validate only if all nodes are linear else pass
            // need the node types for each linked pair of nodes.
            // include the start and end nodes violating the rule
            // message is Nodes of type linear must not form cycles


            foreach (var link in topology.Links)
            {
                var fromNode = topology.Nodes.FirstOrDefault(x => x.Id == link.From) ??
                               throw new InvalidOperationException($"From Id {link.From} in link is invalid");
                if (fromNode.Type != "linear") continue;

                var toNode = topology.Nodes.FirstOrDefault(x => x.Id == link.To) ??
                             throw new InvalidOperationException($"To Id {link.To} in link is invalid");
                if (toNode.Type != "linear") continue;

                if (previousLinearNodeIds.Contains(link.To))
                {
                    messages.Add($"Nodes of type linear must not form cycles {fromNode.Id} to {toNode.Id}");
                    continue;
                }

                previousLinearNodeIds.Add(link.From);
            }


            return new RuleResult
            {
                Passed = messages.Count == 0,
                RuleName = nameof(NoCyclicLinks),
                Messages = messages
            };
        }
    }
}