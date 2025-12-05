using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "describe-update-actions")]
public record AwsElasticacheDescribeUpdateActionsOptions : AwsOptions
{
    [CliOption("--service-update-name")]
    public string? ServiceUpdateName { get; set; }

    [CliOption("--replication-group-ids")]
    public string[]? ReplicationGroupIds { get; set; }

    [CliOption("--cache-cluster-ids")]
    public string[]? CacheClusterIds { get; set; }

    [CliOption("--engine")]
    public string? Engine { get; set; }

    [CliOption("--service-update-status")]
    public string[]? ServiceUpdateStatus { get; set; }

    [CliOption("--service-update-time-range")]
    public string? ServiceUpdateTimeRange { get; set; }

    [CliOption("--update-action-status")]
    public string[]? UpdateActionStatus { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}