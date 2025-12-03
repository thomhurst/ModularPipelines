using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-groups", "simulate-maintenance-event")]
public record GcloudComputeSoleTenancyNodeGroupsSimulateMaintenanceEventOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--nodes")]
    public string[]? Nodes { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}