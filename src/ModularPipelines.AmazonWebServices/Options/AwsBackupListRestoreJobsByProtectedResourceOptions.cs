using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "list-restore-jobs-by-protected-resource")]
public record AwsBackupListRestoreJobsByProtectedResourceOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CommandSwitch("--by-status")]
    public string? ByStatus { get; set; }

    [CommandSwitch("--by-recovery-point-creation-date-after")]
    public long? ByRecoveryPointCreationDateAfter { get; set; }

    [CommandSwitch("--by-recovery-point-creation-date-before")]
    public long? ByRecoveryPointCreationDateBefore { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}