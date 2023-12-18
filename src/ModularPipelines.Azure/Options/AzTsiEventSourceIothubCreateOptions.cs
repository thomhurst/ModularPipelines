using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tsi", "event-source", "iothub", "create")]
public record AzTsiEventSourceIothubCreateOptions(
[property: CommandSwitch("--consumer-group-name")] string ConsumerGroupName,
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--event-source-name")] string EventSourceName,
[property: CommandSwitch("--event-source-resource-id")] string EventSourceResourceId,
[property: CommandSwitch("--iot-hub-name")] string IotHubName,
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--shared-access-key")] string SharedAccessKey
) : AzOptions
{
    [CommandSwitch("--local-timestamp")]
    public string? LocalTimestamp { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--timestamp-property-name")]
    public string? TimestampPropertyName { get; set; }
}