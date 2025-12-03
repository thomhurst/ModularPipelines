using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "create-asset-model")]
public record AwsIotsitewiseCreateAssetModelOptions(
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

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--asset-model-id")]
    public string? AssetModelId { get; set; }

    [CliOption("--asset-model-external-id")]
    public string? AssetModelExternalId { get; set; }

    [CliOption("--asset-model-type")]
    public string? AssetModelType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}