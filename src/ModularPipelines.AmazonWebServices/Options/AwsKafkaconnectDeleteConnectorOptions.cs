using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafkaconnect", "delete-connector")]
public record AwsKafkaconnectDeleteConnectorOptions(
[property: CommandSwitch("--connector-arn")] string ConnectorArn
) : AwsOptions
{
    [CommandSwitch("--current-version")]
    public string? CurrentVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}