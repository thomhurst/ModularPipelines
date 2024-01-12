using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "jobs", "update")]
public record GcloudTransferJobsUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [BooleanCommandSwitch("--clear-description")]
    public bool? ClearDescription { get; set; }

    [BooleanCommandSwitch("--clear-source-creds-file")]
    public bool? ClearSourceCredsFile { get; set; }

    [BooleanCommandSwitch("--clear-source-agent-pool")]
    public bool? ClearSourceAgentPool { get; set; }

    [BooleanCommandSwitch("--clear-destination-agent-pool")]
    public bool? ClearDestinationAgentPool { get; set; }

    [BooleanCommandSwitch("--clear-intermediate-storage-path")]
    public bool? ClearIntermediateStoragePath { get; set; }

    [BooleanCommandSwitch("--clear-manifest-file")]
    public bool? ClearManifestFile { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--source-creds-file")]
    public string? SourceCredsFile { get; set; }

    [CommandSwitch("--source-agent-pool")]
    public string? SourceAgentPool { get; set; }

    [CommandSwitch("--destination-agent-pool")]
    public string? DestinationAgentPool { get; set; }

    [CommandSwitch("--intermediate-storage-path")]
    public string? IntermediateStoragePath { get; set; }

    [CommandSwitch("--manifest-file")]
    public string? ManifestFile { get; set; }

    [CommandSwitch("--event-stream-name")]
    public string? EventStreamName { get; set; }

    [CommandSwitch("--event-stream-starts")]
    public string? EventStreamStarts { get; set; }

    [CommandSwitch("--event-stream-expires")]
    public string? EventStreamExpires { get; set; }

    [BooleanCommandSwitch("--clear-event-stream")]
    public bool? ClearEventStream { get; set; }

    [BooleanCommandSwitch("--clear-schedule")]
    public bool? ClearSchedule { get; set; }

    [CommandSwitch("--schedule-starts")]
    public string? ScheduleStarts { get; set; }

    [CommandSwitch("--schedule-repeats-every")]
    public string? ScheduleRepeatsEvery { get; set; }

    [CommandSwitch("--schedule-repeats-until")]
    public string? ScheduleRepeatsUntil { get; set; }

    [BooleanCommandSwitch("--clear-include-prefixes")]
    public bool? ClearIncludePrefixes { get; set; }

    [BooleanCommandSwitch("--clear-exclude-prefixes")]
    public bool? ClearExcludePrefixes { get; set; }

    [BooleanCommandSwitch("--clear-include-modified-before-absolute")]
    public bool? ClearIncludeModifiedBeforeAbsolute { get; set; }

    [BooleanCommandSwitch("--clear-include-modified-after-absolute")]
    public bool? ClearIncludeModifiedAfterAbsolute { get; set; }

    [BooleanCommandSwitch("--clear-include-modified-before-relative")]
    public bool? ClearIncludeModifiedBeforeRelative { get; set; }

    [BooleanCommandSwitch("--clear-include-modified-after-relative")]
    public bool? ClearIncludeModifiedAfterRelative { get; set; }

    [CommandSwitch("--include-prefixes")]
    public string[]? IncludePrefixes { get; set; }

    [CommandSwitch("--exclude-prefixes")]
    public string[]? ExcludePrefixes { get; set; }

    [CommandSwitch("--include-modified-before-absolute")]
    public string? IncludeModifiedBeforeAbsolute { get; set; }

    [CommandSwitch("--include-modified-after-absolute")]
    public string? IncludeModifiedAfterAbsolute { get; set; }

    [CommandSwitch("--include-modified-before-relative")]
    public string? IncludeModifiedBeforeRelative { get; set; }

    [CommandSwitch("--include-modified-after-relative")]
    public string? IncludeModifiedAfterRelative { get; set; }

    [BooleanCommandSwitch("--clear-delete-from")]
    public bool? ClearDeleteFrom { get; set; }

    [BooleanCommandSwitch("--clear-preserve-metadata")]
    public bool? ClearPreserveMetadata { get; set; }

    [BooleanCommandSwitch("--clear-custom-storage-class")]
    public bool? ClearCustomStorageClass { get; set; }

    [CommandSwitch("--overwrite-when")]
    public string? OverwriteWhen { get; set; }

    [CommandSwitch("--delete-from")]
    public string? DeleteFrom { get; set; }

    [CommandSwitch("--preserve-metadata")]
    public string[]? PreserveMetadata { get; set; }

    [CommandSwitch("--custom-storage-class")]
    public string? CustomStorageClass { get; set; }

    [BooleanCommandSwitch("--clear-notification-config")]
    public bool? ClearNotificationConfig { get; set; }

    [BooleanCommandSwitch("--clear-notification-event-types")]
    public bool? ClearNotificationEventTypes { get; set; }

    [CommandSwitch("--notification-pubsub-topic")]
    public string? NotificationPubsubTopic { get; set; }

    [CommandSwitch("--notification-event-types")]
    public string[]? NotificationEventTypes { get; set; }

    [CommandSwitch("--notification-payload-format")]
    public string? NotificationPayloadFormat { get; set; }

    [BooleanCommandSwitch("--clear-log-config")]
    public bool? ClearLogConfig { get; set; }

    [CommandSwitch("--[no-]enable-posix-transfer-logs")]
    public string[]? NoEnablePosixTransferLogs { get; set; }

    [CommandSwitch("--log-actions")]
    public string[]? LogActions { get; set; }

    [CommandSwitch("--log-action-states")]
    public string[]? LogActionStates { get; set; }

    [CommandSwitch("--source-endpoint")]
    public string? SourceEndpoint { get; set; }

    [CommandSwitch("--source-signing-region")]
    public string? SourceSigningRegion { get; set; }

    [CommandSwitch("--source-auth-method")]
    public string? SourceAuthMethod { get; set; }

    [CommandSwitch("--source-list-api")]
    public string? SourceListApi { get; set; }

    [CommandSwitch("--source-network-protocol")]
    public string? SourceNetworkProtocol { get; set; }

    [CommandSwitch("--source-request-model")]
    public string? SourceRequestModel { get; set; }

    [BooleanCommandSwitch("--clear-source-endpoint")]
    public bool? ClearSourceEndpoint { get; set; }

    [BooleanCommandSwitch("--clear-source-signing-region")]
    public bool? ClearSourceSigningRegion { get; set; }

    [BooleanCommandSwitch("--clear-source-auth-method")]
    public bool? ClearSourceAuthMethod { get; set; }

    [BooleanCommandSwitch("--clear-source-list-api")]
    public bool? ClearSourceListApi { get; set; }

    [BooleanCommandSwitch("--clear-source-network-protocol")]
    public bool? ClearSourceNetworkProtocol { get; set; }

    [BooleanCommandSwitch("--clear-source-request-model")]
    public bool? ClearSourceRequestModel { get; set; }
}