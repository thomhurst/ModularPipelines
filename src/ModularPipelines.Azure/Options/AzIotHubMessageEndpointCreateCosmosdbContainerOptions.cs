using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "message-endpoint", "create", "cosmosdb-container")]
public record AzIotHubMessageEndpointCreateCosmosdbContainerOptions(
[property: CommandSwitch("--container")] string Container,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--en")] string En,
[property: CommandSwitch("--hub-name")] string HubName
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--endpoint-account")]
    public int? EndpointAccount { get; set; }

    [CommandSwitch("--endpoint-resource-group")]
    public string? EndpointResourceGroup { get; set; }

    [CommandSwitch("--endpoint-subscription-id")]
    public string? EndpointSubscriptionId { get; set; }

    [CommandSwitch("--endpoint-uri")]
    public string? EndpointUri { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--partition-key-name")]
    public string? PartitionKeyName { get; set; }

    [CommandSwitch("--partition-key-template")]
    public string? PartitionKeyTemplate { get; set; }

    [CommandSwitch("--pk")]
    public string? Pk { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secondary-key")]
    public string? SecondaryKey { get; set; }
}