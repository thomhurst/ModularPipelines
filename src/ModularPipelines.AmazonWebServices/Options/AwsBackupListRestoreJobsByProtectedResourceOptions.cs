using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "list-restore-jobs-by-protected-resource")]
public record AwsBackupListRestoreJobsByProtectedResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CliOption("--by-status")]
    public string? ByStatus { get; set; }

    [CliOption("--by-recovery-point-creation-date-after")]
    public long? ByRecoveryPointCreationDateAfter { get; set; }

    [CliOption("--by-recovery-point-creation-date-before")]
    public long? ByRecoveryPointCreationDateBefore { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}