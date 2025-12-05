using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "kubernetescluster", "agentpool", "list")]
public record AzNetworkcloudKubernetesclusterAgentpoolListOptions(
[property: CliOption("--kubernetes-cluster-name")] string KubernetesClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}