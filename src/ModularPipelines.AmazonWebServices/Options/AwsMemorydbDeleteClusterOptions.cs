using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "delete-cluster")]
public record AwsMemorydbDeleteClusterOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName
) : AwsOptions
{
    [CommandSwitch("--final-snapshot-name")]
    public string? FinalSnapshotName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}