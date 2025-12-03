using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "jobs", "update")]
public record GcloudTransferJobsUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliFlag("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CliFlag("--clear-source-creds-file")]
    public bool? ClearSourceCredsFile { get; set; }

    [CliFlag("--clear-source-agent-pool")]
    public bool? ClearSourceAgentPool { get; set; }

    [CliFlag("--clear-destination-agent-pool")]
    public bool? ClearDestinationAgentPool { get; set; }

    [CliFlag("--clear-intermediate-storage-path")]
    public bool? ClearIntermediateStoragePath { get; set; }

    [CliFlag("--clear-manifest-file")]
    public bool? ClearManifestFile { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--source-creds-file")]
    public string? SourceCredsFile { get; set; }

    [CliOption("--source-agent-pool")]
    public string? SourceAgentPool { get; set; }

    [CliOption("--destination-agent-pool")]
    public string? DestinationAgentPool { get; set; }

    [CliOption("--intermediate-storage-path")]
    public string? IntermediateStoragePath { get; set; }

    [CliOption("--manifest-file")]
    public string? ManifestFile { get; set; }

    [CliOption("--event-stream-name")]
    public string? EventStreamName { get; set; }

    [CliOption("--event-stream-starts")]
    public string? EventStreamStarts { get; set; }

    [CliOption("--event-stream-expires")]
    public string? EventStreamExpires { get; set; }

    [CliFlag("--clear-event-stream")]
    public bool? ClearEventStream { get; set; }

    [CliFlag("--clear-schedule")]
    public bool? ClearSchedule { get; set; }

    [CliOption("--schedule-starts")]
    public string? ScheduleStarts { get; set; }

    [CliOption("--schedule-repeats-every")]
    public string? ScheduleRepeatsEvery { get; set; }

    [CliOption("--schedule-repeats-until")]
    public string? ScheduleRepeatsUntil { get; set; }

    [CliFlag("--clear-include-prefixes")]
    public bool? ClearIncludePrefixes { get; set; }

    [CliFlag("--clear-exclude-prefixes")]
    public bool? ClearExcludePrefixes { get; set; }

    [CliFlag("--clear-include-modified-before-absolute")]
    public bool? ClearIncludeModifiedBeforeAbsolute { get; set; }

    [CliFlag("--clear-include-modified-after-absolute")]
    public bool? ClearIncludeModifiedAfterAbsolute { get; set; }

    [CliFlag("--clear-include-modified-before-relative")]
    public bool? ClearIncludeModifiedBeforeRelative { get; set; }

    [CliFlag("--clear-include-modified-after-relative")]
    public bool? ClearIncludeModifiedAfterRelative { get; set; }

    [CliOption("--include-prefixes")]
    public string[]? IncludePrefixes { get; set; }

    [CliOption("--exclude-prefixes")]
    public string[]? ExcludePrefixes { get; set; }

    [CliOption("--include-modified-before-absolute")]
    public string? IncludeModifiedBeforeAbsolute { get; set; }

    [CliOption("--include-modified-after-absolute")]
    public string? IncludeModifiedAfterAbsolute { get; set; }

    [CliOption("--include-modified-before-relative")]
    public string? IncludeModifiedBeforeRelative { get; set; }

    [CliOption("--include-modified-after-relative")]
    public string? IncludeModifiedAfterRelative { get; set; }

    [CliFlag("--clear-delete-from")]
    public bool? ClearDeleteFrom { get; set; }

    [CliFlag("--clear-preserve-metadata")]
    public bool? ClearPreserveMetadata { get; set; }

    [CliFlag("--clear-custom-storage-class")]
    public bool? ClearCustomStorageClass { get; set; }

    [CliOption("--overwrite-when")]
    public string? OverwriteWhen { get; set; }

    [CliOption("--delete-from")]
    public string? DeleteFrom { get; set; }

    [CliOption("--preserve-metadata")]
    public string[]? PreserveMetadata { get; set; }

    [CliOption("--custom-storage-class")]
    public string? CustomStorageClass { get; set; }

    [CliFlag("--clear-notification-config")]
    public bool? ClearNotificationConfig { get; set; }

    [CliFlag("--clear-notification-event-types")]
    public bool? ClearNotificationEventTypes { get; set; }

    [CliOption("--notification-pubsub-topic")]
    public string? NotificationPubsubTopic { get; set; }

    [CliOption("--notification-event-types")]
    public string[]? NotificationEventTypes { get; set; }

    [CliOption("--notification-payload-format")]
    public string? NotificationPayloadFormat { get; set; }

    [CliFlag("--clear-log-config")]
    public bool? ClearLogConfig { get; set; }

    [CliOption("--[no-]enable-posix-transfer-logs")]
    public string[]? NoEnablePosixTransferLogs { get; set; }

    [CliOption("--log-actions")]
    public string[]? LogActions { get; set; }

    [CliOption("--log-action-states")]
    public string[]? LogActionStates { get; set; }

    [CliOption("--source-endpoint")]
    public string? SourceEndpoint { get; set; }

    [CliOption("--source-signing-region")]
    public string? SourceSigningRegion { get; set; }

    [CliOption("--source-auth-method")]
    public string? SourceAuthMethod { get; set; }

    [CliOption("--source-list-api")]
    public string? SourceListApi { get; set; }

    [CliOption("--source-network-protocol")]
    public string? SourceNetworkProtocol { get; set; }

    [CliOption("--source-request-model")]
    public string? SourceRequestModel { get; set; }

    [CliFlag("--clear-source-endpoint")]
    public bool? ClearSourceEndpoint { get; set; }

    [CliFlag("--clear-source-signing-region")]
    public bool? ClearSourceSigningRegion { get; set; }

    [CliFlag("--clear-source-auth-method")]
    public bool? ClearSourceAuthMethod { get; set; }

    [CliFlag("--clear-source-list-api")]
    public bool? ClearSourceListApi { get; set; }

    [CliFlag("--clear-source-network-protocol")]
    public bool? ClearSourceNetworkProtocol { get; set; }

    [CliFlag("--clear-source-request-model")]
    public bool? ClearSourceRequestModel { get; set; }
}