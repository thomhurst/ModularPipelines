using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "kubernetescluster", "agentpool", "update")]
public record AzNetworkcloudKubernetesclusterAgentpoolUpdateOptions : AzOptions
{
    [CliOption("--agent-pool-name")]
    public string? AgentPoolName { get; set; }

    [CliOption("--count")]
    public int? Count { get; set; }

    [CliFlag("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kubernetes-cluster-name")]
    public string? KubernetesClusterName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CliOption("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--upgrade-settings")]
    public string? UpgradeSettings { get; set; }
}