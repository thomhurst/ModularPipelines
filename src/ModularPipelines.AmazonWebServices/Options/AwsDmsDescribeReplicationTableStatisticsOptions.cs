using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "describe-replication-table-statistics")]
public record AwsDmsDescribeReplicationTableStatisticsOptions(
[property: CommandSwitch("--replication-config-arn")] string ReplicationConfigArn
) : AwsOptions
{
    [CommandSwitch("--max-records")]
    public int? MaxRecords { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}