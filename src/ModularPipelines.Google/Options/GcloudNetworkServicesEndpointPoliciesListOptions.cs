using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "endpoint-policies", "list")]
public record GcloudNetworkServicesEndpointPoliciesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;