using Topology.Domain.Solver;

namespace Topology.API.DTO
{
    using Domain.Entities;

    /// <summary>
    /// Mapper class for converting to/from DTOs and models
    /// </summary>
    public static class DtoMapper
    {
        public static Topology FromDto(this TopologyDto model)
        {
            return new Topology
            {
                Links = model.Links.Select(x => new Link
                {
                    From = x.From,
                    To = x.To,
                    Kind = x.Kind
                }).ToList(),
                Nodes = model.Nodes.Select(x => new Node
                {
                    Id = x.Id,
                    Type = x.Type,
                    Attributes = x.Attributes.ToDictionary(),
                    Capabilities = x.Capabilities.ToList()
                }).ToList()
            };
        }

        public static ValidationResultDto ToDto(this ValidationResult model)
        {
            return new ValidationResultDto
            {
                Passed = model.Passed,
                RuleResults = model.RuleResults.Select(x => new RuleResultDto
                    {
                        Passed = x.Passed,
                        Messages = x.Messages.ToList(),
                        RuleName = x.RuleName
                    })
                    .ToList()
            };
        }
    }
}