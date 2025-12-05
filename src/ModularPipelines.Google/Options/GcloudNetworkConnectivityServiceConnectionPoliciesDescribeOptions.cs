using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "service-connection-policies", "describe")]
public record GcloudNetworkConnectivityServiceConnectionPoliciesDescribeOptions(
[property: CliArgument] string ServiceConnectionPolicy,
[property: CliArgument] string Region
) : GcloudOptions;