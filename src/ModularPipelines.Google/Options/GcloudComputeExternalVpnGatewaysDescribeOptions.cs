using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "external-vpn-gateways", "describe")]
public record GcloudComputeExternalVpnGatewaysDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;