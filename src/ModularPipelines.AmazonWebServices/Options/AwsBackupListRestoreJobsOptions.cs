using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "list-restore-jobs")]
public record AwsBackupListRestoreJobsOptions : AwsOptions
{
    [CliOption("--by-account-id")]
    public string? ByAccountId { get; set; }

    [CliOption("--by-resource-type")]
    public string? ByResourceType { get; set; }

    [CliOption("--by-created-before")]
    public long? ByCreatedBefore { get; set; }

    [CliOption("--by-created-after")]
    public long? ByCreatedAfter { get; set; }

    [CliOption("--by-status")]
    public string? ByStatus { get; set; }

    [CliOption("--by-complete-before")]
    public long? ByCompleteBefore { get; set; }

    [CliOption("--by-complete-after")]
    public long? ByCompleteAfter { get; set; }

    [CliOption("--by-restore-testing-plan-arn")]
    public string? ByRestoreTestingPlanArn { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}