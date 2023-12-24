using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "start-replication")]
public record AwsDmsStartReplicationOptions(
[property: CommandSwitch("--replication-config-arn")] string ReplicationConfigArn,
[property: CommandSwitch("--start-replication-type")] string StartReplicationType
) : AwsOptions
{
    [CommandSwitch("--cdc-start-time")]
    public long? CdcStartTime { get; set; }

    [CommandSwitch("--cdc-start-position")]
    public string? CdcStartPosition { get; set; }

    [CommandSwitch("--cdc-stop-position")]
    public string? CdcStopPosition { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}