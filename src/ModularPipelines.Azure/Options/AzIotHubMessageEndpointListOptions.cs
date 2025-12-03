using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "message-endpoint", "list")]
public record AzIotHubMessageEndpointListOptions(
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}