using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot-data", "publish")]
public record AwsIotDataPublishOptions(
[property: CommandSwitch("--topic")] string Topic
) : AwsOptions
{
    [CommandSwitch("--qos")]
    public int? Qos { get; set; }

    [CommandSwitch("--payload")]
    public string? Payload { get; set; }

    [CommandSwitch("--user-properties")]
    public string? UserProperties { get; set; }

    [CommandSwitch("--payload-format-indicator")]
    public string? PayloadFormatIndicator { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--response-topic")]
    public string? ResponseTopic { get; set; }

    [CommandSwitch("--correlation-data")]
    public string? CorrelationData { get; set; }

    [CommandSwitch("--message-expiry")]
    public long? MessageExpiry { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}