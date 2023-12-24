using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "restore-db-cluster-to-point-in-time")]
public record AwsRdsRestoreDbClusterToPointInTimeOptions(
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--restore-type")]
    public string? RestoreType { get; set; }

    [CommandSwitch("--source-db-cluster-identifier")]
    public string? SourceDbClusterIdentifier { get; set; }

    [CommandSwitch("--restore-to-time")]
    public long? RestoreToTime { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CommandSwitch("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--backtrack-window")]
    public long? BacktrackWindow { get; set; }

    [CommandSwitch("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CommandSwitch("--db-cluster-parameter-group-name")]
    public string? DbClusterParameterGroupName { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CommandSwitch("--scaling-configuration")]
    public string? ScalingConfiguration { get; set; }

    [CommandSwitch("--engine-mode")]
    public string? EngineMode { get; set; }

    [CommandSwitch("--db-cluster-instance-class")]
    public string? DbClusterInstanceClass { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--iops")]
    public int? Iops { get; set; }

    [CommandSwitch("--serverless-v2-scaling-configuration")]
    public string? ServerlessV2ScalingConfiguration { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--source-db-cluster-resource-id")]
    public string? SourceDbClusterResourceId { get; set; }

    [CommandSwitch("--rds-custom-cluster-configuration")]
    public string? RdsCustomClusterConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}