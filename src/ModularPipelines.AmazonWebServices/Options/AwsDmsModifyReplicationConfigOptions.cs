using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "modify-replication-config")]
public record AwsDmsModifyReplicationConfigOptions(
[property: CliOption("--replication-config-arn")] string ReplicationConfigArn
) : AwsOptions
{
    [CliOption("--replication-config-identifier")]
    public string? ReplicationConfigIdentifier { get; set; }

    [CliOption("--replication-type")]
    public string? ReplicationType { get; set; }

    [CliOption("--table-mappings")]
    public string? TableMappings { get; set; }

    [CliOption("--replication-settings")]
    public string? ReplicationSettings { get; set; }

    [CliOption("--supplemental-settings")]
    public string? SupplementalSettings { get; set; }

    [CliOption("--compute-config")]
    public string? ComputeConfig { get; set; }

    [CliOption("--source-endpoint-arn")]
    public string? SourceEndpointArn { get; set; }

    [CliOption("--target-endpoint-arn")]
    public string? TargetEndpointArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}