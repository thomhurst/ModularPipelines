using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune-graph", "wait", "graph-snapshot-deleted")]
public record AwsNeptuneGraphWaitGraphSnapshotDeletedOptions(
[property: CliOption("--snapshot-identifier")] string SnapshotIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}