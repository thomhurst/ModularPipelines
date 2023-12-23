using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "modify-replication-config")]
public record AwsDmsModifyReplicationConfigOptions(
[property: CommandSwitch("--replication-config-arn")] string ReplicationConfigArn
) : AwsOptions
{
    [CommandSwitch("--replication-config-identifier")]
    public string? ReplicationConfigIdentifier { get; set; }

    [CommandSwitch("--replication-type")]
    public string? ReplicationType { get; set; }

    [CommandSwitch("--table-mappings")]
    public string? TableMappings { get; set; }

    [CommandSwitch("--replication-settings")]
    public string? ReplicationSettings { get; set; }

    [CommandSwitch("--supplemental-settings")]
    public string? SupplementalSettings { get; set; }

    [CommandSwitch("--compute-config")]
    public string? ComputeConfig { get; set; }

    [CommandSwitch("--source-endpoint-arn")]
    public string? SourceEndpointArn { get; set; }

    [CommandSwitch("--target-endpoint-arn")]
    public string? TargetEndpointArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}