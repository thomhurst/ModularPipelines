using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "get-backup-plan-from-template")]
public record AwsBackupGetBackupPlanFromTemplateOptions(
[property: CommandSwitch("--backup-plan-template-id")] string BackupPlanTemplateId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}