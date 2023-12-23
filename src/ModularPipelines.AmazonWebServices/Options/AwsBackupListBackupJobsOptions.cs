using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "list-backup-jobs")]
public record AwsBackupListBackupJobsOptions : AwsOptions
{
    [CommandSwitch("--by-resource-arn")]
    public string? ByResourceArn { get; set; }

    [CommandSwitch("--by-state")]
    public string? ByState { get; set; }

    [CommandSwitch("--by-backup-vault-name")]
    public string? ByBackupVaultName { get; set; }

    [CommandSwitch("--by-created-before")]
    public long? ByCreatedBefore { get; set; }

    [CommandSwitch("--by-created-after")]
    public long? ByCreatedAfter { get; set; }

    [CommandSwitch("--by-resource-type")]
    public string? ByResourceType { get; set; }

    [CommandSwitch("--by-account-id")]
    public string? ByAccountId { get; set; }

    [CommandSwitch("--by-complete-after")]
    public long? ByCompleteAfter { get; set; }

    [CommandSwitch("--by-complete-before")]
    public long? ByCompleteBefore { get; set; }

    [CommandSwitch("--by-parent-job-id")]
    public string? ByParentJobId { get; set; }

    [CommandSwitch("--by-message-category")]
    public string? ByMessageCategory { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}