using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "describe-asset-composite-model")]
public record AwsIotsitewiseDescribeAssetCompositeModelOptions(
[property: CliOption("--asset-id")] string AssetId,
[property: CliOption("--asset-composite-model-id")] string AssetCompositeModelId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}