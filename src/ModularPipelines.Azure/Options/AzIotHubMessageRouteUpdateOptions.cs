using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "message-route", "update")]
public record AzIotHubMessageRouteUpdateOptions(
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--rn")] string Rn
) : AzOptions
{
    [CommandSwitch("--condition")]
    public string? Condition { get; set; }

    [CommandSwitch("--en")]
    public string? En { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }
}