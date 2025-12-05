using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "put-restore-validation-result")]
public record AwsBackupPutRestoreValidationResultOptions(
[property: CliOption("--restore-job-id")] string RestoreJobId,
[property: CliOption("--validation-status")] string ValidationStatus
) : AwsOptions
{
    [CliOption("--validation-status-message")]
    public string? ValidationStatusMessage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}