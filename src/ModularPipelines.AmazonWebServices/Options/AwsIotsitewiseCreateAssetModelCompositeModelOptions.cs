using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "create-asset-model-composite-model")]
public record AwsIotsitewiseCreateAssetModelCompositeModelOptions(
[property: CliOption("--asset-model-id")] string AssetModelId,
[property: CliOption("--asset-model-composite-model-name")] string AssetModelCompositeModelName,
[property: CliOption("--asset-model-composite-model-type")] string AssetModelCompositeModelType
) : AwsOptions
{
    [CliOption("--parent-asset-model-composite-model-id")]
    public string? ParentAssetModelCompositeModelId { get; set; }

    [CliOption("--asset-model-composite-model-external-id")]
    public string? AssetModelCompositeModelExternalId { get; set; }

    [CliOption("--asset-model-composite-model-id")]
    public string? AssetModelCompositeModelId { get; set; }

    [CliOption("--asset-model-composite-model-description")]
    public string? AssetModelCompositeModelDescription { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--composed-asset-model-id")]
    public string? ComposedAssetModelId { get; set; }

    [CliOption("--asset-model-composite-model-properties")]
    public string[]? AssetModelCompositeModelProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}