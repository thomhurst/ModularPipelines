using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "memberships", "generate-gateway-rbac")]
public record GcloudContainerHubMembershipsGenerateGatewayRbacOptions(
[property: CliFlag("--anthos-support")] bool AnthosSupport,
[property: CliOption("--groups")] string Groups,
[property: CliOption("--users")] string Users
) : GcloudOptions
{
    [CliFlag("--apply")]
    public bool? Apply { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CliOption("--membership")]
    public string? Membership { get; set; }

    [CliOption("--rbac-output-file")]
    public string? RbacOutputFile { get; set; }

    [CliFlag("--revoke")]
    public bool? Revoke { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }
}