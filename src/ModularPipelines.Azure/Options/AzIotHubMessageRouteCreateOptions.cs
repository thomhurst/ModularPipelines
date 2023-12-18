using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "message-route", "create")]
public record AzIotHubMessageRouteCreateOptions(
[property: CommandSwitch("--en")] string En,
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--rn")] string Rn,
[property: CommandSwitch("--source-type")] string SourceType
) : AzOptions
{
    [BooleanCommandSwitch("--condition")]
    public bool? Condition { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}