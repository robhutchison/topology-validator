namespace Topology.Domain.Constraints
{
    public class MaxFanOut : ITopologyRule
    {
        public RuleResult Evaluate(Entities.Topology topology)
        {
            var messages = new List<string>();

            // group the links by the source (From) to get all the targets (To)
            var outgoingLinks = topology.Links
                .GroupBy(x => x.From)
                .Where(x => x.Count() > 3);

            foreach (var sourceGroup in outgoingLinks)
            {
                messages.Add($"Node {sourceGroup.Key} has {sourceGroup.Count()} outgoing links");
            }

            return new RuleResult
            {
                Passed = messages.Count == 0,
                RuleName = nameof(MaxFanOut),
                Messages = messages
            };
        }
    }
}