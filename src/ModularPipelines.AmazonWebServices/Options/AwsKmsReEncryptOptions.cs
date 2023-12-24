using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "re-encrypt")]
public record AwsKmsReEncryptOptions(
[property: CommandSwitch("--ciphertext-blob")] string CiphertextBlob,
[property: CommandSwitch("--destination-key-id")] string DestinationKeyId
) : AwsOptions
{
    [CommandSwitch("--source-encryption-context")]
    public IEnumerable<KeyValue>? SourceEncryptionContext { get; set; }

    [CommandSwitch("--source-key-id")]
    public string? SourceKeyId { get; set; }

    [CommandSwitch("--destination-encryption-context")]
    public IEnumerable<KeyValue>? DestinationEncryptionContext { get; set; }

    [CommandSwitch("--source-encryption-algorithm")]
    public string? SourceEncryptionAlgorithm { get; set; }

    [CommandSwitch("--destination-encryption-algorithm")]
    public string? DestinationEncryptionAlgorithm { get; set; }

    [CommandSwitch("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}