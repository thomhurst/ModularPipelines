using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "create-replication-instance")]
public record AwsDmsCreateReplicationInstanceOptions(
[property: CliOption("--replication-instance-identifier")] string ReplicationInstanceIdentifier,
[property: CliOption("--replication-instance-class")] string ReplicationInstanceClass
) : AwsOptions
{
    [CliOption("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--replication-subnet-group-identifier")]
    public string? ReplicationSubnetGroupIdentifier { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--dns-name-servers")]
    public string? DnsNameServers { get; set; }

    [CliOption("--resource-identifier")]
    public string? ResourceIdentifier { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}