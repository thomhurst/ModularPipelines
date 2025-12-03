using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "message-endpoint", "create", "storage-container")]
public record AzIotHubMessageEndpointCreateStorageContainerOptions(
[property: CliOption("--container")] string Container,
[property: CliOption("--en")] string En,
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--batch-frequency")]
    public string? BatchFrequency { get; set; }

    [CliOption("--chunk-size")]
    public string? ChunkSize { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--encoding")]
    public string? Encoding { get; set; }

    [CliOption("--endpoint-account")]
    public int? EndpointAccount { get; set; }

    [CliOption("--endpoint-resource-group")]
    public string? EndpointResourceGroup { get; set; }

    [CliOption("--endpoint-subscription-id")]
    public string? EndpointSubscriptionId { get; set; }

    [CliOption("--endpoint-uri")]
    public string? EndpointUri { get; set; }

    [CliOption("--ff")]
    public string? Ff { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}