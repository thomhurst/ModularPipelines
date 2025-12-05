using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "external-vpn-gateways", "list")]
public record GcloudComputeExternalVpnGatewaysListOptions : GcloudOptions;