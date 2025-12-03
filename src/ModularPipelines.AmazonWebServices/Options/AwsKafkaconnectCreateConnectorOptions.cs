using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafkaconnect", "create-connector")]
public record AwsKafkaconnectCreateConnectorOptions(
[property: CliOption("--capacity")] string Capacity,
[property: CliOption("--connector-configuration")] IEnumerable<KeyValue> ConnectorConfiguration,
[property: CliOption("--connector-name")] string ConnectorName,
[property: CliOption("--kafka-cluster")] string KafkaCluster,
[property: CliOption("--kafka-cluster-client-authentication")] string KafkaClusterClientAuthentication,
[property: CliOption("--kafka-cluster-encryption-in-transit")] string KafkaClusterEncryptionInTransit,
[property: CliOption("--kafka-connect-version")] string KafkaConnectVersion,
[property: CliOption("--plugins")] string[] Plugins,
[property: CliOption("--service-execution-role-arn")] string ServiceExecutionRoleArn
) : AwsOptions
{
    [CliOption("--connector-description")]
    public string? ConnectorDescription { get; set; }

    [CliOption("--log-delivery")]
    public string? LogDelivery { get; set; }

    [CliOption("--worker-configuration")]
    public string? WorkerConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}