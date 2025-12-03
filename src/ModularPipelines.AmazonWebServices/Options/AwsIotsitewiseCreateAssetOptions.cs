using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "create-asset")]
public record AwsIotsitewiseCreateAssetOptions(
[property: CliOption("--asset-name")] string AssetName,
[property: CliOption("--asset-model-id")] string AssetModelId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--asset-description")]
    public string? AssetDescription { get; set; }

    [CliOption("--asset-id")]
    public string? AssetId { get; set; }

    [CliOption("--asset-external-id")]
    public string? AssetExternalId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}