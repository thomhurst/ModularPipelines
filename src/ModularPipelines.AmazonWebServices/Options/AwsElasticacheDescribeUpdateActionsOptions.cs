using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "describe-update-actions")]
public record AwsElasticacheDescribeUpdateActionsOptions : AwsOptions
{
    [CommandSwitch("--service-update-name")]
    public string? ServiceUpdateName { get; set; }

    [CommandSwitch("--replication-group-ids")]
    public string[]? ReplicationGroupIds { get; set; }

    [CommandSwitch("--cache-cluster-ids")]
    public string[]? CacheClusterIds { get; set; }

    [CommandSwitch("--engine")]
    public string? Engine { get; set; }

    [CommandSwitch("--service-update-status")]
    public string[]? ServiceUpdateStatus { get; set; }

    [CommandSwitch("--service-update-time-range")]
    public string? ServiceUpdateTimeRange { get; set; }

    [CommandSwitch("--update-action-status")]
    public string[]? UpdateActionStatus { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}