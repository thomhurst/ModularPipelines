using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "list-recovery-points-by-backup-vault")]
public record AwsBackupListRecoveryPointsByBackupVaultOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CliOption("--backup-vault-account-id")]
    public string? BackupVaultAccountId { get; set; }

    [CliOption("--by-resource-arn")]
    public string? ByResourceArn { get; set; }

    [CliOption("--by-resource-type")]
    public string? ByResourceType { get; set; }

    [CliOption("--by-backup-plan-id")]
    public string? ByBackupPlanId { get; set; }

    [CliOption("--by-created-before")]
    public long? ByCreatedBefore { get; set; }

    [CliOption("--by-created-after")]
    public long? ByCreatedAfter { get; set; }

    [CliOption("--by-parent-recovery-point-arn")]
    public string? ByParentRecoveryPointArn { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}