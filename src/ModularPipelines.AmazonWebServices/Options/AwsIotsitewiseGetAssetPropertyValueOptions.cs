using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "get-asset-property-value")]
public record AwsIotsitewiseGetAssetPropertyValueOptions : AwsOptions
{
    [CliOption("--asset-id")]
    public string? AssetId { get; set; }

    [CliOption("--property-id")]
    public string? PropertyId { get; set; }

    [CliOption("--property-alias")]
    public string? PropertyAlias { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}