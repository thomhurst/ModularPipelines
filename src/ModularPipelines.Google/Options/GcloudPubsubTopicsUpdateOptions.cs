using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "update")]
public record GcloudPubsubTopicsUpdateOptions(
[property: PositionalArgument] string Topic
) : GcloudOptions
{
    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--clear-message-retention-duration")]
    public bool? ClearMessageRetentionDuration { get; set; }

    [CommandSwitch("--message-retention-duration")]
    public string? MessageRetentionDuration { get; set; }

    [BooleanCommandSwitch("--clear-schema-settings")]
    public bool? ClearSchemaSettings { get; set; }

    [CommandSwitch("--message-encoding")]
    public string? MessageEncoding { get; set; }

    [CommandSwitch("--first-revision-id")]
    public string? FirstRevisionId { get; set; }

    [CommandSwitch("--last-revision-id")]
    public string? LastRevisionId { get; set; }

    [CommandSwitch("--schema")]
    public string? Schema { get; set; }

    [CommandSwitch("--schema-project")]
    public string? SchemaProject { get; set; }

    [CommandSwitch("--message-storage-policy-allowed-regions")]
    public string[]? MessageStoragePolicyAllowedRegions { get; set; }

    [BooleanCommandSwitch("--recompute-message-storage-policy")]
    public bool? RecomputeMessageStoragePolicy { get; set; }

    [CommandSwitch("--topic-encryption-key")]
    public string? TopicEncryptionKey { get; set; }

    [CommandSwitch("--topic-encryption-key-keyring")]
    public string? TopicEncryptionKeyKeyring { get; set; }

    [CommandSwitch("--topic-encryption-key-location")]
    public string? TopicEncryptionKeyLocation { get; set; }

    [CommandSwitch("--topic-encryption-key-project")]
    public string? TopicEncryptionKeyProject { get; set; }
}