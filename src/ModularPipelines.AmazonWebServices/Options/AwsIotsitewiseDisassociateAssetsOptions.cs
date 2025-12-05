using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "disassociate-assets")]
public record AwsIotsitewiseDisassociateAssetsOptions(
[property: CliOption("--asset-id")] string AssetId,
[property: CliOption("--hierarchy-id")] string HierarchyId,
[property: CliOption("--child-asset-id")] string ChildAssetId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}