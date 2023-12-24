using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "import-key-material")]
public record AwsKmsImportKeyMaterialOptions(
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--import-token")] string ImportToken,
[property: CommandSwitch("--encrypted-key-material")] string EncryptedKeyMaterial
) : AwsOptions
{
    [CommandSwitch("--valid-to")]
    public long? ValidTo { get; set; }

    [CommandSwitch("--expiration-model")]
    public string? ExpirationModel { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}