using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-edge-security-services", "list")]
public record GcloudComputeNetworkEdgeSecurityServicesListOptions : GcloudOptions;