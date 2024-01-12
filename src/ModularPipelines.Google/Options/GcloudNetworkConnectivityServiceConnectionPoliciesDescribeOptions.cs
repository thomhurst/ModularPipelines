using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "service-connection-policies", "describe")]
public record GcloudNetworkConnectivityServiceConnectionPoliciesDescribeOptions(
[property: PositionalArgument] string ServiceConnectionPolicy,
[property: PositionalArgument] string Region
) : GcloudOptions;