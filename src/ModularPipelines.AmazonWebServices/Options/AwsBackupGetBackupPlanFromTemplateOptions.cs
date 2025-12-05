using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "get-backup-plan-from-template")]
public record AwsBackupGetBackupPlanFromTemplateOptions(
[property: CliOption("--backup-plan-template-id")] string BackupPlanTemplateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}