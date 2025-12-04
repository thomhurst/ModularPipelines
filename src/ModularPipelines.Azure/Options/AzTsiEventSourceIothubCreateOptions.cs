using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tsi", "event-source", "iothub", "create")]
public record AzTsiEventSourceIothubCreateOptions(
[property: CliOption("--consumer-group-name")] string ConsumerGroupName,
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--event-source-name")] string EventSourceName,
[property: CliOption("--event-source-resource-id")] string EventSourceResourceId,
[property: CliOption("--iot-hub-name")] string IotHubName,
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--shared-access-key")] string SharedAccessKey
) : AzOptions
{
    [CliOption("--local-timestamp")]
    public string? LocalTimestamp { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--timestamp-property-name")]
    public string? TimestampPropertyName { get; set; }
}