using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "routing-endpoint", "create")]
public record AzIotHubRoutingEndpointCreateOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--endpoint-resource-group")] string EndpointResourceGroup,
[property: CommandSwitch("--endpoint-subscription-id")] string EndpointSubscriptionId,
[property: CommandSwitch("--endpoint-type")] string EndpointType,
[property: CommandSwitch("--hub-name")] string HubName
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--batch-frequency")]
    public string? BatchFrequency { get; set; }

    [CommandSwitch("--chunk-size")]
    public string? ChunkSize { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [CommandSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandSwitch("--endpoint-uri")]
    public string? EndpointUri { get; set; }

    [CommandSwitch("--entity-path")]
    public string? EntityPath { get; set; }

    [CommandSwitch("--ff")]
    public string? Ff { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

