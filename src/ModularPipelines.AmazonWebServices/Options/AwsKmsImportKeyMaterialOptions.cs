using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "import-key-material")]
public record AwsKmsImportKeyMaterialOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--import-token")] string ImportToken,
[property: CliOption("--encrypted-key-material")] string EncryptedKeyMaterial
) : AwsOptions
{
    [CliOption("--valid-to")]
    public long? ValidTo { get; set; }

    [CliOption("--expiration-model")]
    public string? ExpirationModel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}