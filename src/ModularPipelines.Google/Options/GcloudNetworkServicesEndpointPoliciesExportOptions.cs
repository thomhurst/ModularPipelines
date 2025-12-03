using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "endpoint-policies", "export")]
public record GcloudNetworkServicesEndpointPoliciesExportOptions(
[property: CliArgument] string EndpointPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}