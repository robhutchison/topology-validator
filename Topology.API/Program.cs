using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Topology.API.DTO;
using Topology.Application;
using Topology.Domain.Constraints;
using Topology.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton(new TopologyValidator([
    new NoOrphanNodes(), new NoCyclicLinks(), new CapabilityRequiredAttribute(), new LinkKind(), new MaxFanOut()
]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// NOTE: only doing this because the spec said to use minimal API. I'd normally use a controller and automapper for converting to/from DTOs.
app.MapPost("/topology/validate", (TopologyDto dto) =>
{
    var validator = app.Services.GetRequiredService<TopologyValidator>();

    var topology = new Topology.Domain.Entities.Topology
    {
        Links = dto.Links.Select(x => new Link { From = x.From, To = x.To, Kind = x.Kind }).ToList(),
        Nodes = dto.Nodes.Select(x => new Node { Attributes = x.Attributes.AsReadOnly(), Capabilities = x.Capabilities.AsReadOnly() }).ToList()
    };
    var result = validator.ValidateTopology(topology);

    var resultDto = new ValidationResultDto
    {
        Passed = result.Passed,
        RuleResults = result.RuleResults.Select(x => new RuleResultDto
        {
            Passed = x.Passed,
            Messages = x.Messages.ToList(),
            RuleName = x.RuleName
        })
            .ToList()
    };

    return Results.Ok(resultDto);
});

app.MapGet("/rules", () =>
{
    var validator = app.Services.GetRequiredService<TopologyValidator>();

    return Results.Ok(validator.GetRules());
});

await app.RunAsync();
