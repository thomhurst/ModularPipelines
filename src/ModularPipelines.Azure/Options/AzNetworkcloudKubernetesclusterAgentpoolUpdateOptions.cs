using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "kubernetescluster", "agentpool", "update")]
public record AzNetworkcloudKubernetesclusterAgentpoolUpdateOptions : AzOptions
{
    [CommandSwitch("--agent-pool-name")]
    public string? AgentPoolName { get; set; }

    [CommandSwitch("--count")]
    public int? Count { get; set; }

    [BooleanCommandSwitch("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kubernetes-cluster-name")]
    public string? KubernetesClusterName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CommandSwitch("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--upgrade-settings")]
    public string? UpgradeSettings { get; set; }
}

