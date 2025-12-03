using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "message-endpoint", "create", "servicebus-queue")]
public record AzIotHubMessageEndpointCreateServicebusQueueOptions(
[property: CliOption("--en")] string En,
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--endpoint-namespace-name")]
    public string? EndpointNamespaceName { get; set; }

    [CliOption("--endpoint-policy-name")]
    public string? EndpointPolicyName { get; set; }

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