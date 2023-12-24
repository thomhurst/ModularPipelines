using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "put-restore-validation-result")]
public record AwsBackupPutRestoreValidationResultOptions(
[property: CommandSwitch("--restore-job-id")] string RestoreJobId,
[property: CommandSwitch("--validation-status")] string ValidationStatus
) : AwsOptions
{
    [CommandSwitch("--validation-status-message")]
    public string? ValidationStatusMessage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}