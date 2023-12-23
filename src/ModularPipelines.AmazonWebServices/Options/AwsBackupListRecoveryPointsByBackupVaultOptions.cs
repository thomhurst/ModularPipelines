using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "list-recovery-points-by-backup-vault")]
public record AwsBackupListRecoveryPointsByBackupVaultOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CommandSwitch("--backup-vault-account-id")]
    public string? BackupVaultAccountId { get; set; }

    [CommandSwitch("--by-resource-arn")]
    public string? ByResourceArn { get; set; }

    [CommandSwitch("--by-resource-type")]
    public string? ByResourceType { get; set; }

    [CommandSwitch("--by-backup-plan-id")]
    public string? ByBackupPlanId { get; set; }

    [CommandSwitch("--by-created-before")]
    public long? ByCreatedBefore { get; set; }

    [CommandSwitch("--by-created-after")]
    public long? ByCreatedAfter { get; set; }

    [CommandSwitch("--by-parent-recovery-point-arn")]
    public string? ByParentRecoveryPointArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}