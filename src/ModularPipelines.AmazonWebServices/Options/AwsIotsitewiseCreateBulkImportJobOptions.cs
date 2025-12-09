using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "create-bulk-import-job")]
public record AwsIotsitewiseCreateBulkImportJobOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--job-role-arn")] string JobRoleArn,
[property: CliOption("--files")] string[] Files,
[property: CliOption("--error-report-location")] string ErrorReportLocation,
[property: CliOption("--job-configuration")] string JobConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}