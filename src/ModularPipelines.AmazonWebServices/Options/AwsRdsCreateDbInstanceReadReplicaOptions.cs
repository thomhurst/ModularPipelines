using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-db-instance-read-replica")]
public record AwsRdsCreateDbInstanceReadReplicaOptions(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CommandSwitch("--source-db-instance-identifier")]
    public string? SourceDbInstanceIdentifier { get; set; }

    [CommandSwitch("--db-instance-class")]
    public string? DbInstanceClass { get; set; }

    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--iops")]
    public int? Iops { get; set; }

    [CommandSwitch("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CommandSwitch("--db-parameter-group-name")]
    public string? DbParameterGroupName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--monitoring-interval")]
    public int? MonitoringInterval { get; set; }

    [CommandSwitch("--monitoring-role-arn")]
    public string? MonitoringRoleArn { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--pre-signed-url")]
    public string? PreSignedUrl { get; set; }

    [CommandSwitch("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CommandSwitch("--performance-insights-retention-period")]
    public int? PerformanceInsightsRetentionPeriod { get; set; }

    [CommandSwitch("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CommandSwitch("--processor-features")]
    public string[]? ProcessorFeatures { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CommandSwitch("--domain-fqdn")]
    public string? DomainFqdn { get; set; }

    [CommandSwitch("--domain-ou")]
    public string? DomainOu { get; set; }

    [CommandSwitch("--domain-auth-secret-arn")]
    public string? DomainAuthSecretArn { get; set; }

    [CommandSwitch("--domain-dns-ips")]
    public string[]? DomainDnsIps { get; set; }

    [CommandSwitch("--replica-mode")]
    public string? ReplicaMode { get; set; }

    [CommandSwitch("--max-allocated-storage")]
    public int? MaxAllocatedStorage { get; set; }

    [CommandSwitch("--custom-iam-instance-profile")]
    public string? CustomIamInstanceProfile { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--storage-throughput")]
    public int? StorageThroughput { get; set; }

    [CommandSwitch("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CommandSwitch("--source-db-cluster-identifier")]
    public string? SourceDbClusterIdentifier { get; set; }

    [CommandSwitch("--source-region")]
    public string? SourceRegion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}