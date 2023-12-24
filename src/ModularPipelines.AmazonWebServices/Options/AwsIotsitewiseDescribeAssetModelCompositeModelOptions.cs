using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "describe-asset-model-composite-model")]
public record AwsIotsitewiseDescribeAssetModelCompositeModelOptions(
[property: CommandSwitch("--asset-model-id")] string AssetModelId,
[property: CommandSwitch("--asset-model-composite-model-id")] string AssetModelCompositeModelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}