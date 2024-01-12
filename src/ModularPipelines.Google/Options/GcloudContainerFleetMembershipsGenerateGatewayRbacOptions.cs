using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "memberships", "generate-gateway-rbac")]
public record GcloudContainerFleetMembershipsGenerateGatewayRbacOptions(
[property: BooleanCommandSwitch("--anthos-support")] bool AnthosSupport,
[property: CommandSwitch("--groups")] string Groups,
[property: CommandSwitch("--users")] string Users
) : GcloudOptions
{
    [BooleanCommandSwitch("--apply")]
    public bool? Apply { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CommandSwitch("--membership")]
    public string? Membership { get; set; }

    [CommandSwitch("--rbac-output-file")]
    public string? RbacOutputFile { get; set; }

    [BooleanCommandSwitch("--revoke")]
    public bool? Revoke { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }
}