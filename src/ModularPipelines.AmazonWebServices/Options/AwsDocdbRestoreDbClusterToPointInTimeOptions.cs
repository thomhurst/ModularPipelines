using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "restore-db-cluster-to-point-in-time")]
public record AwsDocdbRestoreDbClusterToPointInTimeOptions(
[property: CliOption("--db-cluster-identifier")] string DbClusterIdentifier,
[property: CliOption("--source-db-cluster-identifier")] string SourceDbClusterIdentifier
) : AwsOptions
{
    [CliOption("--restore-type")]
    public string? RestoreType { get; set; }

    [CliOption("--restore-to-time")]
    public long? RestoreToTime { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--enable-cloudwatch-logs-exports")]
    public string[]? EnableCloudwatchLogsExports { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}