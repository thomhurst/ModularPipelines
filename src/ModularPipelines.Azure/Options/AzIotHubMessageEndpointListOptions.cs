using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "message-endpoint", "list")]
public record AzIotHubMessageEndpointListOptions(
[property: CommandSwitch("--hub-name")] string HubName
) : AzOptions
{
    [CommandSwitch("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}