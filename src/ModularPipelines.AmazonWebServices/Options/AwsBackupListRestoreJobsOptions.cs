using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "list-restore-jobs")]
public record AwsBackupListRestoreJobsOptions : AwsOptions
{
    [CommandSwitch("--by-account-id")]
    public string? ByAccountId { get; set; }

    [CommandSwitch("--by-resource-type")]
    public string? ByResourceType { get; set; }

    [CommandSwitch("--by-created-before")]
    public long? ByCreatedBefore { get; set; }

    [CommandSwitch("--by-created-after")]
    public long? ByCreatedAfter { get; set; }

    [CommandSwitch("--by-status")]
    public string? ByStatus { get; set; }

    [CommandSwitch("--by-complete-before")]
    public long? ByCompleteBefore { get; set; }

    [CommandSwitch("--by-complete-after")]
    public long? ByCompleteAfter { get; set; }

    [CommandSwitch("--by-restore-testing-plan-arn")]
    public string? ByRestoreTestingPlanArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}