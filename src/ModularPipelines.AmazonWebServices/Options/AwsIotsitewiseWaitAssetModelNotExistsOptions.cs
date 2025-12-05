using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "wait", "asset-model-not-exists")]
public record AwsIotsitewiseWaitAssetModelNotExistsOptions(
[property: CliOption("--asset-model-id")] string AssetModelId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}