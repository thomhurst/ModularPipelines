using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "jobs", "create")]
public record GcloudTransferJobsCreateOptions(
[property: CliArgument] string Source,
[property: CliArgument] string Destination
) : GcloudOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

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

    [CliFlag("--do-not-run")]
    public bool? DoNotRun { get; set; }

    [CliOption("--schedule-starts")]
    public string? ScheduleStarts { get; set; }

    [CliOption("--schedule-repeats-every")]
    public string? ScheduleRepeatsEvery { get; set; }

    [CliOption("--schedule-repeats-until")]
    public string? ScheduleRepeatsUntil { get; set; }

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

    [CliOption("--overwrite-when")]
    public string? OverwriteWhen { get; set; }

    [CliOption("--delete-from")]
    public string? DeleteFrom { get; set; }

    [CliOption("--preserve-metadata")]
    public string[]? PreserveMetadata { get; set; }

    [CliOption("--custom-storage-class")]
    public string? CustomStorageClass { get; set; }

    [CliOption("--notification-pubsub-topic")]
    public string? NotificationPubsubTopic { get; set; }

    [CliOption("--notification-event-types")]
    public string[]? NotificationEventTypes { get; set; }

    [CliOption("--notification-payload-format")]
    public string? NotificationPayloadFormat { get; set; }

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

    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }
}