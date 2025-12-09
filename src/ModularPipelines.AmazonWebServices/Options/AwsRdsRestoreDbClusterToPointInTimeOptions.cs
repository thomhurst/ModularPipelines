using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "restore-db-cluster-to-point-in-time")]
public record AwsRdsRestoreDbClusterToPointInTimeOptions(
[property: CliOption("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CliOption("--restore-type")]
    public string? RestoreType { get; set; }

    [CliOption("--source-db-cluster-identifier")]
    public string? SourceDbClusterIdentifier { get; set; }

    [CliOption("--restore-to-time")]
    public long? RestoreToTime { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--backtrack-window")]
    public long? BacktrackWindow { get; set; }

    [CliOption("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CliOption("--db-cluster-parameter-group-name")]
    public string? DbClusterParameterGroupName { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CliOption("--scaling-configuration")]
    public string? ScalingConfiguration { get; set; }

    [CliOption("--engine-mode")]
    public string? EngineMode { get; set; }

    [CliOption("--db-cluster-instance-class")]
    public string? DbClusterInstanceClass { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--iops")]
    public int? Iops { get; set; }

    [CliOption("--serverless-v2-scaling-configuration")]
    public string? ServerlessV2ScalingConfiguration { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--source-db-cluster-resource-id")]
    public string? SourceDbClusterResourceId { get; set; }

    [CliOption("--rds-custom-cluster-configuration")]
    public string? RdsCustomClusterConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}