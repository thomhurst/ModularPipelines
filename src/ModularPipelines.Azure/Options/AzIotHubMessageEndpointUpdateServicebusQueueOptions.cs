using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "message-endpoint", "update", "servicebus-queue")]
public record AzIotHubMessageEndpointUpdateServicebusQueueOptions(
[property: CliOption("--en")] string En,
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--endpoint-resource-group")]
    public string? EndpointResourceGroup { get; set; }

    [CliOption("--endpoint-subscription-id")]
    public string? EndpointSubscriptionId { get; set; }

    [CliOption("--endpoint-uri")]
    public string? EndpointUri { get; set; }

    [CliOption("--entity-path")]
    public string? EntityPath { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}