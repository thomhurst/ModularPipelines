using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-asset-bundle-export-job")]
public record AwsQuicksightDescribeAssetBundleExportJobOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--asset-bundle-export-job-id")] string AssetBundleExportJobId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}