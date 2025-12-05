using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "generate-data-key-without-plaintext")]
public record AwsKmsGenerateDataKeyWithoutPlaintextOptions(
[property: CliOption("--key-id")] string KeyId
) : AwsOptions
{
    [CliOption("--encryption-context")]
    public IEnumerable<KeyValue>? EncryptionContext { get; set; }

    [CliOption("--key-spec")]
    public string? KeySpec { get; set; }

    [CliOption("--number-of-bytes")]
    public int? NumberOfBytes { get; set; }

    [CliOption("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}