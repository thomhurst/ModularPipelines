using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "get-backup-plan-from-json")]
public record AwsBackupGetBackupPlanFromJsonOptions(
[property: CliOption("--backup-plan-template-json")] string BackupPlanTemplateJson
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}