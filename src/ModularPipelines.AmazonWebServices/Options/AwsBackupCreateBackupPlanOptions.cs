using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "create-backup-plan")]
public record AwsBackupCreateBackupPlanOptions(
[property: CommandSwitch("--backup-plan")] string BackupPlan
) : AwsOptions
{
    [CommandSwitch("--backup-plan-tags")]
    public IEnumerable<KeyValue>? BackupPlanTags { get; set; }

    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}