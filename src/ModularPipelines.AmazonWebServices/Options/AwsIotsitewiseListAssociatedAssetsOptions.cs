using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "list-associated-assets")]
public record AwsIotsitewiseListAssociatedAssetsOptions(
[property: CommandSwitch("--asset-id")] string AssetId
) : AwsOptions
{
    [CommandSwitch("--hierarchy-id")]
    public string? HierarchyId { get; set; }

    [CommandSwitch("--traversal-direction")]
    public string? TraversalDirection { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}