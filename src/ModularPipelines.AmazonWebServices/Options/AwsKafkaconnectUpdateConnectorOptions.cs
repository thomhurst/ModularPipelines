using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafkaconnect", "update-connector")]
public record AwsKafkaconnectUpdateConnectorOptions(
[property: CommandSwitch("--capacity")] string Capacity,
[property: CommandSwitch("--connector-arn")] string ConnectorArn,
[property: CommandSwitch("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}