using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "generate-data-key-pair")]
public record AwsKmsGenerateDataKeyPairOptions(
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--key-pair-spec")] string KeyPairSpec
) : AwsOptions
{
    [CommandSwitch("--encryption-context")]
    public IEnumerable<KeyValue>? EncryptionContext { get; set; }

    [CommandSwitch("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CommandSwitch("--recipient")]
    public string? Recipient { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}