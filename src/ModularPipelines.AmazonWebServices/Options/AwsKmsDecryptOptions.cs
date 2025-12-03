using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "decrypt")]
public record AwsKmsDecryptOptions(
[property: CliOption("--ciphertext-blob")] string CiphertextBlob
) : AwsOptions
{
    [CliOption("--encryption-context")]
    public IEnumerable<KeyValue>? EncryptionContext { get; set; }

    [CliOption("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CliOption("--key-id")]
    public string? KeyId { get; set; }

    [CliOption("--encryption-algorithm")]
    public string? EncryptionAlgorithm { get; set; }

    [CliOption("--recipient")]
    public string? Recipient { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}