using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "kubernetescluster", "agentpool", "create")]
public record AzNetworkcloudKubernetesclusterAgentpoolCreateOptions(
[property: CliOption("--agent-pool-name")] string AgentPoolName,
[property: CliOption("--count")] int Count,
[property: CliOption("--kubernetes-cluster-name")] string KubernetesClusterName,
[property: CliOption("--mode")] string Mode,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-sku-name")] string VmSkuName
) : AzOptions
{
    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliOption("--agent-options")]
    public string? AgentOptions { get; set; }

    [CliFlag("--attached-network-configuration")]
    public bool? AttachedNetworkConfiguration { get; set; }

    [CliOption("--availability-zones")]
    public string? AvailabilityZones { get; set; }

    [CliOption("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CliFlag("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CliOption("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--taints")]
    public string? Taints { get; set; }

    [CliOption("--upgrade-settings")]
    public string? UpgradeSettings { get; set; }
}