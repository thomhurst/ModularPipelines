using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "topics", "create")]
public record GcloudPubsubTopicsCreateOptions(
[property: CliArgument] string Topic
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--message-retention-duration")]
    public string? MessageRetentionDuration { get; set; }

    [CliOption("--message-storage-policy-allowed-regions")]
    public string[]? MessageStoragePolicyAllowedRegions { get; set; }

    [CliOption("--message-encoding")]
    public string? MessageEncoding { get; set; }

    [CliOption("--first-revision-id")]
    public string? FirstRevisionId { get; set; }

    [CliOption("--last-revision-id")]
    public string? LastRevisionId { get; set; }

    [CliOption("--schema")]
    public string? Schema { get; set; }

    [CliOption("--schema-project")]
    public string? SchemaProject { get; set; }

    [CliOption("--topic-encryption-key")]
    public string? TopicEncryptionKey { get; set; }

    [CliOption("--topic-encryption-key-keyring")]
    public string? TopicEncryptionKeyKeyring { get; set; }

    [CliOption("--topic-encryption-key-location")]
    public string? TopicEncryptionKeyLocation { get; set; }

    [CliOption("--topic-encryption-key-project")]
    public string? TopicEncryptionKeyProject { get; set; }
}