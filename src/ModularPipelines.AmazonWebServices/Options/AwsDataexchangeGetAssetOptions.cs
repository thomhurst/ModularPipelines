using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataexchange", "get-asset")]
public record AwsDataexchangeGetAssetOptions(
[property: CliOption("--asset-id")] string AssetId,
[property: CliOption("--data-set-id")] string DataSetId,
[property: CliOption("--revision-id")] string RevisionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}