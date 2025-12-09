using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-db-instance")]
public record AwsRdsCreateDbInstanceOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CliOption("--db-instance-class")] string DbInstanceClass,
[property: CliOption("--engine")] string Engine
) : AwsOptions
{
    [CliOption("--db-name")]
    public string? DbName { get; set; }

    [CliOption("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CliOption("--master-username")]
    public string? MasterUsername { get; set; }

    [CliOption("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CliOption("--db-security-groups")]
    public string[]? DbSecurityGroups { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--db-parameter-group-name")]
    public string? DbParameterGroupName { get; set; }

    [CliOption("--backup-retention-period")]
    public int? BackupRetentionPeriod { get; set; }

    [CliOption("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--license-model")]
    public string? LicenseModel { get; set; }

    [CliOption("--iops")]
    public int? Iops { get; set; }

    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--character-set-name")]
    public string? CharacterSetName { get; set; }

    [CliOption("--nchar-character-set-name")]
    public string? NcharCharacterSetName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--db-cluster-identifier")]
    public string? DbClusterIdentifier { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--tde-credential-arn")]
    public string? TdeCredentialArn { get; set; }

    [CliOption("--tde-credential-password")]
    public string? TdeCredentialPassword { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--domain-fqdn")]
    public string? DomainFqdn { get; set; }

    [CliOption("--domain-ou")]
    public string? DomainOu { get; set; }

    [CliOption("--domain-auth-secret-arn")]
    public string? DomainAuthSecretArn { get; set; }

    [CliOption("--domain-dns-ips")]
    public string[]? DomainDnsIps { get; set; }

    [CliOption("--monitoring-interval")]
    public int? MonitoringInterval { get; set; }

    [CliOption("--monitoring-role-arn")]
    public string? MonitoringRoleArn { get; set; }

    [CliOption("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CliOption("--promotion-tier")]
    public int? PromotionTier { get; set; }

    [CliOption("--timezone")]
    public string? Timezone { get; set; }

    [CliOption("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CliOption("--performance-insights-retention-period")]
    public int? PerformanceInsightsRetentionPeriod { get; set; }

    [CliOption("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CliOption("--processor-features")]
    public string[]? ProcessorFeatures { get; set; }

    [CliOption("--max-allocated-storage")]
    public int? MaxAllocatedStorage { get; set; }

    [CliOption("--custom-iam-instance-profile")]
    public string? CustomIamInstanceProfile { get; set; }

    [CliOption("--backup-target")]
    public string? BackupTarget { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--storage-throughput")]
    public int? StorageThroughput { get; set; }

    [CliOption("--master-user-secret-kms-key-id")]
    public string? MasterUserSecretKmsKeyId { get; set; }

    [CliOption("--ca-certificate-identifier")]
    public string? CaCertificateIdentifier { get; set; }

    [CliOption("--db-system-id")]
    public string? DbSystemId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}