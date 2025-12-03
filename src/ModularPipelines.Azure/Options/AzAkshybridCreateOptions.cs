using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("akshybrid", "create")]
public record AzAkshybridCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CliOption("--arc-agent-auto-upgrade")]
    public string? ArcAgentAutoUpgrade { get; set; }

    [CliOption("--arc-agent-version")]
    public string? ArcAgentVersion { get; set; }

    [CliOption("--control-plane-count")]
    public int? ControlPlaneCount { get; set; }

    [CliOption("--control-plane-ip")]
    public string? ControlPlaneIp { get; set; }

    [CliOption("--control-plane-vm-size")]
    public string? ControlPlaneVmSize { get; set; }

    [CliFlag("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CliOption("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CliOption("--load-balancer-count")]
    public int? LoadBalancerCount { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-ssh-key")]
    public bool? NoSshKey { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--node-vm-size")]
    public string? NodeVmSize { get; set; }

    [CliOption("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CliOption("--ssh-key-value")]
    public string? SshKeyValue { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet-ids")]
    public string? VnetIds { get; set; }
}