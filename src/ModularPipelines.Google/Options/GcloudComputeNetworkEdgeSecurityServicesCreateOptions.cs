using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-edge-security-services", "create")]
public record GcloudComputeNetworkEdgeSecurityServicesCreateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CommandSwitch("--security-policy-region")]
    public string? SecurityPolicyRegion { get; set; }
}