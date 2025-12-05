using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "message-endpoint", "create", "cosmosdb-container")]
public record AzIotHubMessageEndpointCreateCosmosdbContainerOptions(
[property: CliOption("--container")] string Container,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--en")] string En,
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--endpoint-account")]
    public int? EndpointAccount { get; set; }

    [CliOption("--endpoint-resource-group")]
    public string? EndpointResourceGroup { get; set; }

    [CliOption("--endpoint-subscription-id")]
    public string? EndpointSubscriptionId { get; set; }

    [CliOption("--endpoint-uri")]
    public string? EndpointUri { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--partition-key-name")]
    public string? PartitionKeyName { get; set; }

    [CliOption("--partition-key-template")]
    public string? PartitionKeyTemplate { get; set; }

    [CliOption("--pk")]
    public string? Pk { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secondary-key")]
    public string? SecondaryKey { get; set; }
}