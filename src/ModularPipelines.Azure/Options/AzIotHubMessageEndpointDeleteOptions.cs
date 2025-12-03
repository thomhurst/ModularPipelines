using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "message-endpoint", "delete")]
public record AzIotHubMessageEndpointDeleteOptions(
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--en")]
    public string? En { get; set; }

    [CliOption("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}