using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-db-instance-read-replica")]
public record AwsRdsCreateDbInstanceReadReplicaOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CliOption("--source-db-instance-identifier")]
    public string? SourceDbInstanceIdentifier { get; set; }

    [CliOption("--db-instance-class")]
    public string? DbInstanceClass { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--iops")]
    public int? Iops { get; set; }

    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--db-parameter-group-name")]
    public string? DbParameterGroupName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--monitoring-interval")]
    public int? MonitoringInterval { get; set; }

    [CliOption("--monitoring-role-arn")]
    public string? MonitoringRoleArn { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--pre-signed-url")]
    public string? PreSignedUrl { get; set; }

    [CliOption("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CliOption("--performance-insights-retention-period")]
    public int? PerformanceInsightsRetentionPeriod { get; set; }

    [CliOption("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CliOption("--processor-features")]
    public string[]? ProcessorFeatures { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CliOption("--domain-fqdn")]
    public string? DomainFqdn { get; set; }

    [CliOption("--domain-ou")]
    public string? DomainOu { get; set; }

    [CliOption("--domain-auth-secret-arn")]
    public string? DomainAuthSecretArn { get; set; }

    [CliOption("--domain-dns-ips")]
    public string[]? DomainDnsIps { get; set; }

    [CliOption("--replica-mode")]
    public string? ReplicaMode { get; set; }

    [CliOption("--max-allocated-storage")]
    public int? MaxAllocatedStorage { get; set; }

    [CliOption("--custom-iam-instance-profile")]
    public string? CustomIamInstanceProfile { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--storage-throughput")]
    public int? StorageThroughput { get; set; }

    [CliOption("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CliOption("--source-db-cluster-identifier")]
    public string? SourceDbClusterIdentifier { get; set; }

    [CliOption("--source-region")]
    public string? SourceRegion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}