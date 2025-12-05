using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "external-vpn-gateways", "describe")]
public record GcloudComputeExternalVpnGatewaysDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;