using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-edge-security-services", "list")]
public record GcloudComputeNetworkEdgeSecurityServicesListOptions : GcloudOptions;