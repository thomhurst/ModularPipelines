using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "message-endpoint", "create", "storage-container")]
public record AzIotHubMessageEndpointCreateStorageContainerOptions(
[property: CommandSwitch("--container")] string Container,
[property: CommandSwitch("--en")] string En,
[property: CommandSwitch("--hub-name")] string HubName
) : AzOptions
{
    [CommandSwitch("--batch-frequency")]
    public string? BatchFrequency { get; set; }

    [CommandSwitch("--chunk-size")]
    public string? ChunkSize { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandSwitch("--endpoint-account")]
    public int? EndpointAccount { get; set; }

    [CommandSwitch("--endpoint-resource-group")]
    public string? EndpointResourceGroup { get; set; }

    [CommandSwitch("--endpoint-subscription-id")]
    public string? EndpointSubscriptionId { get; set; }

    [CommandSwitch("--endpoint-uri")]
    public string? EndpointUri { get; set; }

    [CommandSwitch("--ff")]
    public string? Ff { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}