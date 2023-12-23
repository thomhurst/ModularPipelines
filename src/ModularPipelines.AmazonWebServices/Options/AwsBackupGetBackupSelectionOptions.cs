using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "get-backup-selection")]
public record AwsBackupGetBackupSelectionOptions(
[property: CommandSwitch("--backup-plan-id")] string BackupPlanId,
[property: CommandSwitch("--selection-id")] string SelectionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}