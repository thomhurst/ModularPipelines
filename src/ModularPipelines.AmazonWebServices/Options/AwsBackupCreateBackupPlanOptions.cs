using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "create-backup-plan")]
public record AwsBackupCreateBackupPlanOptions(
[property: CliOption("--backup-plan")] string BackupPlan
) : AwsOptions
{
    [CliOption("--backup-plan-tags")]
    public IEnumerable<KeyValue>? BackupPlanTags { get; set; }

    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}