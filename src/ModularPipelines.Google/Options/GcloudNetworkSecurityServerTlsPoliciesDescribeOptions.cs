using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "server-tls-policies", "describe")]
public record GcloudNetworkSecurityServerTlsPoliciesDescribeOptions(
[property: PositionalArgument] string ServerTlsPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions;