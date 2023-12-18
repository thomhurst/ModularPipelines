using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "kubernetescluster", "agentpool", "delete")]
public record AzNetworkcloudKubernetesclusterAgentpoolDeleteOptions : AzOptions
{
    [CommandSwitch("--agent-pool-name")]
    public string? AgentPoolName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kubernetes-cluster-name")]
    public string? KubernetesClusterName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}