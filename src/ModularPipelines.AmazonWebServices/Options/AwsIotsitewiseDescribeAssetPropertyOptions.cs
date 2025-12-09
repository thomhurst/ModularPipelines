using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "describe-asset-property")]
public record AwsIotsitewiseDescribeAssetPropertyOptions(
[property: CliOption("--asset-id")] string AssetId,
[property: CliOption("--property-id")] string PropertyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}