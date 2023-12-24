using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "decrypt")]
public record AwsKmsDecryptOptions(
[property: CommandSwitch("--ciphertext-blob")] string CiphertextBlob
) : AwsOptions
{
    [CommandSwitch("--encryption-context")]
    public IEnumerable<KeyValue>? EncryptionContext { get; set; }

    [CommandSwitch("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CommandSwitch("--key-id")]
    public string? KeyId { get; set; }

    [CommandSwitch("--encryption-algorithm")]
    public string? EncryptionAlgorithm { get; set; }

    [CommandSwitch("--recipient")]
    public string? Recipient { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}