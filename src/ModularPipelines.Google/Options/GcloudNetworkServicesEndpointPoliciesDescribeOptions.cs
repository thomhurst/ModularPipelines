using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "endpoint-policies", "describe")]
public record GcloudNetworkServicesEndpointPoliciesDescribeOptions(
[property: CliArgument] string EndpointPolicy,
[property: CliArgument] string Location
) : GcloudOptions;