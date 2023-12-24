using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "batch-update-cluster")]
public record AwsMemorydbBatchUpdateClusterOptions(
[property: CommandSwitch("--cluster-names")] string[] ClusterNames
) : AwsOptions
{
    [CommandSwitch("--service-update")]
    public string? ServiceUpdate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}