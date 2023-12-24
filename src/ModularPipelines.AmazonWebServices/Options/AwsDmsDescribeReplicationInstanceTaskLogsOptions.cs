using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "describe-replication-instance-task-logs")]
public record AwsDmsDescribeReplicationInstanceTaskLogsOptions(
[property: CommandSwitch("--replication-instance-arn")] string ReplicationInstanceArn
) : AwsOptions
{
    [CommandSwitch("--max-records")]
    public int? MaxRecords { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}