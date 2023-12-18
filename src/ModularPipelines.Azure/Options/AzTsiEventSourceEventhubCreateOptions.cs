using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tsi", "event-source", "eventhub", "create")]
public record AzTsiEventSourceEventhubCreateOptions(
[property: CommandSwitch("--consumer-group-name")] string ConsumerGroupName,
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--event-hub-name")] string EventHubName,
[property: CommandSwitch("--event-source-name")] string EventSourceName,
[property: CommandSwitch("--event-source-resource-id")] string EventSourceResourceId,
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--namespace")] string Namespace,
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