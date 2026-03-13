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
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// NOTE: only doing this because the spec said to use minimal API. I'd normally use a controller 
app.MapPost("/topology/validate", (TopologyDto dto) =>
{
    var validator = app.Services.GetRequiredService<TopologyValidator>();

    var topology = dto.FromDto();
    var result = validator.ValidateTopology(topology);

    var resultDto = result.ToDto();

    return Results.Ok(resultDto);
});

app.MapGet("/rules", () =>
{
    var validator = app.Services.GetRequiredService<TopologyValidator>();

    return Results.Ok(validator.GetRules());
});

await app.RunAsync();