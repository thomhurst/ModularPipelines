using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "external-vpn-gateways", "list")]
public record GcloudComputeExternalVpnGatewaysListOptions : GcloudOptions;