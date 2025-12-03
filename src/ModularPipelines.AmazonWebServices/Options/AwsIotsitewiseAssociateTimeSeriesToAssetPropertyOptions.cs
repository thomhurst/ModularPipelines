using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "associate-time-series-to-asset-property")]
public record AwsIotsitewiseAssociateTimeSeriesToAssetPropertyOptions(
[property: CliOption("--alias")] string Alias,
[property: CliOption("--asset-id")] string AssetId,
[property: CliOption("--property-id")] string PropertyId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}