using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "wait", "asset-not-exists")]
public record AwsIotsitewiseWaitAssetNotExistsOptions(
[property: CliOption("--asset-id")] string AssetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}