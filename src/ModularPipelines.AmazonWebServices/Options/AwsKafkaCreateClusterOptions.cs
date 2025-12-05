using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "create-cluster")]
public record AwsKafkaCreateClusterOptions(
[property: CliOption("--broker-node-group-info")] string BrokerNodeGroupInfo,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--kafka-version")] string KafkaVersion,
[property: CliOption("--number-of-broker-nodes")] int NumberOfBrokerNodes
) : AwsOptions
{
    [CliOption("--client-authentication")]
    public string? ClientAuthentication { get; set; }

    [CliOption("--configuration-info")]
    public string? ConfigurationInfo { get; set; }

    [CliOption("--encryption-info")]
    public string? EncryptionInfo { get; set; }

    [CliOption("--enhanced-monitoring")]
    public string? EnhancedMonitoring { get; set; }

    [CliOption("--open-monitoring")]
    public string? OpenMonitoring { get; set; }

    [CliOption("--logging-info")]
    public string? LoggingInfo { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--storage-mode")]
    public string? StorageMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}