using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "create-replication-config")]
public record AwsDmsCreateReplicationConfigOptions(
[property: CommandSwitch("--replication-config-identifier")] string ReplicationConfigIdentifier,
[property: CommandSwitch("--source-endpoint-arn")] string SourceEndpointArn,
[property: CommandSwitch("--target-endpoint-arn")] string TargetEndpointArn,
[property: CommandSwitch("--compute-config")] string ComputeConfig,
[property: CommandSwitch("--replication-type")] string ReplicationType,
[property: CommandSwitch("--table-mappings")] string TableMappings
) : AwsOptions
{
    [CommandSwitch("--replication-settings")]
    public string? ReplicationSettings { get; set; }

    [CommandSwitch("--supplemental-settings")]
    public string? SupplementalSettings { get; set; }

    [CommandSwitch("--resource-identifier")]
    public string? ResourceIdentifier { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}