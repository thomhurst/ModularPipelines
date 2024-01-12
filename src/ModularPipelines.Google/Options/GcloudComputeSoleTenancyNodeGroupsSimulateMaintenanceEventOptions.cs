using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-groups", "simulate-maintenance-event")]
public record GcloudComputeSoleTenancyNodeGroupsSimulateMaintenanceEventOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--nodes")]
    public string[]? Nodes { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}