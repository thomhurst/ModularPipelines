using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "describe-asset-model-composite-model")]
public record AwsIotsitewiseDescribeAssetModelCompositeModelOptions(
[property: CliOption("--asset-model-id")] string AssetModelId,
[property: CliOption("--asset-model-composite-model-id")] string AssetModelCompositeModelId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}