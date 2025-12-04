using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hdinsight-on-aks", "cluster", "list-service-config")]
public record AzHdinsightOnAksClusterListServiceConfigOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--cluster-pool-name")] string ClusterPoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}