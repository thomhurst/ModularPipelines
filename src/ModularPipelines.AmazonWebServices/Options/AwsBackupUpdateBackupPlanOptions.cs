using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "update-backup-plan")]
public record AwsBackupUpdateBackupPlanOptions(
[property: CliOption("--backup-plan-id")] string BackupPlanId,
[property: CliOption("--backup-plan")] string BackupPlan
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}