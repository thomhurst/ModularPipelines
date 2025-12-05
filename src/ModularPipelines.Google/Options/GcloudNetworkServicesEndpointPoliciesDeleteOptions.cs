using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "endpoint-policies", "delete")]
public record GcloudNetworkServicesEndpointPoliciesDeleteOptions(
[property: CliArgument] string EndpointPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}