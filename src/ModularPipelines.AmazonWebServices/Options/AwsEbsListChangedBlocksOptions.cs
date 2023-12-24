using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ebs", "list-changed-blocks")]
public record AwsEbsListChangedBlocksOptions(
[property: CommandSwitch("--second-snapshot-id")] string SecondSnapshotId
) : AwsOptions
{
    [CommandSwitch("--first-snapshot-id")]
    public string? FirstSnapshotId { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--starting-block-index")]
    public int? StartingBlockIndex { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}