namespace Topology.Domain.Constraints
{
    /// <summary>
    /// Ensure that nodes do not have an excessive number of outbound links
    /// </summary>
    public class MaxFanOut : ITopologyRule
    {
        public string RuleName()
        {
            return nameof(MaxFanOut);
        }

        public RuleResult Evaluate(Entities.Topology topology)
        {
            // group the links by the source (From) to get all the targets (To)
            var outgoingLinks = topology.Links
                .GroupBy(x => x.From)
                .Where(x => x.Count() > 3);

            var messages = outgoingLinks
                .Select(sourceGroup => 
                    $"Node {sourceGroup.Key} has {sourceGroup.Count()} outgoing links")
                .ToList();

            return new RuleResult
            {
                Passed = messages.Count == 0,
                RuleName = nameof(MaxFanOut),
                Messages = messages
            };
        }
    }
}