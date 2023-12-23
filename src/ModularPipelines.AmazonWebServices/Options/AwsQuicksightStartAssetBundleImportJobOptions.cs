using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "start-asset-bundle-import-job")]
public record AwsQuicksightStartAssetBundleImportJobOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--asset-bundle-import-job-id")] string AssetBundleImportJobId
) : AwsOptions
{
    [CommandSwitch("--asset-bundle-import-source")]
    public string? AssetBundleImportSource { get; set; }

    [CommandSwitch("--override-parameters")]
    public string? OverrideParameters { get; set; }

    [CommandSwitch("--failure-action")]
    public string? FailureAction { get; set; }

    [CommandSwitch("--override-permissions")]
    public string? OverridePermissions { get; set; }

    [CommandSwitch("--override-tags")]
    public string? OverrideTags { get; set; }

    [CommandSwitch("--override-validation-strategy")]
    public string? OverrideValidationStrategy { get; set; }

    [CommandSwitch("--asset-bundle-import-source-bytes")]
    public string? AssetBundleImportSourceBytes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}