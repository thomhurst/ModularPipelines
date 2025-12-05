using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune-graph", "delete-graph-snapshot")]
public record AwsNeptuneGraphDeleteGraphSnapshotOptions(
[property: CliOption("--snapshot-identifier")] string SnapshotIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}