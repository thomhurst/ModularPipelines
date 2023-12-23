using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "update-backup-plan")]
public record AwsBackupUpdateBackupPlanOptions(
[property: CommandSwitch("--backup-plan-id")] string BackupPlanId,
[property: CommandSwitch("--backup-plan")] string BackupPlan
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}