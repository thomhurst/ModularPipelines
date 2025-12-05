using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "server-tls-policies", "describe")]
public record GcloudNetworkSecurityServerTlsPoliciesDescribeOptions(
[property: CliArgument] string ServerTlsPolicy,
[property: CliArgument] string Location
) : GcloudOptions;