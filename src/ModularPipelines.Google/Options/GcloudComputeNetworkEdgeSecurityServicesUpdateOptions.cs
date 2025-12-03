using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-edge-security-services", "update")]
public record GcloudComputeNetworkEdgeSecurityServicesUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CliOption("--security-policy-region")]
    public string? SecurityPolicyRegion { get; set; }
}