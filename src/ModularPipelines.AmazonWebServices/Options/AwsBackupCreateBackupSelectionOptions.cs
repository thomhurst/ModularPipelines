using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "create-backup-selection")]
public record AwsBackupCreateBackupSelectionOptions(
[property: CliOption("--backup-plan-id")] string BackupPlanId,
[property: CliOption("--backup-selection")] string BackupSelection
) : AwsOptions
{
    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}