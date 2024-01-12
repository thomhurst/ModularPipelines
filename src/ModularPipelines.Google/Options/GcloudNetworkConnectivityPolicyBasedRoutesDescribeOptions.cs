using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "policy-based-routes", "describe")]
public record GcloudNetworkConnectivityPolicyBasedRoutesDescribeOptions(
[property: PositionalArgument] string PolicyBasedRoute
) : GcloudOptions;