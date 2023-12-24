using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "describe-asset-composite-model")]
public record AwsIotsitewiseDescribeAssetCompositeModelOptions(
[property: CommandSwitch("--asset-id")] string AssetId,
[property: CommandSwitch("--asset-composite-model-id")] string AssetCompositeModelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}