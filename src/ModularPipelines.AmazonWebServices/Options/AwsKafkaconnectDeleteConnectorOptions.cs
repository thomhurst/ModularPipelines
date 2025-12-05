using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafkaconnect", "delete-connector")]
public record AwsKafkaconnectDeleteConnectorOptions(
[property: CliOption("--connector-arn")] string ConnectorArn
) : AwsOptions
{
    [CliOption("--current-version")]
    public string? CurrentVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}