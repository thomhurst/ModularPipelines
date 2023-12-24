using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "restore-db-cluster-to-point-in-time")]
public record AwsDocdbRestoreDbClusterToPointInTimeOptions(
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

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}