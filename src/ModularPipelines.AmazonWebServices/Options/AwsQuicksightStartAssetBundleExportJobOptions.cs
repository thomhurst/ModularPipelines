using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "start-asset-bundle-export-job")]
public record AwsQuicksightStartAssetBundleExportJobOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--asset-bundle-export-job-id")] string AssetBundleExportJobId,
[property: CommandSwitch("--resource-arns")] string[] ResourceArns,
[property: CommandSwitch("--export-format")] string ExportFormat
) : AwsOptions
{
    [CommandSwitch("--cloud-formation-override-property-configuration")]
    public string? CloudFormationOverridePropertyConfiguration { get; set; }

    [CommandSwitch("--validation-strategy")]
    public string? ValidationStrategy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}