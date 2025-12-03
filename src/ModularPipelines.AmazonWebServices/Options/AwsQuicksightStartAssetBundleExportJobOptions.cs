using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "start-asset-bundle-export-job")]
public record AwsQuicksightStartAssetBundleExportJobOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--asset-bundle-export-job-id")] string AssetBundleExportJobId,
[property: CliOption("--resource-arns")] string[] ResourceArns,
[property: CliOption("--export-format")] string ExportFormat
) : AwsOptions
{
    [CliOption("--cloud-formation-override-property-configuration")]
    public string? CloudFormationOverridePropertyConfiguration { get; set; }

    [CliOption("--validation-strategy")]
    public string? ValidationStrategy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}