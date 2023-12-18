using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "kubernetescluster", "agentpool", "create")]
public record AzNetworkcloudKubernetesclusterAgentpoolCreateOptions(
[property: CommandSwitch("--agent-pool-name")] string AgentPoolName,
[property: CommandSwitch("--count")] int Count,
[property: CommandSwitch("--kubernetes-cluster-name")] string KubernetesClusterName,
[property: CommandSwitch("--mode")] string Mode,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-sku-name")] string VmSkuName
) : AzOptions
{
    [CommandSwitch("--admin-username")]
    public string? AdminUsername { get; set; }

    [CommandSwitch("--agent-options")]
    public string? AgentOptions { get; set; }

    [BooleanCommandSwitch("--attached-network-configuration")]
    public bool? AttachedNetworkConfiguration { get; set; }

    [CommandSwitch("--availability-zones")]
    public string? AvailabilityZones { get; set; }

    [CommandSwitch("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [BooleanCommandSwitch("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CommandSwitch("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--taints")]
    public string? Taints { get; set; }

    [CommandSwitch("--upgrade-settings")]
    public string? UpgradeSettings { get; set; }
}

