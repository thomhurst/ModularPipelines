using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "create-replication-config")]
public record AwsDmsCreateReplicationConfigOptions(
[property: CliOption("--replication-config-identifier")] string ReplicationConfigIdentifier,
[property: CliOption("--source-endpoint-arn")] string SourceEndpointArn,
[property: CliOption("--target-endpoint-arn")] string TargetEndpointArn,
[property: CliOption("--compute-config")] string ComputeConfig,
[property: CliOption("--replication-type")] string ReplicationType,
[property: CliOption("--table-mappings")] string TableMappings
) : AwsOptions
{
    [CliOption("--replication-settings")]
    public string? ReplicationSettings { get; set; }

    [CliOption("--supplemental-settings")]
    public string? SupplementalSettings { get; set; }

    [CliOption("--resource-identifier")]
    public string? ResourceIdentifier { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}