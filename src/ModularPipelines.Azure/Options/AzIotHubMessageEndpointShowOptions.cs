using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "message-endpoint", "show")]
public record AzIotHubMessageEndpointShowOptions(
[property: CliOption("--en")] string En,
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}