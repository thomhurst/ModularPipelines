using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "re-encrypt")]
public record AwsKmsReEncryptOptions(
[property: CliOption("--ciphertext-blob")] string CiphertextBlob,
[property: CliOption("--destination-key-id")] string DestinationKeyId
) : AwsOptions
{
    [CliOption("--source-encryption-context")]
    public IEnumerable<KeyValue>? SourceEncryptionContext { get; set; }

    [CliOption("--source-key-id")]
    public string? SourceKeyId { get; set; }

    [CliOption("--destination-encryption-context")]
    public IEnumerable<KeyValue>? DestinationEncryptionContext { get; set; }

    [CliOption("--source-encryption-algorithm")]
    public string? SourceEncryptionAlgorithm { get; set; }

    [CliOption("--destination-encryption-algorithm")]
    public string? DestinationEncryptionAlgorithm { get; set; }

    [CliOption("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}