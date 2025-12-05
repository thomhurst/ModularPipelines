using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "list-associated-assets")]
public record AwsIotsitewiseListAssociatedAssetsOptions(
[property: CliOption("--asset-id")] string AssetId
) : AwsOptions
{
    [CliOption("--hierarchy-id")]
    public string? HierarchyId { get; set; }

    [CliOption("--traversal-direction")]
    public string? TraversalDirection { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}