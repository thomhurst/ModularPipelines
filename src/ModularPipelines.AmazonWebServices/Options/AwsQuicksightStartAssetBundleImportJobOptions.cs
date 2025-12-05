using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "start-asset-bundle-import-job")]
public record AwsQuicksightStartAssetBundleImportJobOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--asset-bundle-import-job-id")] string AssetBundleImportJobId
) : AwsOptions
{
    [CliOption("--asset-bundle-import-source")]
    public string? AssetBundleImportSource { get; set; }

    [CliOption("--override-parameters")]
    public string? OverrideParameters { get; set; }

    [CliOption("--failure-action")]
    public string? FailureAction { get; set; }

    [CliOption("--override-permissions")]
    public string? OverridePermissions { get; set; }

    [CliOption("--override-tags")]
    public string? OverrideTags { get; set; }

    [CliOption("--override-validation-strategy")]
    public string? OverrideValidationStrategy { get; set; }

    [CliOption("--asset-bundle-import-source-bytes")]
    public string? AssetBundleImportSourceBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}