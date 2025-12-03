using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ebs", "list-changed-blocks")]
public record AwsEbsListChangedBlocksOptions(
[property: CliOption("--second-snapshot-id")] string SecondSnapshotId
) : AwsOptions
{
    [CliOption("--first-snapshot-id")]
    public string? FirstSnapshotId { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--starting-block-index")]
    public int? StartingBlockIndex { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}