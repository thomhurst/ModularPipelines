using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "update-asset-model")]
public record AwsIotsitewiseUpdateAssetModelOptions(
[property: CliOption("--asset-model-id")] string AssetModelId,
[property: CliOption("--asset-model-name")] string AssetModelName
) : AwsOptions
{
    [CliOption("--asset-model-description")]
    public string? AssetModelDescription { get; set; }

    [CliOption("--asset-model-properties")]
    public string[]? AssetModelProperties { get; set; }

    [CliOption("--asset-model-hierarchies")]
    public string[]? AssetModelHierarchies { get; set; }

    [CliOption("--asset-model-composite-models")]
    public string[]? AssetModelCompositeModels { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--asset-model-external-id")]
    public string? AssetModelExternalId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}