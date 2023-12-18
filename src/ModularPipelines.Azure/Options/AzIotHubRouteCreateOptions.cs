using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "route", "create")]
public record AzIotHubRouteCreateOptions(
[property: CommandSwitch("--en")] string En,
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--condition")]
    public string? Condition { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}