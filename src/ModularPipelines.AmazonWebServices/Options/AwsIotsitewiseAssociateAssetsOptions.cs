using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "associate-assets")]
public record AwsIotsitewiseAssociateAssetsOptions(
[property: CommandSwitch("--asset-id")] string AssetId,
[property: CommandSwitch("--hierarchy-id")] string HierarchyId,
[property: CommandSwitch("--child-asset-id")] string ChildAssetId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}