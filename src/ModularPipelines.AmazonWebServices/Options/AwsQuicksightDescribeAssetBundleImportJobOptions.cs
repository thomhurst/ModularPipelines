using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-asset-bundle-import-job")]
public record AwsQuicksightDescribeAssetBundleImportJobOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--asset-bundle-import-job-id")] string AssetBundleImportJobId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}