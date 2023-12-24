using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "delete-db-cluster-snapshot")]
public record AwsNeptuneDeleteDbClusterSnapshotOptions(
[property: CommandSwitch("--db-cluster-snapshot-identifier")] string DbClusterSnapshotIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}