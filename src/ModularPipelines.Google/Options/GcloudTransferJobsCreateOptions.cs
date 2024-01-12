using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "jobs", "create")]
public record GcloudTransferJobsCreateOptions(
[property: PositionalArgument] string Source,
[property: PositionalArgument] string Destination
) : GcloudOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

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

    [BooleanCommandSwitch("--do-not-run")]
    public bool? DoNotRun { get; set; }

    [CommandSwitch("--schedule-starts")]
    public string? ScheduleStarts { get; set; }

    [CommandSwitch("--schedule-repeats-every")]
    public string? ScheduleRepeatsEvery { get; set; }

    [CommandSwitch("--schedule-repeats-until")]
    public string? ScheduleRepeatsUntil { get; set; }

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

    [CommandSwitch("--overwrite-when")]
    public string? OverwriteWhen { get; set; }

    [CommandSwitch("--delete-from")]
    public string? DeleteFrom { get; set; }

    [CommandSwitch("--preserve-metadata")]
    public string[]? PreserveMetadata { get; set; }

    [CommandSwitch("--custom-storage-class")]
    public string? CustomStorageClass { get; set; }

    [CommandSwitch("--notification-pubsub-topic")]
    public string? NotificationPubsubTopic { get; set; }

    [CommandSwitch("--notification-event-types")]
    public string[]? NotificationEventTypes { get; set; }

    [CommandSwitch("--notification-payload-format")]
    public string? NotificationPayloadFormat { get; set; }

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

    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }
}