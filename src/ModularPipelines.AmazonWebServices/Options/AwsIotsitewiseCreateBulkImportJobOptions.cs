using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "create-bulk-import-job")]
public record AwsIotsitewiseCreateBulkImportJobOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--job-role-arn")] string JobRoleArn,
[property: CommandSwitch("--files")] string[] Files,
[property: CommandSwitch("--error-report-location")] string ErrorReportLocation,
[property: CommandSwitch("--job-configuration")] string JobConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}