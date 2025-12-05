using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafkaconnect", "update-connector")]
public record AwsKafkaconnectUpdateConnectorOptions(
[property: CliOption("--capacity")] string Capacity,
[property: CliOption("--connector-arn")] string ConnectorArn,
[property: CliOption("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}