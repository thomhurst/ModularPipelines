using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "topics", "create")]
public record GcloudPubsubTopicsCreateOptions(
[property: PositionalArgument] string Topic
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--message-retention-duration")]
    public string? MessageRetentionDuration { get; set; }

    [CommandSwitch("--message-storage-policy-allowed-regions")]
    public string[]? MessageStoragePolicyAllowedRegions { get; set; }

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

    [CommandSwitch("--topic-encryption-key")]
    public string? TopicEncryptionKey { get; set; }

    [CommandSwitch("--topic-encryption-key-keyring")]
    public string? TopicEncryptionKeyKeyring { get; set; }

    [CommandSwitch("--topic-encryption-key-location")]
    public string? TopicEncryptionKeyLocation { get; set; }

    [CommandSwitch("--topic-encryption-key-project")]
    public string? TopicEncryptionKeyProject { get; set; }
}