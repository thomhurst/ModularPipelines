using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "external-vpn-gateways", "delete")]
public record GcloudComputeExternalVpnGatewaysDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;