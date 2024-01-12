using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "external-vpn-gateways", "delete")]
public record GcloudComputeExternalVpnGatewaysDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;