using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "encrypt")]
public record AwsKmsEncryptOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--plaintext")] string Plaintext
) : AwsOptions
{
    [CliOption("--encryption-context")]
    public IEnumerable<KeyValue>? EncryptionContext { get; set; }

    [CliOption("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CliOption("--encryption-algorithm")]
    public string? EncryptionAlgorithm { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}