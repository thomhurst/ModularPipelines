using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "message-route", "list")]
public record AzIotHubMessageRouteListOptions(
[property: CommandSwitch("--hub-name")] string HubName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }
}