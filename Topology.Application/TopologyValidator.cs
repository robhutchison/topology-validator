using Topology.Domain.Solver;

namespace Topology.Application
{
    /// <summary>
    /// This class implements the logic to validate the Topology
    /// </summary>
    public class TopologyValidator
    {
        public TopologyValidator()
        {
            // register all the constraints when the class is created
        }

        /// <summary>
        /// Take in a Topology and run each of the constraints on it to check if it is valid and return any errors if not.
        /// </summary>
        /// <param name="topology"></param>
        /// <returns></returns>
        public ValidationResult ValidateTopology(Domain.Entities.Topology topology)
        {
            return new ValidationResult();
        }
    }
}
