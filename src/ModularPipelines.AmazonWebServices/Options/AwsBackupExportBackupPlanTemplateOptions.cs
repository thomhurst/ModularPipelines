using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "export-backup-plan-template")]
public record AwsBackupExportBackupPlanTemplateOptions(
[property: CliOption("--backup-plan-id")] string BackupPlanId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}