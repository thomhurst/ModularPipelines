using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "message-endpoint", "update", "eventhub")]
public record AzIotHubMessageEndpointUpdateEventhubOptions(
[property: CommandSwitch("--en")] string En,
[property: CommandSwitch("--hub-name")] string HubName
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--endpoint-resource-group")]
    public string? EndpointResourceGroup { get; set; }

    [CommandSwitch("--endpoint-subscription-id")]
    public string? EndpointSubscriptionId { get; set; }

    [CommandSwitch("--endpoint-uri")]
    public string? EndpointUri { get; set; }

    [CommandSwitch("--entity-path")]
    public string? EntityPath { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}