using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "message-route", "delete")]
public record AzIotHubMessageRouteDeleteOptions(
[property: CommandSwitch("--hub-name")] string HubName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rn")]
    public string? Rn { get; set; }

    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}