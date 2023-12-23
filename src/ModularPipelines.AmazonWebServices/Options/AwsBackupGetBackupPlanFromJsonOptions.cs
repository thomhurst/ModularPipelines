using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "get-backup-plan-from-json")]
public record AwsBackupGetBackupPlanFromJsonOptions(
[property: CommandSwitch("--backup-plan-template-json")] string BackupPlanTemplateJson
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}