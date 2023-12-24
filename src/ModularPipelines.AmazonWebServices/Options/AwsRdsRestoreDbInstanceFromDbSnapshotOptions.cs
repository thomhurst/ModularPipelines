using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "restore-db-instance-from-db-snapshot")]
public record AwsRdsRestoreDbInstanceFromDbSnapshotOptions(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CommandSwitch("--db-snapshot-identifier")]
    public string? DbSnapshotIdentifier { get; set; }

    [CommandSwitch("--db-instance-class")]
    public string? DbInstanceClass { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CommandSwitch("--license-model")]
    public string? LicenseModel { get; set; }

    [CommandSwitch("--db-name")]
    public string? DbName { get; set; }

    [CommandSwitch("--engine")]
    public string? Engine { get; set; }

    [CommandSwitch("--iops")]
    public int? Iops { get; set; }

    [CommandSwitch("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--tde-credential-arn")]
    public string? TdeCredentialArn { get; set; }

    [CommandSwitch("--tde-credential-password")]
    public string? TdeCredentialPassword { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--domain-fqdn")]
    public string? DomainFqdn { get; set; }

    [CommandSwitch("--domain-ou")]
    public string? DomainOu { get; set; }

    [CommandSwitch("--domain-auth-secret-arn")]
    public string? DomainAuthSecretArn { get; set; }

    [CommandSwitch("--domain-dns-ips")]
    public string[]? DomainDnsIps { get; set; }

    [CommandSwitch("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CommandSwitch("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CommandSwitch("--processor-features")]
    public string[]? ProcessorFeatures { get; set; }

    [CommandSwitch("--db-parameter-group-name")]
    public string? DbParameterGroupName { get; set; }

    [CommandSwitch("--custom-iam-instance-profile")]
    public string? CustomIamInstanceProfile { get; set; }

    [CommandSwitch("--backup-target")]
    public string? BackupTarget { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--storage-throughput")]
    public int? StorageThroughput { get; set; }

    [CommandSwitch("--db-cluster-snapshot-identifier")]
    public string? DbClusterSnapshotIdentifier { get; set; }

    [CommandSwitch("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}