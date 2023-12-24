using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "create-cluster")]
public record AwsKafkaCreateClusterOptions(
[property: CommandSwitch("--broker-node-group-info")] string BrokerNodeGroupInfo,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--kafka-version")] string KafkaVersion,
[property: CommandSwitch("--number-of-broker-nodes")] int NumberOfBrokerNodes
) : AwsOptions
{
    [CommandSwitch("--client-authentication")]
    public string? ClientAuthentication { get; set; }

    [CommandSwitch("--configuration-info")]
    public string? ConfigurationInfo { get; set; }

    [CommandSwitch("--encryption-info")]
    public string? EncryptionInfo { get; set; }

    [CommandSwitch("--enhanced-monitoring")]
    public string? EnhancedMonitoring { get; set; }

    [CommandSwitch("--open-monitoring")]
    public string? OpenMonitoring { get; set; }

    [CommandSwitch("--logging-info")]
    public string? LoggingInfo { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--storage-mode")]
    public string? StorageMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}