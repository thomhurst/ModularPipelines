using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "kubernetescluster", "agentpool", "show")]
public record AzNetworkcloudKubernetesclusterAgentpoolShowOptions : AzOptions
{
    [CliOption("--agent-pool-name")]
    public string? AgentPoolName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kubernetes-cluster-name")]
    public string? KubernetesClusterName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}