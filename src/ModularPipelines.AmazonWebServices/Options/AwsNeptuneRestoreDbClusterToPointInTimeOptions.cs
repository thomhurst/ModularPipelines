using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "restore-db-cluster-to-point-in-time")]
public record AwsNeptuneRestoreDbClusterToPointInTimeOptions(
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier,
[property: CommandSwitch("--source-db-cluster-identifier")] string SourceDbClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--restore-type")]
    public string? RestoreType { get; set; }

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

    [CommandSwitch("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CommandSwitch("--db-cluster-parameter-group-name")]
    public string? DbClusterParameterGroupName { get; set; }

    [CommandSwitch("--serverless-v2-scaling-configuration")]
    public string? ServerlessV2ScalingConfiguration { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}