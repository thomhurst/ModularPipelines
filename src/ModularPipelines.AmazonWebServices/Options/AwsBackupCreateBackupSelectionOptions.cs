using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "create-backup-selection")]
public record AwsBackupCreateBackupSelectionOptions(
[property: CommandSwitch("--backup-plan-id")] string BackupPlanId,
[property: CommandSwitch("--backup-selection")] string BackupSelection
) : AwsOptions
{
    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}