using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "describe-replication-table-statistics")]
public record AwsDmsDescribeReplicationTableStatisticsOptions(
[property: CliOption("--replication-config-arn")] string ReplicationConfigArn
) : AwsOptions
{
    [CliOption("--max-records")]
    public int? MaxRecords { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}