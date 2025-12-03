using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "generate-data-key-pair")]
public record AwsKmsGenerateDataKeyPairOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--key-pair-spec")] string KeyPairSpec
) : AwsOptions
{
    [CliOption("--encryption-context")]
    public IEnumerable<KeyValue>? EncryptionContext { get; set; }

    [CliOption("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CliOption("--recipient")]
    public string? Recipient { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}