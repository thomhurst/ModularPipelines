using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "restore-db-instance-from-db-snapshot")]
public record AwsRdsRestoreDbInstanceFromDbSnapshotOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CliOption("--db-snapshot-identifier")]
    public string? DbSnapshotIdentifier { get; set; }

    [CliOption("--db-instance-class")]
    public string? DbInstanceClass { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CliOption("--license-model")]
    public string? LicenseModel { get; set; }

    [CliOption("--db-name")]
    public string? DbName { get; set; }

    [CliOption("--engine")]
    public string? Engine { get; set; }

    [CliOption("--iops")]
    public int? Iops { get; set; }

    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--tde-credential-arn")]
    public string? TdeCredentialArn { get; set; }

    [CliOption("--tde-credential-password")]
    public string? TdeCredentialPassword { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

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

    [CliOption("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CliOption("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CliOption("--processor-features")]
    public string[]? ProcessorFeatures { get; set; }

    [CliOption("--db-parameter-group-name")]
    public string? DbParameterGroupName { get; set; }

    [CliOption("--custom-iam-instance-profile")]
    public string? CustomIamInstanceProfile { get; set; }

    [CliOption("--backup-target")]
    public string? BackupTarget { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--storage-throughput")]
    public int? StorageThroughput { get; set; }

    [CliOption("--db-cluster-snapshot-identifier")]
    public string? DbClusterSnapshotIdentifier { get; set; }

    [CliOption("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}