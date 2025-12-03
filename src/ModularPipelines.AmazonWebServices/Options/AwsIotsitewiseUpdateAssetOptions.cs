using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "update-asset")]
public record AwsIotsitewiseUpdateAssetOptions(
[property: CliOption("--asset-id")] string AssetId,
[property: CliOption("--asset-name")] string AssetName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--asset-description")]
    public string? AssetDescription { get; set; }

    [CliOption("--asset-external-id")]
    public string? AssetExternalId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}