using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot-data", "publish")]
public record AwsIotDataPublishOptions(
[property: CliOption("--topic")] string Topic
) : AwsOptions
{
    [CliOption("--qos")]
    public int? Qos { get; set; }

    [CliOption("--payload")]
    public string? Payload { get; set; }

    [CliOption("--user-properties")]
    public string? UserProperties { get; set; }

    [CliOption("--payload-format-indicator")]
    public string? PayloadFormatIndicator { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--response-topic")]
    public string? ResponseTopic { get; set; }

    [CliOption("--correlation-data")]
    public string? CorrelationData { get; set; }

    [CliOption("--message-expiry")]
    public long? MessageExpiry { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}