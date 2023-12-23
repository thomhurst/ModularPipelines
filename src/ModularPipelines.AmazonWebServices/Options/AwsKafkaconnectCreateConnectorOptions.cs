using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafkaconnect", "create-connector")]
public record AwsKafkaconnectCreateConnectorOptions(
[property: CommandSwitch("--capacity")] string Capacity,
[property: CommandSwitch("--connector-configuration")] IEnumerable<KeyValue> ConnectorConfiguration,
[property: CommandSwitch("--connector-name")] string ConnectorName,
[property: CommandSwitch("--kafka-cluster")] string KafkaCluster,
[property: CommandSwitch("--kafka-cluster-client-authentication")] string KafkaClusterClientAuthentication,
[property: CommandSwitch("--kafka-cluster-encryption-in-transit")] string KafkaClusterEncryptionInTransit,
[property: CommandSwitch("--kafka-connect-version")] string KafkaConnectVersion,
[property: CommandSwitch("--plugins")] string[] Plugins,
[property: CommandSwitch("--service-execution-role-arn")] string ServiceExecutionRoleArn
) : AwsOptions
{
    [CommandSwitch("--connector-description")]
    public string? ConnectorDescription { get; set; }

    [CommandSwitch("--log-delivery")]
    public string? LogDelivery { get; set; }

    [CommandSwitch("--worker-configuration")]
    public string? WorkerConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}