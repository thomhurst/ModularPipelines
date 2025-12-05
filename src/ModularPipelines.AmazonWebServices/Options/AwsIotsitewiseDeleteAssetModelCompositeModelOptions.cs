using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "delete-asset-model-composite-model")]
public record AwsIotsitewiseDeleteAssetModelCompositeModelOptions(
[property: CliOption("--asset-model-id")] string AssetModelId,
[property: CliOption("--asset-model-composite-model-id")] string AssetModelCompositeModelId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}