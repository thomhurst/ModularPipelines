using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "policy-based-routes", "describe")]
public record GcloudNetworkConnectivityPolicyBasedRoutesDescribeOptions(
[property: CliArgument] string PolicyBasedRoute
) : GcloudOptions;