using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "get-backup-selection")]
public record AwsBackupGetBackupSelectionOptions(
[property: CliOption("--backup-plan-id")] string BackupPlanId,
[property: CliOption("--selection-id")] string SelectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}