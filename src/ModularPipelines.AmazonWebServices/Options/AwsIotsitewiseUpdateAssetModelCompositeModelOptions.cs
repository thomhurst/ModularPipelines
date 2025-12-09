using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "update-asset-model-composite-model")]
public record AwsIotsitewiseUpdateAssetModelCompositeModelOptions(
[property: CliOption("--asset-model-id")] string AssetModelId,
[property: CliOption("--asset-model-composite-model-id")] string AssetModelCompositeModelId,
[property: CliOption("--asset-model-composite-model-name")] string AssetModelCompositeModelName
) : AwsOptions
{
    [CliOption("--asset-model-composite-model-external-id")]
    public string? AssetModelCompositeModelExternalId { get; set; }

    [CliOption("--asset-model-composite-model-description")]
    public string? AssetModelCompositeModelDescription { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--asset-model-composite-model-properties")]
    public string[]? AssetModelCompositeModelProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}