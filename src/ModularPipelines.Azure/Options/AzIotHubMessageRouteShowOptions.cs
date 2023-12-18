using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "message-route", "show")]
public record AzIotHubMessageRouteShowOptions(
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--rn")] string Rn
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}