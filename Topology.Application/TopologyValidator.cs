using Topology.Domain;
using Topology.Domain.Constraints;
using Topology.Domain.Solver;

namespace Topology.Application
{
    /// <summary>
    /// This class implements the logic to validate the Topology using the supplied rules
    /// </summary>
    public class TopologyValidator(List<ITopologyRule> rules)
    {
        public List<string> GetRules() => rules.Select(x => x.RuleName()).ToList();

        /// <summary>
        /// Take in a Topology and run each of the constraints on it to check if it is valid and return any errors if not.
        /// </summary>
        /// <param name="topology">A system topology description</param>
        /// <returns>A ValidationResult indicating the result of processing by the configured constraints</returns>
        public ValidationResult ValidateTopology(Domain.Entities.Topology topology)
        {
            var ruleResults = new List<RuleResult>();
            foreach (var rule in rules)
            {
                ruleResults.Add(rule.Evaluate(topology));
            }

            var result =new ValidationResult
            {
                Passed = ruleResults.All(x=>x.Passed),
                RuleResults = ruleResults
            };
            return result;
        }
    }
}
